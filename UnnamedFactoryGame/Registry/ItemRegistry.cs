using Frent;
using Paper.Core.Batcher;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using UnnamedFactoryGame.Models;

namespace UnnamedFactoryGame.Registry;

internal class ItemRegistry
{
    public ItemData this[string name]
    {
        get => _itemsByName[name].Data;
    }

    private readonly List<ItemData> _items;
    private readonly Dictionary<string, (ItemData Data, TextureHandle Handle)> _itemsByName;

    public ItemRegistry(Graphics g)
    {
        Stream jsonDataStream = TitleContainer.OpenStream(Path.Join(g.Content.RootDirectory, "ModelFiles", "items.json"));
        _items = JsonSerializer.Deserialize<List<ItemData>>(jsonDataStream) ?? [];
        _itemsByName = _items.ToDictionary(i => i.Name, i => (i, g.Batcher.GetTextureHandle(g.Content.Load<Texture2D>(Path.Combine("Textures", i.Texture)))));
    }

    public Entity CreateItem(World world, Vector2 position, string itemName)
    {
        if (!_itemsByName.TryGetValue(itemName, out var itemData))
            Throwhelper.Throw_ArgumentException($"invalid name {itemName}", nameof(itemName));

        return world.Create(
            new Transform(position, Random.Shared.NextSingle() * MathHelper.TwoPi),
            new Item(), 
            new Sprite(itemData.Handle, true));
    }
}
