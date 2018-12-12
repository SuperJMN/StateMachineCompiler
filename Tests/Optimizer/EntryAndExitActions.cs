using Xunit;

namespace Smc.Tests.Optimizer
{
    public class EntryAndExitActions : OptimizerFunctionsBase
    {
        [Fact]
        public void EntryFunctionsAdded()
        {
            AssertOptimization("" +
                               "{" +
                               "  i e s a1" +
                               "  i e2 s a2" +
                               "  s <n1 <n2 e i -" +
                               "}",
                "" +
                "i {\n" +
                "  e s {n1 n2 a1}\n" +
                "  e2 s {n1 n2 a2}\n" +
                "}\n" +
                "s {\n" +
                "  e i {}\n" +
                "}\n");
        }

        [Fact]
        public void ExitFunctionsAdded()
        {
            AssertOptimization("" +
                               "{" +
                               "  i >x2 >x1 e s a1" +
                               "  i e2 s a2" +
                               "  s e i -" +
                               "}",
                "" +
                "i {\n" +
                "  e s {x2 x1 a1}\n" +
                "  e2 s {x2 x1 a2}\n" +
                "}\n" +
                "s {\n" +
                "  e i {}\n" +
                "}\n");
        }

        [Fact]
        public void FirstSuperStateEntryAndExitActionsAreAdded()
        {
            AssertOptimization("" +
                               "{" +
                               "  (ib) >ibx1 >ibx2 - - -" +
                               "  (sb) <sbn1 <sbn2 - - -" +
                               "  i:ib >x e s a" +
                               "  s:sb <n e i -" +
                               "}",
                "" +
                "i {\n" +
                "  e s {x ibx1 ibx2 sbn1 sbn2 n a}\n" +
                "}\n" +
                "s {\n" +
                "  e i {}\n" +
                "}\n");
        }


        [Fact]
        public void MultipleSuperStateEntryAndExitActionsAreAdded()
        {
            AssertOptimization(        "" +
                                       "{" +
                                       "  (ib1) >ib1x - - -" +
                                       "  (ib2) : ib1 >ib2x - - -" +
                                       "  (sb1) <sb1n- - -" +
                                       "  (sb2) :sb1 <sb2n- - -" +
                                       "  i:ib2 >x e s a" +
                                       "  s:sb2 <n e i -" +
                                       "}",
                "" +
                "i {\n" +
                "  e s {x ib2x ib1x sb1n sb2n n a}\n" +
                "}\n" +
                "s {\n" +
                "  e i {}\n" +
                "}\n");
        }


        [Fact]
        public void DiamondSuperStateEntryAndExitActionsAreAdded()
        {
            AssertOptimization("" +
                               "{" +
                               "  (ib1) >ib1x - - -" +
                               "  (ib2) : ib1 >ib2x - - -" +
                               "  (ib3) : ib1 >ib3x - - -" +
                               "  (sb1) <sb1n - - -" +
                               "  (sb2) :sb1 <sb2n - - -" +
                               "  (sb3) :sb1 <sb3n - - -" +
                               "  i:ib2 :ib3 >x e s a" +
                               "  s :sb2 :sb3 <n e i -" +
                               "}",
                "" +
                "i {\n" +
                "  e s {x ib3x ib2x ib1x sb1n sb2n sb3n n a}\n" +
                "}\n" +
                "s {\n" +
                "  e i {}\n" +
                "}\n");
        }
    }
}