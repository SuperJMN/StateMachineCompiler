using System.Collections.Generic;

namespace Smc.Semantic
{
    public class SemanticStates : SortedDictionary<string, State>, ISemanticNode
    {
        public SemanticStates(IDictionary<string, State> toDictionary) : base(toDictionary)
        {
        }

        public SemanticStates()
        {
        }

        public void Accept(ISemanticVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}