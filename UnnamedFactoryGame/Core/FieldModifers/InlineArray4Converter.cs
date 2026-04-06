using Frent;
using Frent.Marshalling;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Paper.Core.Batcher;
using Paper.Core.Editor;
using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace UnnamedFactoryGame.Core.FieldModifers;

internal class InlineArray4SingleConverter : IFieldModifer
{
    public Entity Entity { set; private get; }
    public ComponentField? FieldToModify { set; private get; }

    public Type FieldType => typeof(InlineArray4<float>);

    public void UpdateUI()
    {
        if (FieldToModify is null)
            return;
        
        ImGui.Text(FieldToModify.Name);

        InlineArray4<float> old = (InlineArray4<float>)FieldToModify.GetValue(Entity.Get(FieldToModify.ComponentID));

        int i = 0;
        foreach (ref var element in old)
        {
            ImGui.InputFloat(i.ToString(), ref element);
            i++;
        }

        FieldToModify.SetValue(Entity, old);
    }
}