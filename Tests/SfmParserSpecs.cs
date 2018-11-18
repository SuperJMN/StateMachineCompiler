using Smc.Syntax;
using Xunit;

namespace Smc.Tests
{
    public class SfmParserSpecs
    {
        [Fact]
        public void Test1()
        {
            var smcTokenizer = Tokenizer.Create();
            var sut = new Parser(smcTokenizer);

            var input = @"
                        Actions: Turnstile
                        FSM: TwoCoinTurnstile
                        Initial: Locked
                        {
                            (Base)	Reset	Locked	lock

	                        Locked : Base {
		                        Pass	Alarming	-
		                        Coin	FirstCoin	-
	                        }
	                        
	                        Alarming : Base	<alarmOn >alarmOff { 
                                -       -           -   
                            }
	                        
	                        FirstCoin : Base {
		                        Pass	Alarming	-
		                        Coin	Unlocked	unlock
	                        }
	                        
	                        Unlocked : Base {
		                        Pass	Locked	lock
		                        Coin	-		thankyou
	                        }
                        }";

            var tree = sut.Parse(input);
            var formattingVisitor = new SyntaxFormatter();
            tree.Accept(formattingVisitor);

            var formatted = formattingVisitor.Text;

            Assert.Equal(input, formatted);
        }
    }
}
