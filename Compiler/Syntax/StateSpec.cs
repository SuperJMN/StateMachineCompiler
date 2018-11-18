namespace Smc.Syntax
{
    public class StateSpec : ISyntax
    {
        public string Name { get; }
        public StateModifier[] Modifiers { get; }
        public bool IsSuperState { get; }

        public StateSpec(string name, StateModifier[] modifiers, bool isSuperState)
        {
            Name = name;
            Modifiers = modifiers;
            IsSuperState = isSuperState;
        }

        public void Accept(ISyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}