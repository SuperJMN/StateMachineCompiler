using Superpower;

namespace Smc.Syntax
{
    public class Parser
    {
        private readonly Tokenizer<SmcToken> smcTokenizer;

        public Parser(Tokenizer<SmcToken> smcTokenizer)
        {
            this.smcTokenizer = smcTokenizer;
        }

        public FsmSyntax Parse(string input)
        {
            return Parsers.Fsm.Parse(smcTokenizer.Tokenize(input));
        }
    }
}