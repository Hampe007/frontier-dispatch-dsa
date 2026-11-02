#nullable enable
using System;
using System.Collections.Generic;

namespace FrontierDispatch.Core
{
    public sealed class WorldData
    {
        private readonly Location[] _locations;
        private readonly Connection[] _connections;
        private readonly Dictionary<int, Location> _locationsById;
        private readonly Dictionary<int, List<Connection>> _outgoingByLocationId;
        
        public WorldData(IEnumerable<Location> locations, IEnumerable<Connection> connections)
        {
            if (locations == null) throw new ArgumentNullException(nameof(locations));
            if (connections == null) throw new ArgumentNullException(nameof(connections));
            _locations = new List<Location>(locations).ToArray();
            _connections = new List<Connection>(connections).ToArray();
            _locationsById = new Dictionary<int, Location>(_locations.Length);
            _outgoingByLocationId = new Dictionary<int, List<Connection>>();
            
            foreach (var location in _locations)
            {
                _locationsById.Add(location.Id, location);
            }

            foreach (var connection in _connections)
            {
                if (!_outgoingByLocationId.TryGetValue(connection.FromLocationId, out var outgoing))
                {
                    outgoing = new List<Connection>();
                    _outgoingByLocationId.Add(connection.FromLocationId, outgoing);
                }

                outgoing.Add(connection);
            }
        }

        public IReadOnlyList<Location> Locations => _locations;
        public IReadOnlyList<Connection> Connections => _connections;
        
        public Location? FindLocationById(int id)
        {
            if (_locationsById.TryGetValue(id, out var location))
            {
                return location;
            }

            return null;
        }

        public IReadOnlyList<Connection> FindOutgoingConnections(int fromLocationId)
        {
            if (_outgoingByLocationId.TryGetValue(fromLocationId, out var connections))
            {
                return connections;
            }

            return Array.Empty<Connection>();
        }
    }
}
