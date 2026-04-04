using System.Text.Json.Serialization;

namespace UnnamedFactoryGame.Level;

internal enum CardinalDirection
{
    // multiply by pi / 2 to get angle
    // since down is pos y it seems flipped
    [JsonStringEnumMemberName("right")]
    Right = 0,
    [JsonStringEnumMemberName("down")]
    Down = 1,
    [JsonStringEnumMemberName("left")]
    Left = 2,
    [JsonStringEnumMemberName("up")]
    Up = 3,
}

internal static class CardinalDirectionExtensions
{
    private static readonly Point[] s_unitVectors = [new(1, 0), new(0, -1), new(-1, 0), new(0, 1)];

    extension(CardinalDirection c)
    {
        public Point UnitVector => s_unitVectors[(int)c & 3];
    }
}