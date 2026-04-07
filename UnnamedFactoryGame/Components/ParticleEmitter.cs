using System;
using System.Collections.Generic;
using System.Text;
using Frent;
using Frent.Components;

namespace UnnamedFactoryGame.Components;

internal struct ParticleEmitter : IEntityUniformUpdate<Time,Transform>
{
    public float Interval;
    public Animation? Animation;
    public Velocity InitalVelocity;
    public Sprite Sprite;
    public float CurrentInterval;

    [Tick]
    public void Update(Entity e, Time t, ref Transform pos)
    {
        CurrentInterval -= t.FrameDeltaTime;
        if(CurrentInterval < 0)
        {
            CurrentInterval += Interval;
            if (Animation is { } a)
                e.World.Create(pos, InitalVelocity, a, Sprite);
            else
                e.World.Create(pos, InitalVelocity, Sprite);
        }
    }
}
