using System;

namespace FrontierDispatch.Core
{
    public sealed class DispatchJob
    {
        public DispatchJob(int id, string description)
        {
            if (id < 0) throw new ArgumentOutOfRangeException(nameof(id));
            if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Description is required.", nameof(description));
            Id = id;
            Description = description;
        }

        public int Id { get; }
        public string Description { get; }
    }
}
