using Frent.Marshalling;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Paper.Core.Batcher;
using Paper.Core.Editor;
using System;
using System.Buffers;

namespace UnnamedFactoryGame.Core.FieldModifers;

internal class TextureHandleFieldConverter : FieldModifierBase<TextureHandle>
{
    protected override TextureHandle UpdateValue(ComponentField field)
    {
        var graphics = Entity.World.UniformProvider.GetUniform<Graphics>();
        var renderer = Entity.World.UniformProvider.GetUniform<ImGuiRenderer>();
        string activeName = _current == default ? "<None>" : graphics.Batcher.GetTexture(_current).Name;

        ImGui.PushID(HashCode.Combine(Entity, field, field));
        if (ImGui.BeginCombo(field.Name, activeName))
        {
            foreach (var (name, textureHandle) in graphics.Batcher.TextureHandles)
            {
                ImGui.Text(name);
                Texture2D texture = graphics.Batcher.GetTexture(textureHandle);
                if(ImGui.ImageButton(name, renderer.BindTexture(texture), texture.Bounds.Size.ToVector2().ToNumerics()))
                {
                    _current = textureHandle;
                    break;
                }
            }
            ImGui.EndCombo();
        }
        ImGui.PopID();

        return _current;
    }
}