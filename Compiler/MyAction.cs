namespace Compiler
{
    public class MyAction
    {
        public string Name { get; }

        public MyAction(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}