namespace Smc.Syntax
{
    public class StateModifier
    {
        public ModifierKind Kind { get; }
        public string Name { get; }

        public StateModifier(ModifierKind kind, string name)
        {
            Kind = kind;
            Name = name;
        }
    }
}