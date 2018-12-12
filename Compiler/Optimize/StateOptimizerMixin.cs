using System.Collections.Generic;
using System.Linq;
using Smc.Semantic;

namespace Smc.Optimize
{
    internal static class StateOptimizerMixin
    {
        public static IEnumerable<State> HierarchyIncludingSelf(this State state)
        {
            return state.SuperStates.SelectMany(HierarchyIncludingSelf).Concat(new[] { state }).Distinct();
        }


    }
}