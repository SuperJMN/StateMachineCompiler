using FluentAssertions;
using Smc.Semantic;
using Xunit;

namespace Smc.Tests.SemanticAnalyzer.SemanticErrors
{
    public class StateErrors : SpecsBase
    {
        [Fact]
        public void ErrorIfSuperStatesHaveConflictingTransitions()
        {
            ProduceAst("FSM: f Actions: act Initial: s" +
                       "{" +
                       "  (ss1) e1 s1 -" +
                       "  (ss2) e1 s2 -" +
                       "  s :ss1 :ss2 e2 s3 a" +
                       "  s2 e s -" +
                       "  s1 e s -" +
                       "  s3 e s -" +
                       "}").Errors
                .Should()
                .Contain(new AnalysisError(ErrorKind.ConflictingSuperstates, "s|e1"));
        }

        [Fact]
        public void ErrorIfSuperstatesHaveDifferentActionsInSameTransitions()
        {
            ProduceAst("FSM: f Actions: act Initial: s" +
                       "{" +
                       "  (ss1) e1 s1 a1" +
                       "  (ss2) e1 s1 a2" +
                       "  s :ss1 :ss2 e2 s3 a" +
                       "  s1 e s -" +
                       "  s3 e s -" +
                       "}").Errors
                .Should()
                .Contain(new AnalysisError(ErrorKind.ConflictingSuperstates, "s|e1"));
        }

        [Fact]
        public void NextStateNullIsImplicitUse()
        {
            ProduceAst("{s e - -}").Errors
                .Should()
                .NotContain(new AnalysisError(ErrorKind.UnusedState, "s"));
        }

        [Fact]
        public void NoErrorForOverriddenTransition()
        {
            ProduceAst("FSM: f Actions: act Initial: s" +
                       "{" +
                       "  (ss1) e1 s1 -" +
                       "  s :ss1 e1 s3 a" +
                       "  s1 e s -" +
                       "  s3 e s -" +
                       "}").Errors
                .Should()
                .NotContain(new AnalysisError(ErrorKind.ConflictingSuperstates, "s|e1"));
        }

        [Fact]
        public void NoErrorIfSuperStatesHaveIdenticalTransitions()
        {
            ProduceAst("FSM: f Actions: act Initial: s" +
                       "{" +
                       "  (ss1) e1 s1 ax" +
                       "  (ss2) e1 s1 ax" +
                       "  s :ss1 :ss2 e2 s3 a" +
                       "  s1 e s -" +
                       "  s3 e s -" +
                       "}").Errors
                .Should()
                .NotContain(new AnalysisError(ErrorKind.ConflictingSuperstates, "s|e1"));
        }

        [Fact]
        public void NoUndefinedStates()
        {
            ProduceAst("{s - s -}").Errors
                .Should()
                .NotContain(new AnalysisError(ErrorKind.UndefinedState, "s2"));
        }

        [Fact]
        public void NoUnusedStates()
        {
            ProduceAst("{s e s -}").Errors
                .Should()
                .NotContain(new AnalysisError(ErrorKind.UnusedState, "s"));
        }

        [Fact]
        public void NullNextStateIsNotUndefined()
        {
            ProduceAst("{s - - -}").Errors
                .Should()
                .NotContain(new AnalysisError(ErrorKind.UndefinedState));
        }

        [Fact]
        public void SuperStateDefined()
        {
            ProduceAst("{s:ss - - - ss - - -}").Errors
                .Should()
                .NotContain(new AnalysisError(ErrorKind.UndefinedSuperstate, "ss"));
        }

        [Fact]
        public void UndefinedState()
        {
            ProduceAst("{s - s2 -}").Errors
                .Should()
                .Contain(new AnalysisError(ErrorKind.UndefinedState, "s2"));
        }

        [Fact]
        public void UndefinedSuperState()
        {
            ProduceAst("{s:ss - - -}").Errors
                .Should()
                .Contain(new AnalysisError(ErrorKind.UndefinedSuperstate, "ss"));
        }

        [Fact]
        public void UnusedStates()
        {
            ProduceAst("{s e n -}").Errors
                .Should()
                .Contain(new AnalysisError(ErrorKind.UnusedState, "s"));
        }

        [Fact]
        public void UsedAsBaseIsValidUsage()
        {
            ProduceAst("{b e n - s:b e2 s -}").Errors
                .Should()
                .NotContain(new AnalysisError(ErrorKind.UnusedState, "b"));
        }

        [Fact]
        public void UsedAsInitialIsValidUsage()
        {
            ProduceAst("initial: b {b e n -}").Errors
                .Should()
                .NotContain(new AnalysisError(ErrorKind.UnusedState, "b"));
        }
    }
}