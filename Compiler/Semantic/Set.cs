using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Smc.Semantic
{
    internal class Set<T> where T : class

    {
        private readonly HashSet<T> set;

        public Set(IEnumerable<T> o)
        {
            set = new HashSet<T>(o);
        }

        protected bool Equals(Set<T> values)
        {
            return set.SetEquals(values.set);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Set<T>)obj);
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}