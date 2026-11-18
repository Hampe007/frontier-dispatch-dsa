using NUnit.Framework;

using FrontierDispatch.Core;

namespace FrontierDispatch.Tests
{
    public class ReachabilityQueryTests
    {
        [Test]
        public void MultipleRoutes_ReturnsCheapestCostsAndExcludesDisconnectedLocation()
        {
            var locations = new Location[]
            {
                new Location(0, "Start", new WorldPosition(0, 0), "Town"),
                new Location(1, "Goal", new WorldPosition(2, 0), "Town"),
                new Location(2, "Relay", new WorldPosition(1, 0), "Town"),
                new Location(3, "Disconnected", new WorldPosition(3, 0), "Town")
            };

            var connections = new Connection[]
            {
                new Connection(0, 1, 10),
                new Connection(0, 2, 1),
                new Connection(2, 1, 1)
            };

            var world = new WorldData(locations, connections);
            var query = new ReachabilityQuery(world);

            var result = query.FindReachable(0, 10);

            Assert.That(result[0], Is.EqualTo(0));
            Assert.That(result[1], Is.EqualTo(2));
            Assert.That(result[2], Is.EqualTo(1));
            Assert.That(result.ContainsKey(3), Is.False);
        }

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
