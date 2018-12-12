using System.Collections.Generic;
using System.Linq;

namespace Smc.Syntax
{
    public class StateSpec : ISyntax
    {
        public string Name { get; }
        public StateModifier[] Modifiers { get; }
        public bool IsAbstract { get; }

        public StateSpec(string name, StateModifier[] modifiers, bool isAbstract)
        {
            Name = name;
            Modifiers = modifiers;
            IsAbstract = isAbstract;
        }

        public void Accept(ISyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }

        public IEnumerable<string> Superstates => Modifiers.Where(x => x.Kind == ModifierKind.SuperState).SelectMany(x => x.Values);
        public IEnumerable<string> EntryActions => Modifiers.Where(x => x.Kind == ModifierKind.EntryAction).SelectMany(x => x.Values);
        public IEnumerable<string> ExitActions => Modifiers.Where(x => x.Kind == ModifierKind.ExitAction).SelectMany(x => x.Values);

        protected bool Equals(StateSpec other)
        {
            return string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((StateSpec) obj);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }
    }
}