using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Smc.Syntax
{
    public class Headers : Collection<Header>, ISyntax
    {
        public Headers(IList<Header> toList) : base(toList)
        {
        }

        public void Accept(ISyntaxVisitor visitor)
        {
            visitor.Visit(this);            
        }
    }
}