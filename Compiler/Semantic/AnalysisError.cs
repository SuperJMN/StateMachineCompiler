using DeepEqual.Syntax;

namespace Smc.Semantic
{
    public class AnalysisError
    {
        public ErrorKind Kind { get; }
        public object Extra { get; }

        public AnalysisError(ErrorKind kind, object extra = null)
        {
            Kind = kind;
            Extra = extra;
        }

        protected bool Equals(AnalysisError other)
        {
            return Kind == other.Kind && Extra.IsDeepEqual(other.Extra);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AnalysisError) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) Kind * 397) ^ (Extra != null ? Extra.GetHashCode() : 0);
            }
        }
    }
}