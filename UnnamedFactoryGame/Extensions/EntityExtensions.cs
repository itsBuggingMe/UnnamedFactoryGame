using Frent;

namespace UnnamedFactoryGame.Extensions;

internal static class EntityExtensions
{
    extension(Entity self)
    {
        public Entity? GetConveyorOutput()
        {
            if(self.TryGet<Conveyor>(out var conveyorData))
            {
                Entity e = conveyorData.Value[3];
                if(e.IsAlive)
                {
                    return e;
                }
            }

            return null;
        }
    }
}
