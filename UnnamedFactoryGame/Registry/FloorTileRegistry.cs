using Cosmi.Level;
using Frent;
using Paper.Core.Batcher;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
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
        Stream jsonDataStream = TitleContainer.OpenStream(Path.Join(g.Content.RootDirectory, "ModelFiles", "floor_tiles.json"));
        _floors = JsonSerializer.Deserialize<List<FloorTileData>>(jsonDataStream) ?? [];
        _tilesByName = _floors.ToDictionary(i => i.Name, i => 
            (i,
            g.Batcher.GetTextureHandle(g.Content.Load<Texture2D>(Path.Combine("Textures", i.Texture)))));
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
