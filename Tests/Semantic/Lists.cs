using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Equivalency;
using Smc.Semantic;
using Xunit;

namespace Smc.Tests.SemanticAnalyzer
{
    public class Lists : SpecsBase
    {
        private static EquivalencyAssertionOptions<State> Config(EquivalencyAssertionOptions<State> arg)
        {
            return arg.Including(x => x.Name);
        }

        [Fact]
        public void EntryAndExitActionsAreCountedAsActions()
        {
            var ast = ProduceAst("{s <ea >xa - - a}");

            ast.Actions.Should().Contain(new[] {"ea", "xa"});
            ast.Actions.Count.Should().Be(3);
        }

        [Fact]
        public void ManyActionsButNoDuplicates()
        {
            var ast = ProduceAst("{s1 e1 - {a1 a2} s2 e2 - {a3 a1}}");

            ast.Actions.Should().Contain(new[] {"a1", "a2", "a3"});
            ast.Actions.Count.Should().Be(3);
        }

        [Fact]
        public void ManyEvents()
        {
            var ast = ProduceAst("{s1 e1 - - s2 e2 - - s3 e3 - -}");

            ast.Events.Should().Contain("e1");
            ast.Events.Should().Contain("e2");
            ast.Events.Should().Contain("e3");
        }

        [Fact]
        public void ManyEventsButNoDuplicates()
        {
            var ast = ProduceAst("{s1 e1 - - s2 e2 - - s3 e1 - -}");

            ast.Events.Should().Contain(new[] {"e1", "e2"});
            ast.Events.Count.Should().Be(2);
        }

        [Fact]
        public void ManyStates()
        {
            var list = new List<State> {new State("s1"), new State("s3"), new State("s2")};
            ProduceAst("{s1 - - - s2 - - - s3 - - -}").States.Values
                .Should()
                .BeEquivalentTo(list, Config);
        }

        [Fact]
        public void NoNullEvents()
        {
            ProduceAst("{(s1) - - -}").Events
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void OneState()
        {
            var list = new List<State> {new State("s1")};
            ProduceAst("{s1 - - -}").States.Values
                .Should()
                .BeEquivalentTo(list, Config);
        }

        [Fact]
        public void StatesAreKeyedByName()
        {
            var ast = ProduceAst("{s1 - - - s2 - - - s3 - - -}");

            ast.States["s1"].Should().BeEquivalentTo(new State("s1"), Config);
            ast.States["s2"].Should().BeEquivalentTo(new State("s2"), Config);
            ast.States["s3"].Should().BeEquivalentTo(new State("s3"), Config);
        }
    }
}