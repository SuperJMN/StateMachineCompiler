using Superpower;

namespace Compiler
{
    public class SmcParser
    {
        private readonly Tokenizer<SmcToken> smcTokenizer;

        public SmcParser(Tokenizer<SmcToken> smcTokenizer)
        {
            this.smcTokenizer = smcTokenizer;
        }

        public Fsm Parse(string input)
        {
            return Parsers.Fsm.Parse(smcTokenizer.Tokenize(input));
        }
    }
}