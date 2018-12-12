namespace Smc.Semantic
{
    public interface ISemanticVisitor
    {
        void Visit(SemanticStateMachine semanticStateMachine);
        void Visit(State state);
        void Visit(SemanticTransition transition);
        void Visit(SemanticStates states);
    }
}