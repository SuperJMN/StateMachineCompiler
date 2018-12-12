using FluentAssertions;
using Smc.Semantic;
using Smc.Syntax;
using Xunit;

namespace Smc.Tests.SemanticAnalyzer.SemanticErrors
{
    public class HeaderErrors : SpecsBase
    {
        [Fact]
        public void DuplicateHeader()
        {
            var errors = ProduceAst("fsm:f fsm:x{s - - -}").Errors;

            errors
                .Should()
                .Contain(new AnalysisError(ErrorKind.DuplicateHeader, new Header("fsm", "x")));
        }

        [Fact]
        public void InitialStateMustBeDefined()
        {
            ProduceAst("initial: i {s - - -}").Errors
                .Should()
                .Contain(new AnalysisError(ErrorKind.UndefinedState, "initial: i"));
        }

        [Fact]
        public void MissingActions()
        {
            ProduceAst("FSM:f Initial:i {}").Errors
                .Should()
                .NotContain(new[] {new AnalysisError(ErrorKind.NoFsm), new AnalysisError(ErrorKind.NoInitial)});
        }

        [Fact]
        public void MissingFsm()
        {
            var errors = ProduceAst("actions:a Initial:i {}").Errors;

            errors
                .Should()
                .NotContain(new AnalysisError(ErrorKind.NoInitial));

            errors
                .Should()
                .Contain(new AnalysisError(ErrorKind.NoFsm));
        }

        [Fact]
        public void MissingInitial()
        {
            var errors = ProduceAst("Actions:a Fsm:f {}").Errors;

            errors
                .Should()
                .Contain(new AnalysisError(ErrorKind.NoInitial));

            errors
                .Should()
                .NotContain(new AnalysisError(ErrorKind.NoFsm));
        }

        [Fact]
        public void NoHeaders()
        {
            ProduceAst("{}").Errors
                .Should()
                .Contain(new[] {new AnalysisError(ErrorKind.NoFsm), new AnalysisError(ErrorKind.NoInitial)});
        }

        [Fact]
        public void NothingMissing()
        {
            var errors = ProduceAst("Actions:a Fsm:f Initial:i {}").Errors;

            errors
                .Should()
                .NotContain(new[] {new AnalysisError(ErrorKind.NoFsm), new AnalysisError(ErrorKind.NoInitial)});
        }

        [Fact]
        public void UnexpectedHeader()
        {
            var errors = ProduceAst("X:a {s - - -}").Errors;

            errors
                .Should()
                .Contain(new AnalysisError(ErrorKind.UnexpectedHeader, new Header("X", "a")));
        }
    }
}