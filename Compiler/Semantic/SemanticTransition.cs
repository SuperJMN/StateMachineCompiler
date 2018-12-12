using System.Collections.Generic;

namespace Smc.Semantic
{
    public class SemanticTransition : ISemanticNode
    {
        public string Event { get; set; }
        public State NextState { get; set; }
        public List<string> Actions { get; set; } = new List<string>();
        public void Accept(ISemanticVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}