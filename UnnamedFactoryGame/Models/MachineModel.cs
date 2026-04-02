using System.Text.Json.Serialization;

namespace UnnamedFactoryGame.Models;


public class MachineModel
{
    [JsonPropertyName("texture")]
    public string Texture { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("inputs")]
    public Input[] Inputs { get; set; }
    [JsonPropertyName("outputs")]
    public Output[] Outputs { get; set; }
    [JsonPropertyName("additional_components")]
    public object[] AdditionalComponents { get; set; }
    [JsonPropertyName("additional_component_types")]
    public object[] AdditionalComponentTypes { get; set; }
}

public class Hitbox
{
    public int x { get; set; }
    public int y { get; set; }
}

public class Input
{
    public int x { get; set; }
    public int y { get; set; }
    public string direction { get; set; }
    public string accepts { get; set; }
}

public class Output
{
    public int x { get; set; }
    public int y { get; set; }
    public string direction { get; set; }
}
