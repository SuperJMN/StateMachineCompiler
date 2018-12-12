namespace Smc.Generator
{
    public class FsmNode : INscNode
    {
        public void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public EnumNode Events { get; set; }
        public EnumNode States { get; set; }
        public EventDelegatorsNode Delegators { get; set; }
        public HandleEventNode HandleEvent { get; set; }
        public StatePropertyNode Initial { get; set; }
        public string ClassName { get; set; }
        public string ActionsName { get; set; }
    }
}