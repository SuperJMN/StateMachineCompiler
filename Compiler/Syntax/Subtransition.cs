namespace Smc.Syntax
{
    public class Subtransition : ISyntax
    {
        public string Event { get; }
        public string NextState { get; }
        public string[] Actions { get; }

        public Subtransition(string ev, string nextState, string[] actions)
        {
            Event = ev;
            NextState = nextState;
            Actions = actions;
        }

        public void Accept(ISyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}