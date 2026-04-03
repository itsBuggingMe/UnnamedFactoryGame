using UnnamedFactoryGame.Level;
using Frent;
using Frent.Components;

namespace UnnamedFactoryGame.Components;

internal struct TileEntity(Point coord) : IInitable, IDestroyable
{
    public readonly Point Coordinate = coord;
    public Entity Self;

    public void Init(Entity self)
    {
        ref Entity tileSlot = ref self.World.UniformProvider
            .GetUniform<TileGrid>()[Coordinate];

        if (tileSlot.IsAlive)
            throw new System.Exception("Tile exists here already!");
        tileSlot = self;

        if (self.TryGet<Transform>(out var v))
        {
            v.Value.Position = Coordinate * 32;
        }

        Self = self;
    }

    public readonly void Destroy()
    {
        Self.World.UniformProvider
            .GetUniform<TileGrid>()[Coordinate] = default;
    }
}
