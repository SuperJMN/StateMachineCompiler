using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smc.Syntax
{
    public class SyntaxFormatter : ISyntaxVisitor
    {
        private const int IndentSize = 2;
        private readonly StringBuilder builder = new StringBuilder();

        public string Text => builder.ToString();

        public void Visit(FsmSyntax fsmSyntax)
        {
            fsmSyntax.Headers.Accept(this);
            builder.AppendLine("{");
            fsmSyntax.Logic.Accept(this);
            builder.AppendLine("}");
        }

        public void Visit(Header header)
        {
            builder.Append($"{header.Name}:{header.Value}");
        }

        public void Visit(Transition transition)
        {
            builder.Append(Indent(1));
            transition.StateSpec.Accept(this);
            builder.Append(" ");
            transition.Subtransitions.Accept(this);
            builder.AppendLine();
        }

        public void Visit(Subtransition subtransition)
        {
            var ev = FormatOptional(subtransition.Ev);
            var nx = FormatOptional(subtransition.NextState);
            var ac = FormatEvents(subtransition.Actions);

            builder.Append($"{ev} {nx} {ac}");
        }

        public void Visit(StateSpec state)
        {
            var name = state.IsSuperState ? $"({state.Name})" : state.Name;

            var super = state.Modifiers.Where(x => x.Kind == ModifierKind.SuperState).Select(x => ":" + x.Name);
            var entry = state.Modifiers.Where(x => x.Kind == ModifierKind.EntryAction).Select(x => "<" + x.Name);
            var exit = state.Modifiers.Where(x => x.Kind == ModifierKind.ExitAction).Select(x => ">" + x.Name);

            var parts = new[] {name}.Concat(super).Concat(entry).Concat(exit);

            builder.Append(string.Join(" ", parts));            
        }

        public void Visit(Headers headers)
        {
            foreach (var header in headers)
            {
                header.Accept(this);                
                builder.AppendLine();
            }
        }

        public void Visit(Logic logic)
        {
            foreach (var transition in logic)
            {
                transition.Accept(this);
                builder.AppendLine();
            }
        }

        public void Visit(Subtransitions subtransitions)
        {
            if (subtransitions.Count == 1)
            {
                subtransitions.First().Accept(this);
            }
            else
            {
                builder.AppendLine(" {");

                foreach (var subtransition in subtransitions)
                {
                    builder.Append(Indent(2));
                    subtransition.Accept(this);
                    builder.AppendLine();
                }

                builder.Append($"{Indent(1)}}}");
            }
        }

        private string FormatEvents(ICollection<string> actions)
        {
            if (actions.Count == 1) return FormatOptional(actions.First());

            var inner = string.Join("\n", actions.Select(x => $"{Indent(2)}{x}"));
            return $"\n{{{inner}\n";
        }

        private static string Indent(int i)
        {
            var r = "";
            for (var j = 0; j < i * IndentSize; j++) r += " ";

            return r;
        }

        private string FormatOptional(string name)
        {
            return name ?? "-";
        }
    }
}