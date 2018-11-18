using System.Linq;

namespace Smc.Syntax
{
    public class Transition : ISyntax
    {
        public StateSpec StateSpec { get; }
        public Subtransitions Subtransitions { get; }

        public Transition(StateSpec stateSpec, Subtransitions subtransitions)
        {
            StateSpec = stateSpec;
            Subtransitions = subtransitions;
        }

        public override string ToString()
        {
            var subtransitionsStr = string.Join(" ", Subtransitions.Select(x => x.ToString()));
            return $@"{StateSpec}: {{{subtransitionsStr}}}";
        }

        public void Accept(ISyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}