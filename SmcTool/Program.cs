using System.IO;
using CommandLine;
using Smc.Generator;
using Smc.Optimize;
using Smc.Semantic;
using Smc.Syntax;
using Smc.Tests.Implementers;
using CommandlineParserParser = CommandLine.Parser;
using Parser = Smc.Syntax.Parser;

namespace SmcTool
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandlineParserParser.Default.ParseArguments<Options>(args)
                .WithParsed(o => { Compile(o.InputFile, o.OutputFile); });
        }

        private static void Compile(string inputFile, string outputFile)
        {
            var input = File.ReadAllText(inputFile);
            var syntax = new Parser(Tokenizer.Create()).Parse(input);
            var semantic = new SemanticAnalyzer().Analize(syntax);
            var machine = new Optimizer().Optimize(semantic);
            var node = new NscGenerator().Generate(machine);

            var visitor = new CsharpVisitor();
            node.Accept(visitor);
            
            File.WriteAllText(outputFile, visitor.Result);
        }
    }

    internal class Options
    {
        [Option('i', "InputFile")]
        public string InputFile { get; set; }

        [Option('o', "OutputFile")]
        public string OutputFile { get; set; }
    }
}
