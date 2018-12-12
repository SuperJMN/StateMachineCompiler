using Smc.Syntax;
using Superpower;
using Xunit;

namespace Smc.Tests
{
    public class IndividualParserSpecs
    {
        private static void AssertParseResult<T>(string input, string expectation,
            TokenListParser<SmcToken, T> transition) where T : ISyntax
        {
            var syntax = transition.Parse(Tokenizer.Create().Tokenize(input));
            var formatter = new SyntaxFormatter();
            syntax.Accept(formatter);
            var formatted = formatter.Text;
            Assert.Equal(expectation, formatted);
        }

        [Fact]
        public void AllStateAdornments()
        {
            var input = @"(s)<ea>xa:ss e ns a";
            var expectation = "  (s):ss <ea >xa e ns a\r\n";
            AssertParseResult(input, expectation, Parsers.Transition);
        }

        [Fact]
        public void DerivedState()
        {
            var input = @"s:ss e ns a";
            var expectation = "  s:ss e ns a\r\n";

            AssertParseResult(input, expectation, Parsers.Transition);
        }

        [Fact]
        public void EntryAction()
        {
            var input = @"s <ea e ns a";
            var expectation = "  s <ea e ns a\r\n";

            AssertParseResult(input, expectation, Parsers.Transition);
        }

        [Fact]
        public void ExitAction()
        {
            var input = @"s >xa e ns a";
            var expectation = "  s >xa e ns a\r\n";

            AssertParseResult(input, expectation, Parsers.Transition);
        }

        [Fact]
        public void ManyTransitions()
        {
            var input = @"{ s1 e1 s2 a1 s2 e2 s3 a2 }";
            var expectation = "{\r\n" +
                              "  s1 e1 s2 a1\r\n" +
                              "  s2 e2 s3 a2\r\n" +
                              "}\r\n";

            AssertParseResult(input, expectation, Parsers.Logic);
        }

        [Fact]
        public void MultipleEntryActions()
        {
            var input = @"s <x <y - - -";
            var expectation = "  s <x <y - - {}\r\n";

            AssertParseResult(input, expectation, Parsers.Transition);
        }

        [Fact]
        public void MultipleEntryAndExitActionsWithBraces()
        {
            var input = @"s <{u v} >{w x} - - -";
            var expectation = "  s <u <v >w >x - - {}\r\n";

            AssertParseResult(input, expectation, Parsers.Transition);
        }

        [Fact]
        public void MultipleExitActions()
        {
            var input = @"s >x >y - - -";
            var expectation = "  s >x >y - - {}\r\n";

            AssertParseResult(input, expectation, Parsers.Transition);
        }

        [Fact]
        public void MultipleSuperStates()
        {
            AssertParseResult("s :x :y - - -", "  s:x:y - - {}\r\n", Parsers.Transition);
        }

        [Fact]
        public void SimpleTransition()
        {
            var input = @"s e ns a";
            var expectation = "  s e ns a\r\n";

            AssertParseResult(input, expectation, Parsers.Transition);
        }

        [Fact]
        public void StateWithAllDashes()
        {
            AssertParseResult("s - - -", "  s - - {}\r\n", Parsers.Transition);
        }

        [Fact]
        public void StateWithNoSubTransitions()
        {
            var input = @"s {}";
            var expectation = "  s \r\n  {\r\n  }\r\n";
            AssertParseResult(input, expectation, Parsers.Transition);
        }


        [Fact]
        public void StateWithSeveralSubtransitions()
        {
            var input = @"s { e1 ns a1 e2 ns a2 }";
            var expectation = "  s \r\n  {\r\n    e1 ns a1\r\n    e2 ns a2\r\n  }\r\n";
            AssertParseResult(input, expectation, Parsers.Transition);
        }

        [Fact]
        public void StateWithSubtransition()
        {
            var input = @"s { ev ne a}";
            var expectation = "  s ev ne a\r\n";
            AssertParseResult(input, expectation, Parsers.Transition);
        }

        [Fact]
        public void SuperState()
        {
            AssertParseResult("{(ss) e s a}", "{\r\n" +
                                              "  (ss) e s a\r\n" +
                                              "}\r\n", Parsers.Logic);
        }

        [Fact]
        public void TransitionWithManyActions()
        {
            var input = @"s e ns {a1 a2}";
            var expectation = "  s e ns {a1 a2}\r\n";

            AssertParseResult(input, expectation, Parsers.Transition);
        }

        [Fact]
        public void TransitionWithNullAction()
        {
            var input = @"s e ns -";
            var expectation = "  s e ns {}\r\n";

            AssertParseResult(input, expectation, Parsers.Transition);
        }
    }
}