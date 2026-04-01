using Frent.Components;

namespace Cosmi.Components;

/// <summary>
/// as if you applied a continuous, conservative force porportional to mass
/// </summary>
internal struct Acceleration(Vector2 val) : IUpdate<Velocity>
{
    public Vector2 Value = val;

    [Tick]
    public void Update(ref Velocity arg)
    {
        arg.DXY += Value;
    }
}
