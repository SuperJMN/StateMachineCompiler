namespace Compiler
{
    public class StateSpec
    {
        public State State { get; }
        public string[] Modifiers { get; }

        public StateSpec(State state, string[] modifiers)
        {
            State = state;
            Modifiers = modifiers;
        }
    }
}