using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace UnnamedFactoryGame.Models;

internal record class ItemData(
    [property: JsonPropertyName("texture")] string Texture,
    [property: JsonPropertyName("name")] string Name
    );
