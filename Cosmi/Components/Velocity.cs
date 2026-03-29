using Frent.Components;

namespace Cosmi.Components;

internal struct Velocity(Vector2 xy = default, float dθ = default) : IUpdate<Transform>
{
    public Vector2 DXY = xy;
    public float Dθ = dθ;

    [Tick]
    public readonly void Update(ref Transform pos)
    {
        pos.Position += DXY;
        pos.Rotation += Dθ;
    }
}