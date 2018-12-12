using Xunit;

namespace Smc.Tests.Optimizer
{
    public class AcceptanceSpecs : OptimizerFunctionsBase
    {
        [Fact]
        public void OverridingTransitions()
        {
            AssertOptimizationFullSyntax("" +
                                         "Actions: Turnstile\n" +
                                         "FSM: TwoCoinTurnstile\n" +
                                         "Initial: Locked\n" +
                                         "{" +
                                         "    (Base)  Reset  Locked  lock" +
                                         "" +
                                         "  Locked : Base {" +
                                         "    Pass  Alarming  -" +
                                         "    Coin  FirstCoin -" +
                                         "  }" +
                                         "" +
                                         "  Alarming : Base <alarmOn >alarmOff -  -  -" +
                                         "" +
                                         "  FirstCoin : Base {" +
                                         "    Pass  Alarming  -" +
                                         "    Coin  Unlocked  unlock" +
                                         "  }" +
                                         "" +
                                         "  Unlocked : Base {" +
                                         "    Pass  Locked  lock" +
                                         "    Coin  -       thankyou" +
                                         "  }\n" +
                                         "}\n",
                "" +
                "Initial: Locked\n" +
                "Fsm: TwoCoinTurnstile\n" +
                "Actions: Turnstile\n" +
                "{\n" +
                "  Alarming {\n" +
                "    Reset Locked {alarmOff lock}\n" +
                "  }\n" +
                "  FirstCoin {\n" +
                "    Pass Alarming {alarmOn}\n" +
                "    Coin Unlocked {unlock}\n" +
                "    Reset Locked {lock}\n" +
                "  }\n" +
                "  Locked {\n" +
                "    Pass Alarming {alarmOn}\n" +
                "    Coin FirstCoin {}\n" +
                "    Reset Locked {lock}\n" +
                "  }\n" +
                "  Unlocked {\n" +
                "    Pass Locked {lock}\n" +
                "    Coin Unlocked {thankyou}\n" +
                "    Reset Locked {lock}\n" +
                "  }\n" +
                "}\n"                
            );
        }
    }
}