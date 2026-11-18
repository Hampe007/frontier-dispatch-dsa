using System;
using System.Collections.Generic;

namespace FrontierDispatch.Core
{
    // Assignment Phase 4.

    /// <summary>
    /// Finds minimum-total-cost routes through a Frontier Dispatch world
    /// using Dijkstra's shortest-path algorithm.
    ///
    /// This class is intentionally incomplete in the assignment starter.
    /// It should eventually use the student's prepared world representation
    /// and MinPriorityQueue implementation.
    /// </summary>
    public sealed class DijkstraRouteFinder
    {
        private readonly WorldData _world;

        /// <summary>
        /// Constructor. Creates a route finder operating on the supplied world.
        /// </summary>
        public DijkstraRouteFinder(WorldData world)
        {
            _world = world ?? throw new ArgumentNullException(nameof(world));
        }

        /// <summary>
        /// Finds a minimum-total-travel-cost route from the start location
        /// to the goal location.
        ///
        /// A successful RouteResult should contain the route as location IDs,
        /// including both the start and goal locations.
        /// </summary>
        public RouteResult FindRoute(int startLocationId, int goalLocationId)
        {
            if (_world.FindLocationById(startLocationId) == null || _world.FindLocationById(goalLocationId) == null)
            {
                return new RouteResult(false, Array.Empty<int>(), 0, 0);
            }

            if (startLocationId == goalLocationId)
            {
                return new RouteResult(true, new[] { startLocationId }, 0, 0);
            }

            var bestCost = new Dictionary<int, int>();
            var cameFrom = new Dictionary<int, int>();
            var visited = new HashSet<int>();
            var frontier = new MinPriorityQueue<int>();

            bestCost[startLocationId] = 0;
            frontier.Enqueue(startLocationId, 0);

            int expandedCount = 0;

            while (frontier.Count > 0)
            {
                int current = frontier.Dequeue();
                
                if (!visited.Add(current))
                {
                    continue;
                }

                expandedCount++;

                if (current == goalLocationId)
                {
                    var path = new List<int> { goalLocationId };
                    int node = goalLocationId;
                    
                    while (node != startLocationId)
                    {
                        node = cameFrom[node];
                        path.Add(node);
                    }

                    path.Reverse();
                    return new RouteResult(true, path, bestCost[goalLocationId], expandedCount);
                }

                foreach (Connection connection in _world.FindOutgoingConnections(current))
                {
                    int candidate = bestCost[current] + connection.TravelCost;

                    if (!bestCost.TryGetValue(connection.ToLocationId, out int knownCost) || candidate < knownCost)
                    {
                        bestCost[connection.ToLocationId] = candidate;
                        cameFrom[connection.ToLocationId] = current;

                        frontier.Enqueue(connection.ToLocationId, candidate);
                    }
                }
            }

            return new RouteResult(false, Array.Empty<int>(), 0, expandedCount);
        }
    }
}
