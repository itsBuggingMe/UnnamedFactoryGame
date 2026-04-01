using Frent.Components;

namespace Cosmi.Components;

internal struct MousePosition : IUniformUpdate<Camera2D, Transform>
{
    [Tick]
    public void Update(Camera2D camera, ref Transform arg)
    {
        arg.Position = camera.ScreenToWorld(InputHelper.MouseLocation.ToVector2());
    }
}
