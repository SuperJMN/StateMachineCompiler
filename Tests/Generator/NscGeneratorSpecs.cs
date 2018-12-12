using System.Text;
using FluentAssertions;
using Smc.Generator;
using Smc.Syntax;
using Xunit;

namespace Smc.Tests.Generator
{
    public class NscGeneratorSpecs
    {
        [Fact]
        public void StatesAndEvents()
        {
            AssertGenerated(
                "" +
                "{" +
                "  I e1 S a1 " +
                "  I e2 - a2" +
                "  S e1 I a3" +
                "  S e2 - a4" +
                "}",
                "enum StateEnum [I, S] enum Event [e1, e2] ", new EnumVisitor());
        }

        [Fact]
        public void EventDelegatorsAreGenerated()
        {
            AssertGenerated(
                "" +
                "{" +
                "  I e1 S a1 " +
                "  I e2 - a2" +
                "  S e1 I a3" +
                "  S e2 - a4" +
                "}",
                "delegators [e1, e2] ", new EventDelegatorVisitor());
        }

        [Fact]
        public void FsmClassNodeIsGenerated()
        {
            AssertGenerated(
                "{I e I a}",
                "class f:acts {d e e p he sc}", new FsmClassVisitor());
        }

        private class FsmClassVisitor : EmptyVisitor
        {
            public override void Visit(FsmNode node)
            {
                StringBuilder.Append($"class {node.ClassName}:{node.ActionsName} {{");
                node.Delegators.Accept(this);
                node.States.Accept(this);
                node.Events.Accept(this);
                node.Initial.Accept(this);
                node.HandleEvent.Accept(this);
                StringBuilder.Append("}");
            }

            public override void Visit(SwitchCaseNode node)
            {
                StringBuilder.Append("sc");
            }

            public override void Visit(StatePropertyNode node)
            {
                StringBuilder.Append("p ");
            }

            public override void Visit(FunctionCallNode node)
            {
                StringBuilder.Append("fc");
            }

            public override void Visit(EnumeratorNode node)
            {
                StringBuilder.Append("en");
            }

            public override void Visit(EnumNode node)
            {
                StringBuilder.Append("e ");
            }

            public override void Visit(EventDelegatorsNode node)
            {
                StringBuilder.Append("d ");
            }

            public override void Visit(HandleEventNode node)
            {
                StringBuilder.Append("he ");
                node.SwitchCaseNode.Accept(this);
            }
        }

        [Fact]
        public void HandleEventIsGenerated()
        {
            AssertGenerated("{I e I a}", "he(s)", new HandleEventVisitor());
        }

        [Fact]
        public void StatePropertyIsCreated()
        {
            AssertGenerated("{I e I a}", "state property = I", new StatePropertyVisitor());
        }

        [Fact]
        public void OneTransition()
        {
            AssertGenerated("{I e I a}", "s State {case I {s ev {case e {setState(StateEnum.I) a() } default(I);}}}", new TestVisitor());
        }

        [Fact]
        public void TwoTransitions()
        {
            AssertGenerated("{I e1 S a1 S e2 I a2}", 
                "" +
                "s State {" +
                "case I {s ev {case e1 {setState(StateEnum.S) a1() } default(I);}}" +
                "case S {s ev {case e2 {setState(StateEnum.I) a2() } default(S);}}" +
                "}", new TestVisitor());
        }

        [Fact]
        public void TwoStatesTwoEventsFourActions()
        {
            AssertGenerated("" +
                            "{" +
                            "  I e1 S a1 " +
                            "  I e2 - a2" +
                            "  S e1 I a3" +
                            "  S e2 - a4" +
                            "}", "" +
                                 "s State {" +
                                 "case I {s ev {case e1 {setState(StateEnum.S) a1() }" +
                                 "case e2 {setState(StateEnum.I) a2() } default(I);}}" +
                                 "case S {s ev {case e1 {setState(StateEnum.I) a3() }" +
                                 "case e2 {setState(StateEnum.S) a4() } default(S);}}}", new TestVisitor());
        }

