using FluentAssertions;
using Smc.Semantic;
using Xunit;

namespace Smc.Tests.SemanticAnalyzer.SemanticErrors
{
    public class TransitionErrors : SpecsBase
    {
        [Fact]
        public void AbstractStatesCanBeUsedAsSuperStates()
        {
            ProduceAst("{(as) e - - s:as e s -}").Errors
                .Should()
                .NotContain(new AnalysisError(ErrorKind.AbstractStateUsedAsNextState, "s(e)=>s"));
        }

        [Fact]
        public void AbstractStatesCantBeTargets()
        {
            ProduceAst("{(as) e - - s e as -}").Errors
                .Should()
                .Contain(new AnalysisError(ErrorKind.AbstractStateUsedAsNextState, "s(e)=>as"));
        }

        [Fact]
        public void DuplicateTransitions()
        {
            ProduceAst("{s e - - s e - -}").Errors
                .Should()
                .Contain(new AnalysisError(ErrorKind.DuplicateTransition, "s(e)"));
        }

        [Fact]
        public void EntryAndExitActionsNotMultiplyDefined()
        {
            var unexpectedItemsList = new[]
            {
                new AnalysisError(ErrorKind.StateActionsMultiplyDefined, "s"),
                new AnalysisError(ErrorKind.StateActionsMultiplyDefined, "es"),
                new AnalysisError(ErrorKind.StateActionsMultiplyDefined, "xs")
            };

            ProduceAst("{" +
                       "  s - - -" +
                       "  s - - -" +
                       "  es - - -" +
                       "  es <x - - - " +
                       "  es <x - - -" +
                       "  xs >x - - -" +
                       "  xs >{x} - - -" +
                       "}").Errors
                .Should()
                .NotContain(unexpectedItemsList);
        }

        [Fact]
        public void ErrorIfStateHasMultipleEntryActionDefinitions()
        {
            var analysisErrors = ProduceAst("{s - - - ds <x - - - ds <y - - -}").Errors;

            analysisErrors
                .Should()
                .NotContain(new AnalysisError(ErrorKind.StateActionsMultiplyDefined, "s"));

            analysisErrors
                .Should()
                .Contain(new AnalysisError(ErrorKind.StateActionsMultiplyDefined, "ds"));
        }

        [Fact]
        public void ErrorIfStateHasMultipleExitActionDefinitions()
        {
            ProduceAst("{ds >x - - - ds >y - - -}").Errors
                .Should()
                .Contain(new AnalysisError(ErrorKind.StateActionsMultiplyDefined, "ds"));
        }

        [Fact]
        public void ErrorIfStateHasMultiplyDefinedEntryAndExitActions()
        {
            ProduceAst("{ds >x - - - ds <y - - -}").Errors
                .Should()
                .Contain(new AnalysisError(ErrorKind.StateActionsMultiplyDefined, "ds"));
        }

        [Fact]
        public void NoDuplicateTransitions()
        {
            ProduceAst("{s e - -}").Errors
                .Should()
                .NotContain(new AnalysisError(ErrorKind.DuplicateTransition, "s(e)"));
        }
    }
}