using System.Text.Json.Serialization;

namespace UnnamedFactoryGame.Models;

internal record class FloorTileData(
    [property: JsonPropertyName("texture")] string Texture,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("mine_production_output")] string? MineProductionOutput
    );