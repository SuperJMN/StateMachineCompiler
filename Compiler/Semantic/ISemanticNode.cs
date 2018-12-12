using System.Collections;

namespace Smc.Semantic
{
    public interface ISemanticNode
    {
        void Accept(ISemanticVisitor visitor);         
    }
}