        private void AssertWithHeader(string full, string expected, EmptyVisitor visitor)
        {
            var syntax = new Parser(Tokenizer.Create()).Parse(full);
            var semantic = new Semantic.SemanticAnalyzer().Analize(syntax);
            var machine = new Optimize.Optimizer().Optimize(semantic);
            var node = new NscGenerator().Generate(machine);
            node.Accept(visitor);
            visitor.Result.Should().Be(expected);
        }

        private void AssertGenerated(string transitionSyntax, string expected, EmptyVisitor visitor)
        {
            var header = "Initial: I FSM:f Actions:acts";
            var syntax = new Parser(Tokenizer.Create()).Parse(header + transitionSyntax);
            var semantic = new Semantic.SemanticAnalyzer().Analize(syntax);
            var machine = new Optimize.Optimizer().Optimize(semantic);
            var node = new NscGenerator().Generate(machine);
            node.Accept(visitor);
            visitor.Result.Should().Be(expected);
        }

        private class EnumVisitor : EmptyVisitor
        {
            public override void Visit(EnumNode node)
            {
                StringBuilder.Append($"enum {node.Name} [{string.Join(", ", node.Values)}] ");
            }
        }

        private abstract class EmptyVisitor : INodeVisitor
        {
            protected readonly StringBuilder StringBuilder = new StringBuilder();

            public virtual void Visit(EnumNode node)
            {
            }

            public virtual void Visit(FsmNode node)
            {
                node.Initial.Accept(this);
                node.States.Accept(this);
                node.Events.Accept(this);
                node.Delegators.Accept(this);
                node.HandleEvent.Accept(this);
            }

            public virtual void Visit(EventDelegatorsNode node)
            {
            }

            public virtual void Visit(SwitchCaseNode node)
            {
            }

            public virtual void Visit(HandleEventNode node)
            {
                node.SwitchCaseNode.Accept(this);
            }

            public virtual void Visit(CaseNode node)
            {
            }

            public virtual void Visit(EnumeratorNode node)
            {
                StringBuilder.Append($"{node.Enumeration}.{node.Enumerator}");
            }

            public virtual void Visit(FunctionCallNode node)
            {
            }

            public virtual void Visit(DefaultCaseNode node)
            {
                StringBuilder.Append($" default({node.State});");
            }

            public void Visit(SetterNode node)
            {
                StringBuilder.Append($"setState(");
                node.Node.Accept(this);
                StringBuilder.Append(") ");
            }

            public virtual void Visit(StatePropertyNode node)
            {
            }

            public string Result => StringBuilder.ToString();
        }

        private class TestVisitor : EmptyVisitor
        {
            public override void Visit(SwitchCaseNode node)
            {
                StringBuilder.Append($"s {node.VariableName} {{");

                foreach (var nodeCaseNode in node.CaseNodes)
                {
                    nodeCaseNode.Accept(this);
                }

                StringBuilder.Append("}");
            }

            public override void Visit(CaseNode node)
            {
                StringBuilder.Append($"case {node.CaseName}");
                StringBuilder.Append(" {");
                node.CaseActionNode.Accept(this);
                StringBuilder.Append("}");
            }

            public override void Visit(FunctionCallNode node)
            {
                StringBuilder.Append($"{node.Name}(");
                node.Node?.Accept(this);
                StringBuilder.Append(") ");
            }
        }

        private class EventDelegatorVisitor : EmptyVisitor
        {
            public override void Visit(EventDelegatorsNode node)
            {
                StringBuilder.Append($"delegators [{string.Join(", ", node.Events)}] ");
            }
        }

        private class HandleEventVisitor : EmptyVisitor
        {
            public override void Visit(SwitchCaseNode node)
            {
                StringBuilder.Append("s");
            }

            public override void Visit(HandleEventNode node)
            {
                StringBuilder.Append("he(");
                node.SwitchCaseNode.Accept(this);
                StringBuilder.Append(")");
            }
        }

        private class StatePropertyVisitor : EmptyVisitor
        {
            public override void Visit(StatePropertyNode node)
            {
                StringBuilder.Append($"state property = {node.InitialState}");
            }
        }
    }
}
