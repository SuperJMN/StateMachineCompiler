namespace Compiler
{
    public class Event
    {
        public string Name { get; }

        public Event(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}