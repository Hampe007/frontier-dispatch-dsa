#nullable enable

using System;
using System.Collections.Generic;

namespace FrontierDispatch.Core
{
    public sealed class WorldQueries
    {
        private readonly WorldData _world;

        public WorldQueries(WorldData world)
        {
            _world = world ?? throw new ArgumentNullException(nameof(world));
        }

        public Location? FindLocationById(int id)
        {
            return _world.FindLocationById(id);
        }

        public IReadOnlyList<Connection> FindOutgoingConnections(int fromLocationId)
        {
            return _world.FindOutgoingConnections(fromLocationId);
        }
    }
}
