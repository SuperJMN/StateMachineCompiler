using System;
using Compiler;
using Xunit;

namespace UncleBobFsm
{
    public class SfmParserSpecs
    {
        [Fact]
        public void Test1()
        {
            var smcTokenizer = SmcTokenizer.Create();
            var sut = new SmcParser(smcTokenizer);

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
	                        
	                        Alarming : Base	<alarmOn >alarmOff -	-	-
	                        
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
        }
    }
}
