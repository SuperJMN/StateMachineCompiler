namespace Smc.Syntax
{
    public class Subtransition
    {
        public string Ev { get; }
        public string NextState { get; }
        public string[] Actions { get; }

        public Subtransition(string ev, string nextState, string[] actions)
        {
            Ev = ev;
            NextState = nextState;
            Actions = actions;
        }
    }
}