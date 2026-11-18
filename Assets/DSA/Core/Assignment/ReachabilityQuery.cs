using System;
using System.Collections.Generic;

namespace FrontierDispatch.Core
{
    // Assignment Phase 5. Adapt shortest-path reasoning to a travel-cost budget.

    /// <summary>
    /// Finds locations that can be reached from a starting location without
    /// exceeding a maximum total travel cost.
    ///
    /// This is an adaptation of shortest-path reasoning: instead of searching
    /// for one goal, the query determines the cheapest known cost to every
    /// location that fits within the supplied budget.
    ///
    /// This class is intentionally incomplete in the assignment starter.
    ///
    /// What do we mean by "query"? In this context, a query is simply a one-time search for information.
    /// </summary>
    public sealed class ReachabilityQuery
    {
        private readonly WorldData _world;

        /// <summary>
        /// Creates a reachability query operating on the supplied world.
        /// </summary>
        public ReachabilityQuery(WorldData world)
        {
            _world = world ?? throw new ArgumentNullException(nameof(world));
        }

        /// <summary>
        /// Returns reachable location IDs mapped to their cheapest known
        /// travel cost from the starting location.
        ///
        /// Only locations whose cheapest cost is within maxTravelCost should
        /// be included. The starting location has cost 0.
        /// </summary>
        public IReadOnlyDictionary<int, int> FindReachable(int startLocationId, int maxTravelCost)
        {
            var bestCost = new Dictionary<int, int>();

            if (_world.FindLocationById(startLocationId) == null || maxTravelCost < 0)
            {
                return bestCost;
            }

            var visited = new HashSet<int>();
            var frontier = new MinPriorityQueue<int>();

            bestCost[startLocationId] = 0;
            frontier.Enqueue(startLocationId, 0);

            while (frontier.Count > 0)
            {
                int current = frontier.Dequeue();

                if (!visited.Add(current))
                {
                    continue;
                }

                foreach (Connection connection in _world.FindOutgoingConnections(current))
                {
                    int remainingBudget = maxTravelCost - bestCost[current];

                    if (connection.TravelCost > remainingBudget)
                    {
                        continue;
                    }

                    int candidate = bestCost[current] + connection.TravelCost;

                    if (!bestCost.TryGetValue(connection.ToLocationId, out int knownCost) || candidate < knownCost)
                    {
                        bestCost[connection.ToLocationId] = candidate;
                        frontier.Enqueue(connection.ToLocationId, candidate);
                    }
                }
            }
            
            return bestCost;
        }
    }
}
