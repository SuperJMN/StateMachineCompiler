using FluentAssertions;
using Smc.Semantic;
using Smc.Syntax;

namespace Smc.Tests
{
    public class SpecsBase
    {
        protected static SemanticStateMachine ProduceAst(string input)
        {
            var analizer = new Semantic.SemanticAnalyzer();
            var fsmSyntax = new Parser(Tokenizer.Create()).Parse(input);
            var ast = analizer.Analize(fsmSyntax);
            return ast;
        }

        protected void AssertSyntaxToAstWithDefaultHeaders(string syntax, string ast)
        {
            var semantic = ProduceAst("initial: s fsm:f actions:a " + syntax);
            var formatter = new SemanticFormatter();
            semantic.States.Accept(formatter);
            var result = formatter.GetResult();

            CompressWhiteSpace(result.Replace("\r\n", "\n")).Should().Be(CompressWhiteSpace(ast));
        }

        protected void AssertAst(string syntax, string ast)
        {
            var semantic = ProduceAst(syntax);
            var formatter = new SemanticFormatter();
            semantic.Accept(formatter);
            var result = formatter.GetResult();

            CompressWhiteSpace(result.Replace("\r\n", "\n")).Should().Be(CompressWhiteSpace(ast));
        }

        protected void AssertStates(string syntax, string ast)
        {
            var semantic = ProduceAst(syntax);
            var formatter = new SemanticFormatter();
            semantic.States.Accept(formatter);
            var result = formatter.GetResult();

            CompressWhiteSpace(result.Replace("\r\n", "\n")).Should().Be(CompressWhiteSpace(ast));
        }

        protected static string CompressWhiteSpace(string s)
        {
            return s.ReplaceAll("\\n+", "\n").ReplaceAll("[\t ]+", " ").ReplaceAll(" *\n *", "\n");
        }
    }
}