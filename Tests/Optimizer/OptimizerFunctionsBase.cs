using FluentAssertions;
using Smc.Optimize;
using Smc.Semantic;
using Smc.Syntax;

namespace Smc.Tests.Optimizer
{
    public class OptimizerFunctionsBase
    {
        protected OptimizedStateMachine ProduceStateMachineWithHeader(string str)
        {
            return ProduceStateMachine("fsm:f initial:i actions:a " + str);
        }

        private OptimizedStateMachine ProduceStateMachine(string fsmSyntax)
        {
            var asmSyntax = new Parser(Tokenizer.Create()).Parse(fsmSyntax);
            var ast = new Semantic.SemanticAnalyzer().Analize(asmSyntax);
            var optimizer = new Optimize.Optimizer();
            return optimizer.Optimize(ast);
        }

        private static string CompressWhiteSpace(string s)
        {
            return s.ReplaceAll("\\n+", "\n").ReplaceAll("[\t ]+", " ").ReplaceAll(" *\n *", "\n");
        }

        protected void AssertOptimization(string syntax, string stateMachine)
        {
            var optimizedStateMachine = ProduceStateMachineWithHeader(syntax);
            CompressWhiteSpace(optimizedStateMachine.GetTransitionsString())
                .Should()
                .Be(CompressWhiteSpace(stateMachine));
        }

        protected void AssertOptimizationFullSyntax(string syntax, string stateMachine)
        {
            var optimizedStateMachine = ProduceStateMachine(syntax);
            CompressWhiteSpace(optimizedStateMachine.ToString())
                .Should()
                .Be(CompressWhiteSpace(stateMachine));
        }
    }
}