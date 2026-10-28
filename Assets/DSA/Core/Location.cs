using System;

namespace FrontierDispatch.Core
{
    /// <summary>
    /// A location/node in the Frontier Dispatch world graph.
    /// </summary>
    public sealed class Location
    {
        public Location(int id, string name, WorldPosition position, string type)
        {
            if (id < 0) throw new ArgumentOutOfRangeException(nameof(id));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required.", nameof(name));
            if (string.IsNullOrWhiteSpace(type)) throw new ArgumentException("Type is required.", nameof(type));

            Id = id;
            Name = name;
            Position = position;
            Type = type;
        }

        public int Id { get; }
        public string Name { get; }
        public WorldPosition Position { get; }
        public string Type { get; }
    }
}
