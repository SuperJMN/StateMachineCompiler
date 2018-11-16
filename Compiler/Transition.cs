using System;
using System.Linq;

namespace Compiler
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
            var subtransitionsStr = string.Join(Environment.NewLine, Subtransitions.Select(x => x.ToString()));
            return $@"{StateSpec}: {{{subtransitionsStr}}}";
        }
    }
}