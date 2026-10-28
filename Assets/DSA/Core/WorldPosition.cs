namespace FrontierDispatch.Core
{
    public readonly struct WorldPosition
    {
        public WorldPosition(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float X { get; }
        public float Y { get; }
    }
}
