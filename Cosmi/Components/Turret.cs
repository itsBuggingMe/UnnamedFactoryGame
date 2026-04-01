using Cosmi.Level;
using Frent;
using Frent.Components;

namespace Cosmi.Components;

internal struct Turret(Entity @base) : IInitable, IUniformUpdate<Graphics, Transform>
{
    public Entity Base = @base;

    public void Init(Entity self)
    {
        self.Get<Transform>().Position = 
            Base.Get<TileEntity>().Coordinate.ToVector2() * TileGrid.TilePixelSize +
            new Vector2(TileGrid.TilePixelSize / 2);
    }

    [Tick]
    public void Update(Graphics graphics, ref Transform turretPos)
    {
        ref var input = ref Base.Get<ItemAcceptor>();
        if (input.CurrentItem.IsAlive)
        {// we have a bullet... or at least some item

            input.CurrentItem.Delete();
            input.CurrentItem = default;

            Base.World.CreateBullet(turretPos.Position, Vector2.Rotate(Vector2.UnitX * 16, turretPos.Rotation));
            Base.World.CasingFallingParticle(graphics.Casing0, turretPos.Position);
        }
    }
}
