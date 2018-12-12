using System.Collections.Generic;

namespace Smc.Generator
{
    public class EnumNode : INscNode
    {
        public string Name { get; }
        public List<string> Values { get; }

        public EnumNode(string name, List<string> values)
        {
            Name = name;
            Values = values;
        }

        public void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}