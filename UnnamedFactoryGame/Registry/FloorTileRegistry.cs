using UnnamedFactoryGame.Level;
using Frent;
using Paper.Core.Batcher;
using System;
using System.Collections.Generic;
using System.Linq;
using UnnamedFactoryGame.Models;

namespace UnnamedFactoryGame.Registry;

internal class FloorTileRegistry
{
    public FloorTileData this[string name]
    {
        get => _tilesByName[name].Data;
    }

    private readonly List<FloorTileData> _floors;
    private readonly Dictionary<string, (FloorTileData Data, TextureHandle Handle)> _tilesByName;
    private ItemRegistry _items;

    public FloorTileRegistry(Graphics g, ItemRegistry itemRegistry)
    {
        _items = itemRegistry;
        _floors = RegistryHelper.DeserializeFromJson<FloorTileData>(g, "floor_tiles.json");
        _tilesByName = _floors.ToDictionary(i => i.Name, i => 
            (i,
            g.Batcher.GetTextureHandle(g.Content.Load<Texture2D>(i.Texture))));
    }

    public bool TryCreateMineOutput(World world, Point coordinate, FloorTileKind kind, out Entity item)
    {
        if(_tilesByName.TryGetValue(kind.FloorTileRegistryName, out var data) && data.Data.MineProductionOutput is { } someItem)
        {
            item = _items.CreateItem(world, coordinate.ToVector2(), someItem);
            return true;
        }

        item = default;
        return false;
    }
}
