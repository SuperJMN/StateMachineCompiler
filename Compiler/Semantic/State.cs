using System.Collections.Generic;

namespace Smc.Semantic
{
    public class State : ISemanticNode
    {
        public State(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public ICollection<SemanticTransition> Transitions { get; } = new List<SemanticTransition>();
        public HashSet<string> EntryActions { get; } = new HashSet<string>();
        public HashSet<string> ExitActions { get; } = new HashSet<string>();
        public bool IsAbstract { get; set; }
        public HashSet<State> SuperStates { get; } = new HashSet<State>();

        public void Accept(ISemanticVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}