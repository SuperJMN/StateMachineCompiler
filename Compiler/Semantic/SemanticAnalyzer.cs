using System;
using System.Collections.Generic;
using System.Linq;
using Smc.Syntax;

namespace Smc.Semantic
{
    public class SemanticAnalyzer
    {
        private SemanticStateMachine ast;
        private readonly AnalyzerHeader initialHeader = new AnalyzerHeader();
        private readonly AnalyzerHeader actionsHeader = new AnalyzerHeader();
        private readonly AnalyzerHeader fsmHeader = new AnalyzerHeader();

        public SemanticStateMachine Analize(FsmSyntax fsm)
        {
            ast = new SemanticStateMachine();

            AnalizeHeaders(fsm);
            CheckForErrorsAndWarnings(fsm);

            ProduceSemanticMachine(fsm);

            return ast;
        }

        private void ProduceSemanticMachine(FsmSyntax fsm)
        {
            if (ast.Errors.Any())
            {
                return;
            }

            var name = HeaderValue("initial", fsm);
            if (name != null)
            {
                ast.InitialState = new State(name);
            }

            ast.ActionClass = HeaderValue("actions", fsm);
            ast.FsmName = HeaderValue("fsm", fsm);

            var states = fsm.Logic.GroupBy(x => x.State.Name);
            ast.States = new SemanticStates(states.ToDictionary(x => x.Key, x => new State(x.Key)));
            ast.Events = fsm.Logic.SelectMany(x => x.Subtransitions).Select(y => y.Event).Where(x => x != null).ToHashSet();
            ast.Actions = fsm.Logic.SelectMany(x => x.Subtransitions).SelectMany(y => y.Actions)
                .Concat(fsm.Logic.SelectMany(x => x.State.EntryActions))
                .Concat(fsm.Logic.SelectMany(x => x.State.ExitActions))
                .ToHashSet();

            foreach (var transition in fsm.Logic)
            {
                var semanticState = CompileState(transition);
                CompileTransitions(semanticState, transition);
            }
        }

        private void CompileTransitions(State semanticState, Transition transition)
        {
            foreach (var st in transition.Subtransitions)
            {
                CompileTransition(st, semanticState);
            }
        }

        private void CompileTransition(Subtransition st, State state)
        {
            var transition = new SemanticTransition();

            transition.Actions.AddRange(st.Actions);

            transition.Event = st.Event;
            transition.NextState = st.NextState == null ? state : ast.States[st.NextState];

            state.Transitions.Add(transition);
        }

        private State CompileState(Transition transition)
        {
            var state = ast.States[transition.State.Name];

            foreach (var a in transition.State.EntryActions)
            {
                state.EntryActions.Add(a);
            }

            foreach (var a in transition.State.ExitActions)
            {
                state.ExitActions.Add(a);
            }

            foreach (var a in transition.State.Superstates)
            {
                state.SuperStates.Add(ast.States[a]);
            }

            state.IsAbstract = transition.State.IsAbstract;

            return state;
        }

        private static string HeaderValue(string name, FsmSyntax fsm)
        {
            return fsm.Headers.FirstOrDefault(x => string.Equals(x.Name, name, StringComparison.InvariantCultureIgnoreCase))?.Value;
        }

        private void CheckForErrorsAndWarnings(FsmSyntax fsm)
        {
            CreateStateEventAndActionLists(fsm);
            CheckUndefinedStates(fsm);
            CheckUnusedStates(fsm);
            CheckConflictingSuperstates(fsm);
            CheckDuplicateTransitions(fsm);
            CheckThatAbstractStatesAreNotTargets(fsm);
            CheckForMultiplyDefinedStateActions(fsm);
        }

        private void CheckForMultiplyDefinedStateActions(FsmSyntax fsm)
        {
            var groups = from t in fsm.Logic
                         let state = new { t.State.Name, t.State.EntryActions, t.State.ExitActions }
                         where t.State.EntryActions.Any() || t.State.ExitActions.Any()
                         group state by state.Name;

            foreach (var group in groups)
            {
                if (group.Select(t => new Set<string>(t.EntryActions)).Distinct().Count() > 1)
                {
                    ast.AddError(new AnalysisError(ErrorKind.StateActionsMultiplyDefined, group.Key));
                }

                if (group.Select(t => new Set<string>(t.ExitActions)).Distinct().Count() > 1)
                {
                    ast.AddError(new AnalysisError(ErrorKind.StateActionsMultiplyDefined, group.Key));
                }
            }
        }

        private void CheckThatAbstractStatesAreNotTargets(FsmSyntax fsm)
        {
            var abstractTargets = from t in fsm.Logic
                                  from st in t.Subtransitions
                                  let stateSpecIsAbstract = fsm.GetTransitionForNamedState(st.NextState)
                                  where stateSpecIsAbstract != null
                                  where stateSpecIsAbstract.State.IsAbstract
                                  select new { t, st };

            foreach (var target in abstractTargets)
            {
                var parameter = $"{target.t.State.Name}({target.st.Event})=>{target.st.NextState}";
                ast.AddError(new AnalysisError(ErrorKind.AbstractStateUsedAsNextState, parameter));
            }
        }

        private void CheckDuplicateTransitions(FsmSyntax fsm)
        {
            var keys = from t in fsm.Logic
                       from st in t.Subtransitions
                       group st by new { State = t.State.Name, st.Event } into grouped
                       select grouped;

            var duplicate = keys.Where(x => x.Count() > 1);

            foreach (var d in duplicate)
            {
                ast.AddError(new AnalysisError(ErrorKind.DuplicateTransition, $"{d.Key.State}({d.Key.Event})"));
            }
        }

