namespace Smc.Syntax
{
    public class Subtransition : ISyntax
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

        public void Accept(ISyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}