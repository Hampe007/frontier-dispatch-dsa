using System;
using NUnit.Framework;
using FrontierDispatch.Core;

namespace FrontierDispatch.Tests
{
    public sealed class  DijkstraRouteFinderTests
    {
        //[Test]
        [TestCase(5)]
        [TestCase(50)]
        [TestCase(200)]
        //public void ConnectedLocations_FindsRoute()
        public void ConnectedLocations_FindsRoute(int size)
        {
            //var world = WorldDatasetFactory.CreateSmall();
            var world = WorldDatasetFactory.CreateGrid(size, size);
            var finder = new DijkstraRouteFinder(world);
            int goalId = size * size - 1;
            
            //RouteResult result = finder.FindRoute(0, 24);
            RouteResult result = finder.FindRoute(0, goalId);
            
            Assert.That(result.Found, Is.True);
            Assert.That(result.LocationIds[0], Is.EqualTo(0));
            //Assert.That(result.LocationIds[result.LocationIds.Count - 1], Is.EqualTo(24));
            Assert.That(result.LocationIds[result.LocationIds.Count - 1], Is.EqualTo(goalId));
        }
    }
}

