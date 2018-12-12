using System.Collections.Generic;

namespace Smc.Generator
{
    public class EventDelegatorsNode : INscNode

    {
        public List<string> Events { get; }

        public EventDelegatorsNode(List<string> events)
        {
            Events = events;
        }

        public void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}