        private void CheckConflictingSuperstates(FsmSyntax fsm)
        {
            foreach (var transition in fsm.Logic)
            {
                var baseTransitions = fsm.GetBaseTransitions(transition);
                var subtransitionsByEvent =
                    (from t in baseTransitions
                     from subt in t.Subtransitions
                     group subt by subt.Event
                    into byEvent
                     select byEvent).ToList();

                CheckSubtranstionForConflicts(subtransitionsByEvent, transition);
            }
        }

        private void CheckSubtranstionForConflicts(IEnumerable<IGrouping<string, Subtransition>> subtransitionsByEvent, Transition transition)
        {
            foreach (var subtransitionGroup in subtransitionsByEvent)
            {
                var transitionedStates = from st in subtransitionGroup
                                         let actions = new Set<string>(st.Actions)
                                         select new { State = st.NextState, Set = actions };

                if (transitionedStates.Distinct().Count() > 1)
                {
                    var detail = transition.State.Name + "|" + subtransitionGroup.Key;
                    ast.Errors.Add(new AnalysisError(ErrorKind.ConflictingSuperstates, detail));
                }
            }
        }

        private void CheckUnusedStates(FsmSyntax fsm)
        {
            var unused = fsm.DefinedStates().Except(fsm.UsedStates());

            foreach (var state in unused)
            {
                ast.Errors.Add(new AnalysisError(ErrorKind.UnusedState, state));
            }
        }

        private void CreateStateEventAndActionLists(FsmSyntax fsm)
        {
            AddStateNamesToStateList(fsm);
            AddEntryAndExitActionsToActionList(fsm);
            AddEventsToEventList(fsm);
            AddTransitionActionsToActionList(fsm);
        }

        private void AddTransitionActionsToActionList(FsmSyntax fsm)
        {
            var actions = from t in fsm.Logic
                          from st in t.Subtransitions
                          from action in st.Actions
                          select action;

            ast.Actions.UnionWith(actions);
        }

        private void AddEventsToEventList(FsmSyntax fsm)
        {
            var events = from t in fsm.Logic
                         from st in t.Subtransitions
                         where st.Event != null
                         select st.Event;

            ast.Events.UnionWith(events);
        }

        private void AddEntryAndExitActionsToActionList(FsmSyntax fsm)
        {
            var actions = from t in fsm.Logic
                          let implicitActions = t.State.EntryActions.Concat(t.State.ExitActions)
                          from action in implicitActions
                          select action;

            ast.Actions.UnionWith(actions);
        }

        private void AddStateNamesToStateList(FsmSyntax fsm)
        {
            var states = fsm.Logic.Select(x => new State(x.State.Name));

            foreach (var state in states)
            {
                ast.States[state.Name] = state;
            }
        }

        private void AnalizeHeaders(FsmSyntax fsm)
        {
            SetHeaders(fsm);
            CheckMissingHeaders();
        }

        private void CheckUndefinedStates(FsmSyntax fsm)
        {
            var initialState = initialHeader.Value;
            if (fsm.Logic.All(x => x.State.Name != initialState))
            {
                ast.AddError(new AnalysisError(ErrorKind.UndefinedState, "initial: " + initialState));
            }

            foreach (var superstate in fsm.Logic.SelectMany(x => x.State.Superstates))
            {
                CheckUndefinedState(superstate, ErrorKind.UndefinedSuperstate);
            }

            foreach (var superstate in fsm.Logic.SelectMany(t => t.Subtransitions.Select(st => st.NextState)))
            {
                CheckUndefinedState(superstate, ErrorKind.UndefinedState);
            }
        }

        private void CheckUndefinedState(string name, ErrorKind errorKind)
        {
            if (name == null)
            {
                return;
            }

            if (!ast.States.ContainsKey(name))
            {
                ast.Errors.Add(new AnalysisError(errorKind, name));
            }
        }

        private void CheckMissingHeaders()
        {
            if (initialHeader.IsEmpty)
            {
                ast.AddError(new AnalysisError(ErrorKind.NoInitial));
            }

            if (fsmHeader.IsEmpty)
            {
                ast.AddError(new AnalysisError(ErrorKind.NoFsm));
            }
        }

        private void SetHeaders(FsmSyntax fsm)
        {
            foreach (var header in fsm.Headers)
            {
                if (IsHeader(header, "initial"))
                {
                    SetHeader(header, initialHeader);
                }
                else if (IsHeader(header, "actions"))
                {
                    SetHeader(header, actionsHeader);
                }
                else if (IsHeader(header, "fsm"))
                {
                    SetHeader(header, fsmHeader);
                }
                else
                {
                    ast.AddError(new AnalysisError(ErrorKind.UnexpectedHeader, header));
                }
            }
        }

        private void SetHeader(Header header, AnalyzerHeader analyzerHeader)
        {
            if (!analyzerHeader.IsEmpty)
            {
                ast.AddError(new AnalysisError(ErrorKind.DuplicateHeader, header));
            }
            else
            {
                analyzerHeader.Name = header.Name;
                analyzerHeader.Value = header.Value;
            }
        }

        private bool IsHeader(Header header, string name)
        {
            return string.Equals(header.Name, name, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}