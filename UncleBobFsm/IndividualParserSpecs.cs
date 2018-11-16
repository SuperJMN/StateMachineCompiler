using Compiler;
using Superpower;
using Superpower.Model;
using Xunit;

namespace UncleBobFsm
{
    public class IndividualParserSpecs
    {
        [Fact]
        public void Transition()
        {
            var input = @"
                            Locked : Base 
                            {
		                        Pass	Alarming	-
		                        Coin	FirstCoin	-
	                        }";

            var parsed = Parsers.Transition.Parse(Tokenize(input));
        }

        [Fact]
        public void Subtransitions()
        {
            var input = @"{
		                        Pass	Alarming	-
		                        Coin	FirstCoin	-
	                        }";

            var parsed = Parsers.Subtransitions.Parse(Tokenize(input));
        }

        [Fact]
        public void Subtransitions2()
        {
            var input = @"{ Pass Alarming - }";

            var parsed = Parsers.Subtransitions.Parse(Tokenize(input));
        }

        [Fact]
        public void StateSpec()
        {
            var input = @"Pass: Base";

            var parsed = Parsers.StateSpec.Parse(Tokenize(input));
        }

        [Fact]
        public void Subtransition()
        {
            var input = @"Pass Alarming -";

            var parsed = Parsers.Subtransition.Parse(Tokenize(input));
        }

        private static TokenList<SmcToken> Tokenize(string input)
        {
            return SmcTokenizer.Create().Tokenize(input);
        }
    }
}