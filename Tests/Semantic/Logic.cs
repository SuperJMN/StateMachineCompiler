using Xunit;

namespace Smc.Tests.SemanticAnalyzer
{
    public class Logic : SpecsBase
    {
        [Fact]
        public void ActionsRemainInOrder()
        {
            AssertSyntaxToAstWithDefaultHeaders("{s e s {the quick brown fox jumped over the lazy dogs back}}",
                "" +
                "{\n" +
                "  s {\n" +
                "    e s {the quick brown fox jumped over the lazy dogs back}\n" +
                "  }\n" +
                "}\n"
            );
        }

        [Fact]
        public void EntryAndExitActionsRemainInOrder()
        {
            AssertSyntaxToAstWithDefaultHeaders("{s <{d o} <g >{c a} >t e s a}",
                "" +
                "{\n" +
                "  s <d <o <g >c >a >t {\n" +
                "    e s {a}\n" +
                "  }\n" +
                "}\n"
            );
        }

        [Fact]
        public void NullNextStateRefersToSelf()
        {
            AssertSyntaxToAstWithDefaultHeaders("{s e - a}",
                "" +
                "{\n" +
                "  s {\n" +
                "    e s {a}\n" +
                "  }\n" +
                "}\n"
            );
        }

        [Fact]
        public void OneTransition()
        {
            AssertSyntaxToAstWithDefaultHeaders("{s e s a}",
                "{\n" +
                "    s {\n" +
                "       e s {a}\n" +
                "   }\n" +
                "}\n");
        }

        [Fact]
        public void SuperStatesAreAggregated()
        {
            AssertSyntaxToAstWithDefaultHeaders("{s:b1 e1 s a s:b2 e2 s a (b1) e s - (b2) e s -}",
                "" +
                "{\n" +
                "  (b1) {\n" +
                "    e s {}\n" +
                "  }\n" +
                "\n" +
                "  (b2) {\n" +
                "    e s {}\n" +
                "  }\n" +
                "\n" +
                "  s :b1 :b2 {\n" +
                "    e1 s {a}\n" +
                "    e2 s {a}\n" +
                "  }\n" +
                "}\n");
        }

        [Fact]
        public void TwoTransitionsAreAggregated()
        {
            AssertSyntaxToAstWithDefaultHeaders("{s e1 s a s e2 s a}",
                "" +
                "{\n" +
                "  s {\n" +
                "    e1 s {a}\n" +
                "    e2 s {a}\n" +
                "  }\n" +
                "}\n");
        }
    }
}