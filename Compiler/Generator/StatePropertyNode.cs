namespace Smc.Generator
{
    public class StatePropertyNode : INscNode
    {
        public StatePropertyNode(string initialState)
        {
            InitialState = initialState;
        }
        public void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public string InitialState { get; }
    }
}