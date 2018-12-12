using System.Text;
using Smc.Generator;

namespace Smc.Tests.Implementers
{
    public class CsharpVisitor : INodeVisitor
    {
        private readonly StringBuilder stringBuilder = new StringBuilder();
        private string Indent => GetIndent(indentLevel);

        private static string GetIndent(int i)
        {
            var result = "";

            for (var j = 0; j < i; j++)
            {
                result += "\t";
            }

            return result;
        }

        private int indentLevel;

        public string Result => stringBuilder.ToString();

        public void Visit(EnumNode node)
        {
            var values = string.Join(", ", node.Values);
            IndentedAppendLine($"public enum {node.Name} {{{values}}}");
            AppendLine();
        }

        public void Visit(FsmNode node)
        {
            IndentedAppendLine("using System;");
            AppendLine();
            IndentedAppendLine("namespace Smc");
            IndentedAppendLine("{");
            indentLevel++;
            IndentedAppendLine($"public class {node.ClassName} : {node.ActionsName}");
            IndentedAppendLine("{");

            indentLevel++;

            node.States.Accept(this);
            node.Events.Accept(this);
            node.Delegators.Accept(this);
            node.Initial.Accept(this);
            node.HandleEvent.Accept(this);

            indentLevel--;
            IndentedAppendLine("}");
            indentLevel--;
            IndentedAppendLine("}");
        }

        public void Visit(EventDelegatorsNode node)
        {
            foreach (var ev in node.Events)
            {
                IndentedAppendLine($"public void {ev}() {{ Handle(Event.{ev}); }}");
                AppendLine();
            }
        }

        public void Visit(SwitchCaseNode node)
        {
            IndentedAppendLine($"switch ({node.VariableName})");
            IndentedAppendLine("{");

            indentLevel++;
            foreach (var cn in node.CaseNodes)
            {
                cn.Accept(this);
            }
            indentLevel--;

            IndentedAppendLine("}");
        }

        public void Visit(HandleEventNode node)
        {
            IndentedAppendLine("public void Handle(Event ev)");
            IndentedAppendLine("{");
            indentLevel++;
            node.SwitchCaseNode.Accept(this);
            indentLevel--;
            IndentedAppendLine("}");
        }

        public void Visit(CaseNode node)
        {
            IndentedAppendLine($"case {node.SwitchName}.{node.CaseName}:");
            indentLevel++;
            node.CaseActionNode.Accept(this);
            IndentedAppendLine("break;");
            indentLevel--;
        }

        public void Visit(EnumeratorNode node)
        {
            Append($"{node.Enumeration}.{node.Enumerator}");
        }

        public void Visit(FunctionCallNode node)
        {
            IndentedAppend($"{node.Name}(");
            node.Node?.Accept(this);
            AppendLine(");");
        }

        private void AppendLine(string s = "")
        {
            stringBuilder.AppendLine(s);
        }

        private void Append(string s)
        {
            stringBuilder.Append(s);
        }

        public void Visit(StatePropertyNode node)
        {
            IndentedAppendLine($"public StateEnum State {{ get; private set; }} = StateEnum.{node.InitialState};");
            AppendLine();
        }

        public void Visit(DefaultCaseNode node)
        {
        }

        public void Visit(SetterNode node)
        {
            IndentedAppend($"{node.Name} = ");
            node.Node.Accept(this);
            AppendLine(";");
        }

        private void IndentedAppend(string str)
        {
            stringBuilder.Append($"{Indent}{str}");
        }

        private void IndentedAppendLine(string str = "")
        {
            stringBuilder.AppendLine($"{Indent}{str}");
        }
    }
}