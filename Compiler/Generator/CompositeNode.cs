using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Smc.Generator
{
    internal class CompositeNode : Collection<INscNode>, INscNode
    {
        public CompositeNode(IEnumerable<INscNode> subNodes) : base(new List<INscNode>(subNodes))
        {            
        }

        public void Accept(INodeVisitor visitor)
        {
            foreach (var i in Items)
            {
                i.Accept(visitor);
            }
        }
    }
}