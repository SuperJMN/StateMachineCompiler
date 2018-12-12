using System.Collections.Generic;
using System.Linq;

namespace Smc.Optimize
{
    public class OptimizedStateMachine
    {
        public OpimizedHeader Header { get; set; } 
        public List<string> States { get; } = new List<string>();
        public List<string> Events { get; } = new List<string>();
        public List<string> Actions { get; } = new List<string>();

        public List<OptimizedTransition> Transitions { get; } = new List<OptimizedTransition>();

        public override string ToString()
        {
            var transitions = GetTransitionsString();
            return $"Initial: {Header.Initial}\nFsm: {Header.Fsm}\nActions: {Header.Actions}\n{{\n{transitions}}}\n";
        }

        public string GetTransitionsString()
        {
            return string.Join("\n", Transitions.Select(GetTransitionString)) + "\n";
        }

        private string GetTransitionString(OptimizedTransition transition)
        {
            var subTransitionsString = string.Join("\n", transition.Subtransitions.Select(FormatSubtransition));
            return $"{transition.CurrentState} {{\n {subTransitionsString} \n}}";
        }

        private string FormatSubtransition(OptimizedSubtransition arg)
        {
            return $"  {arg.Event} {arg.NextState} {{{ActionsToString(arg.Actions)}}}";
        }

        private string ActionsToString(List<string> actions)
        {
            if (actions.Count == 0)
            {
                return string.Empty;
            }

            return string.Join(" ", actions);
        }

        public class OpimizedHeader
        {
            public string Fsm { get; set; }
            public string Initial { get; set; }
            public string Actions { get; set; }
        }

        public class OptimizedTransition
        {
            public string CurrentState { get; set; }
            public List<OptimizedSubtransition> Subtransitions { get; set; } = new List<OptimizedSubtransition>();
        }

        public class OptimizedSubtransition
        {
            public string Event { get; set; }
            public string NextState { get; set; }
            public List<string> Actions { get; set; } = new List<string>();
        }
    }
}