using UnnamedFactoryGame.Components;
using Frent;
using Paper.Core.Batcher;
using System;

namespace UnnamedFactoryGame.Core;

internal static class Factory
{
    extension(World w)
    {
        public Entity CreateBullet(Vector2 position, Vector2 velocity)
        {   
            return w.Create(
                new Transform(position, velocity == default ? default : Helper.MeasureAngle(velocity) + MathHelper.PiOver2),
                new Velocity(velocity),
                new Sprite(w.UniformProvider.GetUniform<Graphics>().Bullet0, true),
                new Decay(60));
        }
    }

    /// <param name="oomph">amount of power the particle is given</param>
    public static Entity CasingFallingParticle(this World w, TextureHandle texture, Vector2 position, bool direction = true, float oomph = 2f, float lifespan = 40)
    {
        float sidewaysVelocity = Random.Shared.NextSingle(0.7f, 0.9f) * (direction ? -1 : 1);
        Vector2 velocity = new(sidewaysVelocity, -oomph);

        const float MaxFadeAnimationLength = 7;

        float animationLength = Math.Min(MaxFadeAnimationLength, lifespan);

        return w.Create(
            new Transform(position, MathHelper.PiOver2),
            new Velocity(velocity, Random.Shared.NextSingle(-0.1f, 0.2f) * (direction ? -1 : 1)),
            new Acceleration(new(0, 0.2f)),
            new Decay(lifespan),
            new Tween<Sprite>(TweenType.InverseCubic, animationLength, lifespan - animationLength, (_, ref s, t) =>
            {
                s.Tint = Color.White * (1 - t);
            }),
            new Sprite(texture, true));
    }
}
