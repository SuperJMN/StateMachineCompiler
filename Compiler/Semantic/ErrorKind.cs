namespace Smc.Semantic
{
    public enum ErrorKind
    {
        NoFsm,
        NoInitial,
        UnexpectedHeader,
        DuplicateHeader,
        UndefinedState,
        UndefinedSuperstate,
        UnusedState,
        ConflictingSuperstates,
        DuplicateTransition,
        AbstractStateUsedAsNextState,
        StateActionsMultiplyDefined
    }
}