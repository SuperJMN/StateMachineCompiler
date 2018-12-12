using Smc.Generator;
using Smc.Syntax;
using Xunit;

namespace Smc.Tests.Implementers
{
    public class CSharpSpecs
    {
        [Fact(Skip = "Shitty test. Doesn't test properly")]
        public void Test1()
        {
            var input =
                        @"Actions: Turnstile
                        FSM: TwoCoinTurnstile
                        Initial: Locked
                        {
                          (Base)  Reset  Locked  Lock

                          Locked : Base {
                            Pass  Alarming   -
                            Coin  FirstCoin  -
                          }
                          
                          Alarming : Base  >AlarmOn <AlarmOff {
                            - - -
                          }
                          
                          FirstCoin : Base {
                            Pass  Alarming  -
                            Coin  Unlocked  Unlock
                          }
                          
                          Unlocked : Base {
                            Pass  Locked  Lock
                            Coin  -       ThankYou
                          }
                        }";

            var status = Generate(input);
        }

        public string Generate(string input)
        {
            var syntax = new Parser(Tokenizer.Create()).Parse(input);
            var semantic = new Semantic.SemanticAnalyzer().Analize(syntax);
            var machine = new Optimize.Optimizer().Optimize(semantic);
            var node = new NscGenerator().Generate(machine);
            var visitor = new CsharpVisitor();
            node.Accept(visitor);
            return visitor.Result;
        }
    }
}