using System.Collections.Generic;
using System.Linq;
using Smc.Semantic;

namespace Smc.Optimize
{
    public partial class Optimizer
    {
        public class SubTransitionOptimizer
        {
            private readonly SemanticTransition semanticTransition;
            private OptimizedStateMachine.OptimizedSubtransition subTransition;
            private readonly State currentState;

            public SubTransitionOptimizer(SemanticTransition semanticTransition, State currentState)
            {
                this.semanticTransition = semanticTransition;
                this.currentState = currentState;
            }

            public OptimizedStateMachine.OptimizedSubtransition Optimize()
            {
                subTransition = new OptimizedStateMachine.OptimizedSubtransition
                {
                    Event = semanticTransition.Event,
                    NextState = semanticTransition.NextState.Name,
                    Actions = GetExitActions(currentState)
                        .Concat(GetEntryActions(semanticTransition.NextState))
                        .Concat(semanticTransition.Actions).ToList()
                };
                
                return subTransition;
            }

            private static IEnumerable<string> GetEntryActions(State state)
            {
                return state.HierarchyIncludingSelf().SelectMany(x => x.EntryActions);
            }

            private static IEnumerable<string> GetExitActions(State state)
            {
                return state.HierarchyIncludingSelf().Reverse().SelectMany(x => x.ExitActions);
            }
        }
    }
}