using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace UnnamedFactoryGame.Registry;

internal static class RegistryHelper
{
    public static List<T> DeserializeFromJson<T>(Graphics g, string filename)
    {
        Stream jsonDataStream = TitleContainer.OpenStream(Path.Join(g.Content.RootDirectory, "ModelFiles", filename));
        return JsonSerializer.Deserialize<List<T>>(jsonDataStream) ?? [];
    }
}
