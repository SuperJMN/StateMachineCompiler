using System.Linq;

namespace Compiler
{
    public class Subtransition
    {
        public Event Ev { get; }
        public NextState NextNextState { get; }
        public MyAction[] Actions { get; }

        public Subtransition(Event ev, NextState nextNextState, MyAction[] actions)
        {
            Ev = ev;
            NextNextState = nextNextState;
            Actions = actions;
        }

        public override string ToString()
        {
            var actions = string.Join(",", Actions.Select(x => x.ToString()));
            return $"On {Ev} go to state {NextNextState} and perform {actions}";
        }
    }
}