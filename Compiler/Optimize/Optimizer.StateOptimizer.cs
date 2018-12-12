using System.Collections.Generic;
using System.Linq;
using Smc.Semantic;

namespace Smc.Optimize
{
    public partial class Optimizer
    {
        private class StateOptimizer
        {
            private readonly State currentState;
            private readonly ISet<string> eventsForThisState = new HashSet<string>();

            private readonly List<OptimizedStateMachine.OptimizedTransition> transitions =
                new List<OptimizedStateMachine.OptimizedTransition>();

            public StateOptimizer(State currentState)
            {
                this.currentState = currentState;
            }

            public IEnumerable<OptimizedStateMachine.OptimizedTransition> AddTransitionsForState()
            {
                var transition = new OptimizedStateMachine.OptimizedTransition
                {
                    CurrentState = currentState.Name
                };

                AddSubTransitions(transition);
                transitions.Add(transition);
                return transitions;
            }

            private void AddSubTransitions(OptimizedStateMachine.OptimizedTransition transition)
            {
                foreach (var stateInHierarchy in currentState.HierarchyIncludingSelf().Reverse())
                {
                    AddStateTransitions(transition, stateInHierarchy);
                }
            }

            private void AddStateTransitions(OptimizedStateMachine.OptimizedTransition transition, State state)
            {
                foreach (var semanticTransition in state.Transitions)
                {
                    if (EventExistsAndHasNotBeenOverridden(semanticTransition.Event))
                    {
                        AddSubTransition(semanticTransition, transition);
                    }
                }
            }

            private void AddSubTransition(SemanticTransition semanticTransition,
                OptimizedStateMachine.OptimizedTransition transition)
            {
                eventsForThisState.Add(semanticTransition.Event);
                var subTransition = new SubTransitionOptimizer(semanticTransition, currentState).Optimize();
                transition.Subtransitions.Add(subTransition);
            }

            private bool EventExistsAndHasNotBeenOverridden(string ev)
            {
                return ev != null && !eventsForThisState.Contains(ev);
            }
        }
    }
}