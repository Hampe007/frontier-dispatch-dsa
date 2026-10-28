#nullable enable

using System;
using System.Collections.Generic;

namespace FrontierDispatch.Core
{
    /// <summary>
    /// Straightforward queries over the supplied WorldData.
    ///
    /// The starter project intentionally performs these queries directly over the stored
    /// collections. The implementation is correct, but students should analyze
    /// how its work scales as the world becomes larger and queries are repeated.
    ///
    /// Do not assume that every method must be replaced. Identify the operations
    /// that matter first, then justify any representation changes.
    /// </summary>
    public sealed class WorldQueries
    {
        private readonly WorldData _world;

        public WorldQueries(WorldData world)
        {
            _world = world ?? throw new ArgumentNullException(nameof(world));
        }

        public Location? FindLocationById(int id)
        {
            for (int i = 0; i < _world.Locations.Count; ++i)
            {
                Location location = _world.Locations[i];
                if (location.Id == id)
                {
                    return location;
                }
            }

            return null;
        }

        public IReadOnlyList<Connection> FindOutgoingConnections(int fromLocationId)
        {
            var result = new List<Connection>();
            for (int i = 0; i < _world.Connections.Count; ++i)
            {
                Connection connection = _world.Connections[i];
                if (connection.FromLocationId == fromLocationId)
                {
                    result.Add(connection);
                }
            }

            return result;
        }
    }
}
