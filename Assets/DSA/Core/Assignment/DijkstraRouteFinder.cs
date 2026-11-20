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
            var costs = new Dictionary<int, int>();
            var previousLocation = new Dictionary<int, int>();
            var visited = new HashSet<int>();
            var queue = new MinPriorityQueue<int>();

            int expandedNodesCount = 0;

            costs[startLocationId] = 0;
            queue.Enqueue(startLocationId, 0);

            while (queue.Count > 0)
            {
                int currentLocationId = queue.Dequeue();

                if (visited.Contains(currentLocationId))
                {
                    continue;
                }
                
                visited.Add(currentLocationId);
                expandedNodesCount++;

                if (currentLocationId == goalLocationId)
                {
                    var path = new List<int>();

                    int pathLocationId = goalLocationId;

                    while (pathLocationId != startLocationId)
                    {
                        path.Add(pathLocationId);
                        pathLocationId = previousLocation[pathLocationId];
                    }

                    path.Add(startLocationId);
                    path.Reverse();
                    return new RouteResult(true, path, costs[goalLocationId], expandedNodesCount);
                }
                
                foreach (Connection connection in _world.FindOutgoingConnections(currentLocationId))
                {
                    int nextLocationId = connection.ToLocationId;
                    int costToNextLocation = costs[currentLocationId] + connection.TravelCost;
                    
                    if (!costs.ContainsKey(nextLocationId) || costToNextLocation < costs[nextLocationId])
                    {
                        costs[nextLocationId] = costToNextLocation;
                        previousLocation[nextLocationId] = currentLocationId;
                        queue.Enqueue(nextLocationId, costToNextLocation);
                    }
                }
            }
            
            return new RouteResult(false, new List<int>(), 0, expandedNodesCount);
        }
    }
}
