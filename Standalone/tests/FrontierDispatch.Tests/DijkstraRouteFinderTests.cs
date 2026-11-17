using System;
using NUnit.Framework;
using FrontierDispatch.Core;

namespace FrontierDispatch.Tests
{
    public sealed class  DijkstraRouteFinderTests
    {
        [Test]
        public void ConnectedLocations_FindsRoute()
        {
            var world = WorldDatasetFactory.CreateSmall();
            var finder = new DijkstraRouteFinder(world);

            RouteResult result = finder.FindRoute(0, 24);

            Assert.That(result.Found, Is.True);
            Assert.That(result.LocationIds[0], Is.EqualTo(0));
            Assert.That(result.LocationIds[result.LocationIds.Count - 1], Is.EqualTo(24));
        }
    }
}

