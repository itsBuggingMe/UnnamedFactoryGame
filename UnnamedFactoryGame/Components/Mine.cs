using Cosmi.Level;
using Frent;
using Frent.Components;

namespace Cosmi.Components;

internal struct Mine : IUniformUpdate<(TileGrid Grid, World World, Graphics Graphics), TileEntity>
{
    public CardinalDirection Facing;

    [Tick]
    public void Update((TileGrid Grid, World World, Graphics Graphics) u, ref TileEntity position)
    {
        // make more exensible
        if(u.Grid.FloorTileAt(position.Coordinate) is FloorTileKind.Iron && 
            u.Grid[position.Coordinate + Facing.UnitVector].TryGet<ItemAcceptor>(out var itemAcceptor) &&
            itemAcceptor.Value.CanPlace)
        {

            itemAcceptor.Value.TryPlace(u.World.CreateItem(position.Coordinate.ToVector2(), u.Graphics.IronOreItem));
        }
    }
}
