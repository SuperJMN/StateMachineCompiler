namespace Smc.Syntax
{
    public class StateSpec
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
    }
}