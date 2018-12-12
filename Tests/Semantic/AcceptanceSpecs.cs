using Xunit;

namespace Smc.Tests.SemanticAnalyzer
{
    public class AcceptanceSpecs : SpecsBase
    {
        [Fact]
        public void SubwayTurnstileOne()
        {
            AssertAst("" +
                      "Actions: Turnstile\n" +
                      "FSM: OneCoinTurnstile\n" +
                      "Initial: Locked\n" +
                      "{\n" +
                      "  Locked\tCoin\tUnlocked\t{alarmOff unlock}\n" +
                      "  Locked \tPass\tLocked\t\talarmOn\n" +
                      "  Unlocked\tCoin\tUnlocked\tthankyou\n" +
                      "  Unlocked\tPass\tLocked\t\tlock\n" +
                      "}",
                "" +
                "Actions: Turnstile\n" +
                "FSM: OneCoinTurnstile\n" +
                "Initial: Locked\n" +
                "  {\n" +
                "    Locked {\n" +
                "    Coin Unlocked {alarmOff unlock}\n" +
                "    Pass Locked {alarmOn}\n" +
                "  }\n" +
                "\n" +
                "  Unlocked {\n" +
                "    Coin Unlocked {thankyou}\n" +
                "    Pass Locked {lock}\n" +
                "  }\n" +
                "}\n");
        }

        [Fact]
        public void SubwayTurnstileThree()
        {
            AssertAst("" +
                      "Actions: Turnstile\n" +
                      "FSM: TwoCoinTurnstile\n" +
                      "Initial: Locked\n" +
                      "{\n" +
                      "    (Base)\tReset\tLocked\tlock\n" +
                      "\n" +
                      "\tLocked : Base {\n" +
                      "\t\tPass\tAlarming\t-\n" +
                      "\t\tCoin\tFirstCoin\t-\n" +
                      "\t}\n" +
                      "\t\n" +
                      "\tAlarming : Base\t<alarmOn >alarmOff -\t-\t-\n" +
                      "\t\n" +
                      "\tFirstCoin : Base {\n" +
                      "\t\tPass\tAlarming\t-\n" +
                      "\t\tCoin\tUnlocked\tunlock\n" +
                      "\t}\n" +
                      "\t\n" +
                      "\tUnlocked : Base {\n" +
                      "\t\tPass\tLocked\tlock\n" +
                      "\t\tCoin\t-\t\tthankyou\n" +
                      "\t}\n" +
                      "}",
                "" +
                "Actions: Turnstile\n" +
                "FSM: TwoCoinTurnstile\n" +
                "Initial: Locked\n" +
                "{\n" +
                "  Alarming :Base <alarmOn >alarmOff {\n" +
                "    null Alarming {}\n" +
                "  }\n" +
                "\n" +
                "  (Base) {\n" +
                "    Reset Locked {lock}\n" +
                "  }\n" +
                "\n" +
                "  FirstCoin :Base {\n" +
                "    Pass Alarming {}\n" +
                "    Coin Unlocked {unlock}\n" +
                "  }\n" +
                "\n" +
                "  Locked :Base {\n" +
                "    Pass Alarming {}\n" +
                "    Coin FirstCoin {}\n" +
                "  }\n" +
                "\n" +
                "  Unlocked :Base {\n" +
                "    Pass Locked {lock}\n" +
                "    Coin Unlocked {thankyou}\n" +
                "  }\n" +
                "}\n");
        }

        [Fact]
        public void SubwayTurnstileTwo()
        {
            AssertAst("" +
                      "Actions: Turnstile\n" +
                      "FSM: TwoCoinTurnstile\n" +
                      "Initial: Locked\n" +
                      "{\n" +
                      "\tLocked\n" +
                      "{\n" +
                      "\t\tPass\tAlarming\talarmOn\n" +
                      "\t\tCoin\tFirstCoin\t-\n" +
                      "\t\tReset\tLocked\t{lock alarmOff}\n" +
                      "\t}\n" +
                      "\t\n" +
                      "\tAlarming\tReset\tLocked {lock alarmOff}\n" +
                      "\t\n" +
                      "\tFirstCoin {\n" +
                      "\t\tPass\tAlarming\t-\n" +
                      "\t\tCoin\tUnlocked\tunlock\n" +
                      "\t\tReset\tLocked {lock alarmOff}\n" +
                      "\t}\n" +
                      "\t\n" +
                      "\tUnlocked {\n" +
                      "\t\tPass\tLocked\tlock\n" +
                      "\t\tCoin\t-\t\tthankyou\n" +
                      "\t\tReset\tLocked {lock alarmOff}\n" +
                      "\t}\n" +
                      "}",
                "" +
                "Actions: Turnstile\n" +
                "FSM: TwoCoinTurnstile\n" +
                "Initial: Locked\n" +
                "  {\n" +
                "  Alarming {\n" +
                "    Reset Locked {lock alarmOff}\n" +
                "  }\n" +
                "\n" +
                "  FirstCoin {\n" +
                "    Pass Alarming {}\n" +
                "    Coin Unlocked {unlock}\n" +
                "    Reset Locked {lock alarmOff}\n" +
                "  }\n" +
                "\n" +
                "  Locked {\n" +
                "    Pass Alarming {alarmOn}\n" +
                "    Coin FirstCoin {}\n" +
                "    Reset Locked {lock alarmOff}\n" +
                "  }\n" +
                "\n" +
                "  Unlocked {\n" +
                "    Pass Locked {lock}\n" +
                "    Coin Unlocked {thankyou}\n" +
                "    Reset Locked {lock alarmOff}\n" +
                "  }\n" +
                "}\n");
        }
    }
}