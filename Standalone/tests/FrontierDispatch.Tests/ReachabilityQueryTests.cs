using NUnit.Framework;

using FrontierDispatch.Core;

namespace FrontierDispatch.Tests
{
    public class ReachabilityQueryTests
    {
        [TestCase(2, false)]
        [TestCase(3, true)]
        public void BudgetBoundary_IncludesLocationOnlyWhenAffordable(int budget, bool expectedReachable)
        {
            var locations = new Location[]
            {
                new Location(0, "Start", new WorldPosition(0, 0), "Town"),
                new Location(1, "Goal", new WorldPosition(1, 0), "Town")
            };

            var connections = new Connection[] { new Connection(0, 1, 3) };
            var world = new WorldData(locations, connections);
            var query = new ReachabilityQuery(world);

            var result = query.FindReachable(0, budget);

            Assert.That(result[0], Is.EqualTo(0));
            Assert.That(result.ContainsKey(1), Is.EqualTo(expectedReachable));
        }
    }
}
