namespace FrontierDispatch.Core
{
    public static class SampleWorldFactory
    {
        public static WorldData CreateSmall()
        {
            var locations = new[]
            {
                new Location(101, "Harbor", new WorldPosition(-4f, -1f), "Port"),
                new Location(205, "Relay", new WorldPosition(-1f, 2f), "Relay"),
                new Location(310, "Crossroads", new WorldPosition(1f, 0f), "Junction"),
                new Location(512, "Old Mine", new WorldPosition(4f, 2f), "Industry"),
                new Location(734, "North Gate", new WorldPosition(3f, 5f), "Gate"),
                new Location(901, "Farm", new WorldPosition(-3f, 5f), "Farm")
            };

            var connections = new[]
            {
                new Connection(101, 205, 4), new Connection(205, 101, 4),
                new Connection(101, 310, 7), new Connection(310, 101, 7),
                new Connection(205, 310, 3), new Connection(310, 205, 3),
                new Connection(205, 901, 5), new Connection(901, 205, 5),
                new Connection(310, 512, 4), new Connection(512, 310, 4),
                new Connection(310, 734, 8), new Connection(734, 310, 8),
                new Connection(512, 734, 3), new Connection(734, 512, 3),
                new Connection(901, 734, 7), new Connection(734, 901, 7)
            };

            return new WorldData(locations, connections);
        }
    }
}
