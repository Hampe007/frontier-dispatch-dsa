using System;
using NUnit.Framework;
using FrontierDispatch.Core;

namespace FrontierDispatch.Tests
{
    public sealed class  DijkstraRouteFinderTests
    {
        [Test]
        public void UnreachableGoal_ReturnsNoRoute()
        {
            var locations = new Location[]
            {
                new Location(0, "Start", new WorldPosition(0, 0), "Town"),
                new Location(1, "Goal", new WorldPosition(1, 0), "Town")
            };

            var world = new WorldData(locations, new Connection[0]);
            var finder = new DijkstraRouteFinder(world);

            RouteResult result = finder.FindRoute(0, 1);

            Assert.That(result.Found, Is.False);
            Assert.That(result.LocationIds, Is.Empty);
            Assert.That(result.TotalCost, Is.EqualTo(0));
            Assert.That(result.ExpandedCount, Is.EqualTo(1));
        }

        [Test]
        public void CheaperRouteWithMoreEdges_ReturnsMinimumCostPath()
        {
            var locations = new Location[]
            {
                new Location(0, "Start", new WorldPosition(0, 0), "Town"),
                new Location(1, "Goal", new WorldPosition(2, 0), "Town"),
                new Location(2, "Relay", new WorldPosition(1, 0), "Town")
            };

            var connections = new Connection[]
            {
                new Connection(0, 1, 10),
                new Connection(0, 2, 1),
                new Connection(2, 1, 1)
            };

            var world = new WorldData(locations, connections);
            var finder = new DijkstraRouteFinder(world);

            RouteResult result = finder.FindRoute(0, 1);

            Assert.That(result.Found, Is.True);
            Assert.That(result.LocationIds, Is.EqualTo(new int[] { 0, 2, 1 }));
            Assert.That(result.TotalCost, Is.EqualTo(2));
            Assert.That(result.ExpandedCount, Is.EqualTo(3));
        }
        
        [TestCase(5)]
        [TestCase(50)]
        [TestCase(200)]
        public void ConnectedLocations_FindsRoute(int size)
        {
            var world = WorldDatasetFactory.CreateGrid(size, size);
            var finder = new DijkstraRouteFinder(world);
            int goalId = size * size - 1;
            
            RouteResult result = finder.FindRoute(0, goalId);
            
            Assert.That(result.Found, Is.True);
            Assert.That(result.LocationIds[0], Is.EqualTo(0));
            Assert.That(result.LocationIds[result.LocationIds.Count - 1], Is.EqualTo(goalId));
        }
    }
}

