using System.Collections.Generic;
using System.Linq;

namespace Smc.Syntax
{
    public class FsmSyntax : ISyntax
    {
        public FsmSyntax(Headers headers, Logic logic)
        {
            Headers = headers;
            Logic = logic;
        }

        public Headers Headers { get; }
        public Logic Logic { get; }

        public void Accept(ISyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            var headers = FormatHeaders();
            var logic = FormatLogic();

            return string.Join("\n", headers, logic);
        }

        private string FormatLogic()
        {
            var transitions = string.Join("\n\n", Logic.Select(FormatTransition));
            return $"{{\n{transitions}\n}}";
        }

        private string FormatTransition(Transition transition)
        {
            return "  " + FormatState(transition.StateSpec) + " " + FormatSubtransitions(transition.Subtransitions);
        }

        private string FormatSubtransitions(IList<Subtransition> subtransitions)
        {
            if (subtransitions.Count == 1) return FormatSubTransition(subtransitions.First());

            var innerText = string.Join("\n",
                subtransitions.Select(subtransition => "    " + FormatSubTransition(subtransition)));
            return $"{{\n{innerText}\n  }}";
        }

        private string FormatSubTransition(Subtransition arg)
        {
            var ev = FormatOptional(arg.Ev);
            var nx = FormatOptional(arg.NextState);
            var ac = FormatEvents(arg.Actions);

            return $"{ev} {nx} {ac}";
        }

        private string FormatEvents(ICollection<string> actions)
        {
            if (actions.Count == 1) return FormatOptional(actions.First());

            var inner = string.Join("\n", actions.Select(x => $"    {x}"));
            return $"\n{{{inner}\n";
        }

        private string FormatOptional(string name)
        {
            return name ?? "-";
        }

        private string FormatState(StateSpec state)
        {
            var name = state.IsSuperState ? $"({state.Name})" : state.Name;

            var super = state.Modifiers.Where(x => x.Kind == ModifierKind.SuperState).Select(x => ":" + x.Name);
            var entry = state.Modifiers.Where(x => x.Kind == ModifierKind.EntryAction).Select(x => "<" + x.Name);
            var exit = state.Modifiers.Where(x => x.Kind == ModifierKind.ExitAction).Select(x => ">" + x.Name);

            var parts = new[] {name}.Concat(super).Concat(entry).Concat(exit);

            return string.Join(" ", parts);
        }

        private string FormatHeader(Header header)
        {
            return $"{header.Name}:{header.Value}";
        }

        private string FormatHeaders()
        {
            return string.Join("\n", Headers.Select(FormatHeader));
        }
    }
}