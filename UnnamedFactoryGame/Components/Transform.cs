namespace Cosmi.Components;

internal struct Transform(Vector2 pos, float rot)
{
    public Vector2 Position = pos;
    public float Rotation = rot;
}
