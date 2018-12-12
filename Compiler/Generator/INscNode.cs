namespace Smc.Generator
{
    public interface INscNode
    {
        void Accept(INodeVisitor visitor);
    }
}