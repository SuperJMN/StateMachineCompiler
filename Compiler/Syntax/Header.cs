namespace Smc.Syntax
{
    public class Header : ISyntax
    {
        public string Name { get; }
        public string Value { get; }

        public Header(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public override string ToString()
        {
            return $"{Name} {Value}";
        }

        public void Accept(ISyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}