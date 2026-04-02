using Frent;
using Frent.Components;

namespace UnnamedFactoryGame.Components;

internal struct Decay(float TimeLeft) : IEntityUpdate
{

    [Tick]
    public void Update(Entity self)
    {
        if (TimeLeft-- <= 0) self.Delete();
    }
}
