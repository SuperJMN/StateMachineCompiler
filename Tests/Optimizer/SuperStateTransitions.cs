using Xunit;

namespace Smc.Tests.Optimizer
{
    public class SuperStateTransitions : OptimizerFunctionsBase
    {
        [Fact]
        public void DeepInheritanceOfTransitions()
        {
            AssertOptimization("" +
                               "{" +
                               "  (b1) {" +
                               "    b1e1 s b1a1" +
                               "    b1e2 s b1a2" +
                               "  }" +
                               "  (b2):b1 b2e s b2a" +
                               "  i:b2 e s a" +
                               "  s e i -" +
                               "}",
                "" +
                "i {\n" +
                "  e s {a}\n" +
                "  b2e s {b2a}\n" +
                "  b1e1 s {b1a1}\n" +
                "  b1e2 s {b1a2}\n" +
                "}\n" +
                "s {\n" +
                "  e i {}\n" +
                "}\n"
            );
        }

        [Fact]
        public void DiamondInheritanceOfTransitions()
        {
            AssertOptimization("" +
                               "{" +
                               "  (b) be s ba" +
                               "  (b1):b b1e s b1a" +
                               "  (b2):b b2e s b2a" +
                               "  i:b1 :b2 e s a" +
                               "  s e i -" +
                               "}",
                "" +
                "i {\n" +
                "  e s {a}\n" +
                "  b2e s {b2a}\n" +
                "  b1e s {b1a}\n" +
                "  be s {ba}\n" +
                "}\n" +
                "s {\n" +
                "  e i {}\n" +
                "}\n"
            );
        }

        [Fact]
        public void EliminationOfDuplicateTransitions()
        {
            AssertOptimization("" +
                               "{" +
                               "  (b) e s a" +
                               "  i:b e s a" +
                               "  s e i -" +
                               "}",
                "" +
                "i {\n" +
                "  e s {a}\n" +
                "}\n" +
                "s {\n" +
                "  e i {}\n" +
                "}\n"
            );
        }

        [Fact]
        public void MultipleInheritanceOfTransitions()
        {
            AssertOptimization("" +
                               "{" +
                               "  (b1) b1e s b1a" +
                               "  (b2) b2e s b2a" +
                               "  i:b1 :b2 e s a" +
                               "  s e i -" +
                               "}",
                "" +
                "i {\n" +
                "  e s {a}\n" +
                "  b2e s {b2a}\n" +
                "  b1e s {b1a}\n" +
                "}\n" +
                "s {\n" +
                "  e i {}\n" +
                "}\n"
            );
        }

        [Fact]
        public void OverridingTransitions()
        {
            AssertOptimization("" +
                               "{" +
                               "  (b) e s2 a2" +
                               "  i:b e s a" +
                               "  s e i -" +
                               "  s2 e i -" +
                               "}",
                "" +
                "i {\n" +
                "  e s {a}\n" +
                "}\n" +
                "s {\n" +
                "  e i {}\n" +
                "}\n" +
                "s2 {\n" +
                "  e i {}\n" +
                "}\n"
            );
        }

        [Fact]
        public void SimpleInheritanceOfTransitions()
        {
            AssertOptimization("" +
                               "{" +
                               "  (b) be s ba" +
                               "  i:b e s a" +
                               "  s e i -" +
                               "}",
                "" +
                "i {\n" +
                "  e s {a}\n" +
                "  be s {ba}\n" +
                "}\n" +
                "s {\n" +
                "  e i {}\n" +
                "}\n"
            );
        }
    }
}