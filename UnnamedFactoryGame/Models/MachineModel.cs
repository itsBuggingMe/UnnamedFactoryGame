using System.Text.Json.Serialization;
using UnnamedFactoryGame.Level;

namespace UnnamedFactoryGame.Models;

internal record class MachineModel(
    [property: JsonPropertyName("texture")] string Texture,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("inputs")] Input[] Inputs,
    [property: JsonPropertyName("outputs")] Output[] Outputs,
    [property: JsonPropertyName("additional_components")] object[] AdditionalComponents,
    [property: JsonPropertyName("additional_component_types")] object[] AdditionalComponentTypes
    );

internal record class Hitbox(
    [property: JsonPropertyName("x")] int X,
    [property: JsonPropertyName("y")] int Y
    );

internal record class Input(
    [property: JsonPropertyName("x")] int X,
    [property: JsonPropertyName("y")] int Y,
    [property: JsonPropertyName("direction"), JsonConverter(typeof(JsonStringEnumConverter))] CardinalDirection Direction,
    [property: JsonPropertyName("accepts")] string Accepts
    );

internal record class Output(
    [property: JsonPropertyName("x")] int X,
    [property: JsonPropertyName("y")] int Y,
    [property: JsonPropertyName("direction"), JsonConverter(typeof(JsonStringEnumConverter))] CardinalDirection Direction
    );
