using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Smc.Syntax
{
    public class Subtransitions : Collection<Subtransition>, ISyntax
    {
        public Subtransitions(List<Subtransition> list) : base(list)
        {
        }

        public void Accept(ISyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}