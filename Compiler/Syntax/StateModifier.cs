namespace Smc.Syntax
{
    public class StateModifier
    {
        public ModifierKind Kind { get; }
        public string[] Values { get; }

        public StateModifier(ModifierKind kind, string[] values)
        {
            Kind = kind;
            Values = values;
        }
    }
}