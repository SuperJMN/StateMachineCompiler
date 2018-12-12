using Smc.Semantic;

namespace Smc.Optimize
{
    public partial class Optimizer
    {
        private OptimizedStateMachine optimizedStateMachine;
        private SemanticStateMachine semanticStateMachine;

        public OptimizedStateMachine Optimize(SemanticStateMachine ast)
        {
            semanticStateMachine = ast;
            optimizedStateMachine = new OptimizedStateMachine();
            AddHeader(ast);
            AddLists();
            AddTransitions();
            return optimizedStateMachine;
        }

        private void AddTransitions()
        {
            foreach (var s in semanticStateMachine.States.Values)
            {
                if (!s.IsAbstract)
                {
                    optimizedStateMachine.Transitions.AddRange(new StateOptimizer(s).AddTransitionsForState());
                }
            }
        }

        private void AddLists()
        {
            AddStates();
            AddEvents();
            AddActions();
        }

        private void AddActions()
        {
            foreach (var s in semanticStateMachine.States.Values)
            {
                if (!s.IsAbstract)
                {
                    optimizedStateMachine.States.Add(s.Name);
                }
            }
        }

        private void AddEvents()
        {
            optimizedStateMachine.Events.AddRange(semanticStateMachine.Events);
        }

        private void AddStates()
        {
            optimizedStateMachine.Actions.AddRange(semanticStateMachine.Actions);
        }

        private void AddHeader(SemanticStateMachine ast)
        {
            optimizedStateMachine.Header = new OptimizedStateMachine.OpimizedHeader
            {
                Fsm = ast.FsmName,
                Initial = ast.InitialState.Name,
                Actions = ast.ActionClass
            };
        }
    }
}