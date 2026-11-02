using NUnit.Framework;
using FrontierDispatch.Core;

namespace FrontierDispatch.Tests
{
    public sealed class WorldQueriesTests
    {
        [Test]
        public void FindLocationById_ReturnsMatchingLocation()
        {
            var queries = new WorldQueries(SampleWorldFactory.CreateSmall());
            Location? location = queries.FindLocationById(734);
            Assert.That(location, Is.Not.Null);
            Assert.That(location!.Name, Is.EqualTo("North Gate"));
        }

        [Test]
        public void FindLocationById_ReturnsNullWhenMissing()
        {
            var queries = new WorldQueries(SampleWorldFactory.CreateSmall());
            Assert.That(queries.FindLocationById(9999), Is.Null);
        }

        [Test]
        public void FindLocationById_EmptyWorld_ReturnsNull()
        {
            var world = new WorldData(new Location[0], new Connection[0]);
            var queries = new WorldQueries(world);

            Assert.That(queries.FindLocationById(123), Is.Null);
        }

        [Test]
        public void FindOutgoingConnections_ReturnsOnlyMatchingOrigin()
        {
            var queries = new WorldQueries(SampleWorldFactory.CreateSmall());
            var outgoing = queries.FindOutgoingConnections(734);
            Assert.That(outgoing.Count, Is.EqualTo(3));
            for (int i = 0; i < outgoing.Count; ++i)
            {
                Assert.That(outgoing[i].FromLocationId, Is.EqualTo(734));
            }
        }
        
        [Test]
        public void WorldData_DuplicateLocationId_ThrowsArgumentException()
        {
            var locations = new[]
            {
                new Location(2, "Harbor", new WorldPosition(0, 0), "Port"),
                new Location(2, "Home", new WorldPosition(1, 1), "House")
            };
            Assert.Throws<System.ArgumentException>(() => new WorldData(locations, new Connection[0]));
        }
    }
}
