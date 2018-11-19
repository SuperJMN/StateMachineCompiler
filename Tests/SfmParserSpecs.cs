using Smc.Syntax;
using Xunit;

namespace Smc.Tests
{
    public class SfmParserSpecs
    {
        [Fact(Skip = "Not ready yet")]
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

	                        Locked : Base 
                            {
		                        Pass	Alarming	-
		                        Coin	FirstCoin	-
	                        }
	                        
	                        Alarming : Base	<alarmOn >alarmOff 
                            { 
                                -       -           -   
                            }
	                        
	                        FirstCoin : Base 
                            {
		                        Pass	Alarming	-
		                        Coin	Unlocked	unlock
	                        }
	                        
	                        Unlocked : Base 
                            {
		                        Pass	Locked	lock
		                        Coin	-		thankyou
	                        }
                        }";

            var expected = "Actions:Turnstile\r\nFSM:TwoCoinTurnstile\r\nInitial:Locked\r\n{\r\n  (Base) Reset Locked lock\r\n\r\n  Locked :Base \r\n  {\r\n    Pass Alarming -\r\n    Coin FirstCoin -\r\n  }\r\n\r\n  Alarming :Base <alarmOff >alarmOn - - -\r\n\r\n  FirstCoin :Base \r\n  {\r\n    Pass Alarming -\r\n    Coin Unlocked unlock\r\n  }\r\n\r\n  Unlocked :Base \r\n  {\r\n    Pass Locked lock\r\n    Coin - thankyou\r\n  }\r\n\r\n}\r\n";

            var tree = sut.Parse(input);
            var formattingVisitor = new SyntaxFormatter();
            tree.Accept(formattingVisitor);

            var formatted = formattingVisitor.Text;

            Assert.Equal(expected, formatted);
        }
    }
}
