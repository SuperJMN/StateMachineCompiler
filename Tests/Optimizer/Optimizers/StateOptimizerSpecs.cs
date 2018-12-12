using System.Collections.Generic;
using Smc.Semantic;
using Xunit;

namespace Smc.Tests.Optimizer.Optimizers
{
    public class StateOptimizerSpecs
    {
        [Fact(Skip = "hola")]
        public void First()
        {
            var sut = new Optimize.Optimizer.SubTransitionOptimizer(new SemanticTransition()
            {
                Actions = new List<string>() { "first", "second"},
                NextState = new State("next"),
                Event = "event",
            }, new State("state"));

            var result = sut.Optimize();
        }

        [Fact(Skip = "hola")]
        public void Second()
        {

            var super = new State("super")
            {
                Transitions = {new SemanticTransition {NextState = null, Actions = {"a1"}},}
            };

            var state = new State("current") { SuperStates = { super }};

            var sut = new Optimize.Optimizer.SubTransitionOptimizer(new SemanticTransition(), state);

            var result = sut.Optimize();
        }
    }
}