namespace Smc.Syntax
{
    public interface ISyntax
    {
        void Accept(ISyntaxVisitor visitor);
    }
}