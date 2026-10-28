using System;
using System.Collections.Generic;

namespace FrontierDispatch.Core
{
    /// <summary>
    /// Describes the result of a route-search operation.
    /// </summary>
    public sealed class RouteResult
    {
        public RouteResult(bool found, IReadOnlyList<int> locationIds, int totalCost, int expandedCount)
        {
            Found = found;
            LocationIds = locationIds ?? throw new ArgumentNullException(nameof(locationIds));
            TotalCost = totalCost;
            ExpandedCount = expandedCount;
        }

        /// <summary>
        /// True when a route to the requested goal was found.
        /// </summary>
        public bool Found { get; }

        /// <summary>
        /// Location IDs making up the resulting route, in traversal order.
        /// For a successful route, this includes the start and goal.
        /// </summary>
        public IReadOnlyList<int> LocationIds { get; }

        /// <summary>
        /// Sum of the connection travel costs along the resulting route.
        /// </summary>
        public int TotalCost { get; }

        /// <summary>
        /// Number of locations expanded by the search.
        ///
        /// This can be useful when comparing search behavior independently
        /// of wall-clock timing.
        /// </summary>
        public int ExpandedCount { get; }
    }
}
