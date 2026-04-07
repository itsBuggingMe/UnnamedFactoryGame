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

internal class InlineArray3EntityConverter : IFieldModifer
{
    public Entity Entity { set; private get; }
    public ComponentField? FieldToModify { set; private get; }

    public Type FieldType => typeof(InlineArray3<Entity>);

    public void UpdateUI()
    {
        if (FieldToModify is null)
            return;

        ImGui.Text(FieldToModify.Name);

        InlineArray3<Entity> old = (InlineArray3<Entity>)FieldToModify.GetValue(Entity.Get(FieldToModify.ComponentID));

        int i = 0;
        foreach (ref var entity in old)
        {
            entity = UpdateOne(entity, i.ToString());
            i++;
        }
        FieldToModify.SetValue(Entity, old);
    }

    private Entity UpdateOne(Entity current, string name)
    {
        int entity = EntityMarshal.EntityID(current);
        if (ImGui.InputInt(name, ref entity, 0, 0, ImGuiInputTextFlags.AutoSelectAll | ImGuiInputTextFlags.EnterReturnsTrue))
        {
            foreach (var potentialTarget in Entity.World.CreateQuery().Build().EnumerateWithEntities())
            {
                if (EntityMarshal.EntityID(potentialTarget) == entity)
                {
                    return potentialTarget;
                }
            }
        }
        return current;
    }
}