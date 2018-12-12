using System;
using System.Collections.Generic;
using System.Linq;
using Smc.Syntax;

namespace Smc.Semantic
{
    public static class FsmSyntaxMixin
    {

        public static IEnumerable<string> DefinedStates(this FsmSyntax self)
        {
            return self.Logic.Select(x => x.State.Name);
        }

        public static IEnumerable<string> DefinedSuperStates(this FsmSyntax self)
        {
            return self.Logic.Where(x => x.State.IsAbstract).Select(x => x.State.Name);
        }

        public static IEnumerable<string> DefinedRegularStates(this FsmSyntax self)
        {
            return self.Logic.Where(x => !x.State.IsAbstract).Select(x => x.State.Name);
        }

        public static string InitialState(this FsmSyntax self)
        {
            return self.Headers.FirstOrDefault(x => String.Equals(x.Name, "initial", StringComparison.InvariantCultureIgnoreCase))?.Value;
        }

        public static IEnumerable<string> StatesUsedAsNextState(this FsmSyntax self)
        {
            return self.Logic.SelectMany(x => x.Subtransitions).Select(x => x.NextState).Distinct();
        }

        public static IEnumerable<string> StatesUsedAsSuperstate(this FsmSyntax self)
        {
            return self.Logic.SelectMany(x => x.State.Superstates).Distinct();
        }

        public static IEnumerable<string> UsedStates(this FsmSyntax self)
        {
            return new[] {self.InitialState()}
                .Concat(self.StatesUsedAsNextState())
                .Concat(self.ImplicitlyUsedStates())
                .Concat(self.StatesUsedAsSuperstate());
        }

        public static IEnumerable<string> ImplicitlyUsedStates(this FsmSyntax self)
        {
            return (from transition in self.Logic
                    from subtransition in transition.Subtransitions
                    where subtransition.NextState == null
                    select transition.State.Name).Distinct();
        }

        public static Transition GetTransitionForNamedState(this FsmSyntax fsm, string transitionName)
        {
            return fsm.Logic
                .FirstOrDefault(transition => string.Equals(transition.State.Name, transitionName, StringComparison.InvariantCultureIgnoreCase));
        }

        public static IEnumerable<Transition> GetBaseTransitions(this FsmSyntax fsmSyntax, Transition current)
        {
            return from superStateName in from superState in current.State.Superstates
                    select superState
                let baseTransition = fsmSyntax.GetTransitionForNamedState(superStateName)
                where baseTransition != null
                from baseAndRest in new[] {baseTransition}.Concat(GetBaseTransitions(fsmSyntax, baseTransition))
                select baseAndRest;
        }
    }
}