namespace Cosmi.Level;

internal enum CardinalDirection
{
    // multiply by pi / 2 to get angle
    Right = 0,
    Up = 1,
    Left = 2,
    Down = 3,
}

internal static class CardinalDirectionExtensions
{
    private static readonly Point[] s_unitVectors = [new(1, 0), new(0, 1), new(-1, 0), new(0, -1)];

    extension(CardinalDirection c)
    {
        public Point UnitVector => s_unitVectors[(int)c & 3];
    }
}