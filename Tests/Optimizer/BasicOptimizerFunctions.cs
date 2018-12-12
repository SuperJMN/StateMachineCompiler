using FluentAssertions;
using Xunit;

namespace Smc.Tests.Optimizer
{
    public class BasicOptimizerFunctions : OptimizerFunctionsBase
    {
        [Fact]
        public void AbstractStatesAreRemoved()
        {
            var sm = ProduceStateMachineWithHeader("{(b) - - - i:b e i -}");

            sm.States.Should().NotContain("b");
        }

        [Fact]
        public void ActionsArePreserved()
        {
            var sm = ProduceStateMachineWithHeader("{i e1 s a1 s e2 i a2}");

            sm.Actions.Should().Contain("a1", "a2");
        }

        [Fact]
        public void EventsArePreserved()
        {
            var sm = ProduceStateMachineWithHeader("{i e1 s - s e2 i -}");

            sm.Events.Should().Contain("e1", "e2");
        }

        [Fact]
        public void Header()
        {
            var sm = ProduceStateMachineWithHeader("{i e i -}");
            sm.Header.Fsm.Should().Be("f");
            sm.Header.Initial.Should().Be("i");
            sm.Header.Actions.Should().Be("a");
        }

        [Fact]
        public void SimpleStateMachine()
        {
            AssertOptimization("{i e i a1}",
                "" +
                "i {\n" +
                "  e i {a1}\n" +
                "}\n");
        }

        [Fact]
        public void StatesArePreserved()
        {
            var sm = ProduceStateMachineWithHeader("{i e s - s e i -}");

            sm.States.Should().Contain("i", "s");
        }
    }
}