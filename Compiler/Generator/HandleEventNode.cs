namespace Smc.Generator
{
    public class HandleEventNode : INscNode
    {
        public SwitchCaseNode SwitchCaseNode { get; }

        public HandleEventNode(SwitchCaseNode switchCaseNode)
        {
            SwitchCaseNode = switchCaseNode;
        }

        public void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}