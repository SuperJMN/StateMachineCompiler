namespace Smc.Generator
{
    public class DefaultCaseNode : INscNode
    {
        public string State { get; }

        public DefaultCaseNode(string state)
        {
            State = state;
        }

        public void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}