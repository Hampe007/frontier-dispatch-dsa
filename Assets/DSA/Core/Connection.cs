using System;

namespace FrontierDispatch.Core
{
    /// <summary>
    /// A directed connection between two world locations with an integer travel cost.
    /// </summary>
    public sealed class Connection
    {
        public Connection(int fromLocationId, int toLocationId, int travelCost)
        {
            if (fromLocationId < 0) throw new ArgumentOutOfRangeException(nameof(fromLocationId));
            if (toLocationId < 0) throw new ArgumentOutOfRangeException(nameof(toLocationId));
            if (travelCost < 0) throw new ArgumentOutOfRangeException(nameof(travelCost));

            FromLocationId = fromLocationId;
            ToLocationId = toLocationId;
            TravelCost = travelCost;
        }

        public int FromLocationId { get; }
        public int ToLocationId { get; }
        public int TravelCost { get; }
    }
}
