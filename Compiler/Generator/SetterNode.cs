namespace Smc.Generator
{
    public class SetterNode : INscNode
    {
        public string Name { get; }
        public INscNode Node { get; }

        public SetterNode(string name, INscNode node)
        {
            Name = name;
            Node = node;
        }

        public void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}