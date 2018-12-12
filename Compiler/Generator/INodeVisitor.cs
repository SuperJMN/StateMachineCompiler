namespace Smc.Generator
{
    public interface INodeVisitor
    {
        void Visit(EnumNode node);
        void Visit(FsmNode node);
        void Visit(EventDelegatorsNode node);
        void Visit(SwitchCaseNode node);
        void Visit(HandleEventNode node);
        void Visit(CaseNode node);
        void Visit(EnumeratorNode node);
        void Visit(FunctionCallNode node);
        void Visit(StatePropertyNode node);
        void Visit(DefaultCaseNode node);
        void Visit(SetterNode node);
    }
}