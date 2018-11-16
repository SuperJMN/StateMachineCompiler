namespace Compiler
{
    public class NextState
    {
        public string Name { get; }

        public NextState(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}