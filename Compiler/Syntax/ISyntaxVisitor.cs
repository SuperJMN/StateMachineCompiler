namespace Smc.Syntax
{
    public interface ISyntaxVisitor
    {
        void Visit(FsmSyntax fsmSyntax);
        void Visit(Header header);
        void Visit(Transition transition);
        void Visit(Subtransition subtransition);
        void Visit(StateSpec stateSpec);
        void Visit(Headers headers);
        void Visit(Logic logic);
        void Visit(Subtransitions subtransition);
    }
}