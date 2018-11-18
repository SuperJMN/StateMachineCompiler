using System.Linq;

namespace Smc.Syntax
{
    public class Transition
    {
        public StateSpec StateSpec { get; }
        public Subtransition[] Subtransitions { get; }

        public Transition(StateSpec stateSpec, Subtransition[] subtransitions)
        {
            StateSpec = stateSpec;
            Subtransitions = subtransitions;
        }

        public override string ToString()
        {
            var subtransitionsStr = string.Join(" ", Subtransitions.Select(x => x.ToString()));
            return $@"{StateSpec}: {{{subtransitionsStr}}}";
        }
    }
}