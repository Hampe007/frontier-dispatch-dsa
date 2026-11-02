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
        public void FindOutgoingConnections_UnknownSource_ReturnsEmpty()
        {
            var queries = new WorldQueries(SampleWorldFactory.CreateSmall());

            Assert.That(queries.FindOutgoingConnections(9999), Is.Empty);
        }

        [Test]
        public void FindOutgoingConnections_LocationWithNoOutgoing_ReturnsEmpty()
        {
            var locations = new[]
            {
                new Location(2, "Harbor", new WorldPosition(0, 0), "Port")
            };
            var world = new WorldData(locations, new Connection[0]);
            var queries = new WorldQueries(world);

            Assert.That(queries.FindOutgoingConnections(2), Is.Empty);
        }

        [Test]
        public void FindOutgoingConnections_MixedSources_PreservesMatchingOrder()
        {
            var locations = new[]
            {
                new Location(1, "Town", new WorldPosition(0, 0), "Town"),
                new Location(2, "Farm", new WorldPosition(1, 0), "Farm"),
                new Location(3, "Mine", new WorldPosition(2, 0), "Mine")
            };

            var first = new Connection(1, 2, 4);
            var other = new Connection(2, 1, 4);
            var last = new Connection(1, 3, 7);

            var world = new WorldData(locations, new[] { first, other, last });
            var queries = new WorldQueries(world);

            Assert.That(queries.FindOutgoingConnections(1), Is.EqualTo(new[] { first, last }));
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
