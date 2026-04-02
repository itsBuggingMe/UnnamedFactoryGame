using System;

namespace UnnamedFactoryGame.Level;

public enum FloorTileKind
{
    Void,
    Grass,
    Iron,
    Coal,
}

public static class FloorTileKindExtensions
{
    extension(FloorTileKind k)
    {
        public string FloorTileRegistryName => k switch
        {
            FloorTileKind.Iron => "iron_ore",
            FloorTileKind.Coal => "coal_ore",
            _ => throw new ArgumentException($"{k} not implemented or invalid"),
        };
    }
}