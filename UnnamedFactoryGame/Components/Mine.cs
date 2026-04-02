using UnnamedFactoryGame.Level;
using Frent;
using Frent.Components;
using UnnamedFactoryGame.Registry;

namespace UnnamedFactoryGame.Components;

internal struct Mine : IUniformUpdate<(TileGrid Grid, World World, FloorTileRegistry FloorTileRegistry), TileEntity>
{
    public CardinalDirection Facing;

    [Tick]
    public void Update((TileGrid Grid, World World, FloorTileRegistry FloorTileRegistry) u, ref TileEntity position)
    {
        // make more exensible
        FloorTileKind floor = u.Grid.FloorTileAt(position.Coordinate);

        if (floor is not FloorTileKind.Grass &&
            u.Grid[position.Coordinate + Facing.UnitVector].TryGet<ItemAcceptor>(out var itemAcceptor) &&
            itemAcceptor.Value.CanPlace &&
            u.FloorTileRegistry.TryCreateMineOutput(u.World, position.Coordinate, floor, out Entity item))
        {
            itemAcceptor.Value.TryPlace(item);
        }
    }
}
