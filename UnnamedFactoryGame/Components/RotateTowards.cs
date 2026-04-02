using Frent;
using Frent.Components;
using System;

namespace UnnamedFactoryGame.Components;

internal struct RotateTowards(Entity target, float smoothing = 0.9f) : IUpdate<Transform>
{
    public Entity Target = target;
    // 0: no smoothing, 1: doesnt move
    public float Smoothing = smoothing;

    [Tick]
    public void Update(ref Transform t)
    {
        if (Target.TryGet<Transform>(out var targetPos))
        {
            Vector2 delta = targetPos.Value.Position - t.Position;

            float targetAngle = Helper.MeasureAngle(delta);
            float currentAngle = t.Rotation;

            float diff = MathF.Atan2(
                MathF.Sin(targetAngle - currentAngle),
                MathF.Cos(targetAngle - currentAngle)
            );

            t.Rotation += diff * (1 - Smoothing);
        }
    }
}
