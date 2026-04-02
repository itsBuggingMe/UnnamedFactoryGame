using UnnamedFactoryGame.Components;
using UnnamedFactoryGame.Level;
using Frent;
using Paper.Core.Batcher;
using System;
using System.Collections.Generic;
using System.Linq;
using UnnamedFactoryGame.Models;

namespace UnnamedFactoryGame.Registry;

internal class MachineRegistry
{
    public MachineModel this[string name]
    {
        get => _machinesByName[name].Data;
    }

    private readonly List<MachineModel> _machines;
    private readonly Dictionary<string, (MachineModel Data, TextureHandle Handle)> _machinesByName;

    public MachineRegistry(Graphics g)
    {
        _machines = RegistryHelper.DeserializeFromJson<MachineModel>(g, "machines.json");
        _machinesByName = _machines.ToDictionary(m => m.Name, m => (m, g.Batcher.GetTextureHandle(g.Content.Load<Texture2D>(m.Texture))));
    }

    public Entity CreateMachine(World world, string machineName, Point coordinate, CardinalDirection facing = CardinalDirection.Right)
    {
        if (!_machinesByName.TryGetValue(machineName, out var machineData))
            Throwhelper.Throw_ArgumentException($"invalid name {machineName}", nameof(machineName));

        return world.Create(
            new Transform(coordinate.ToVector2() * TileGrid.TilePixelSize, 0),
            new TileEntity(coordinate),
            new Sprite(machineData.Handle),
            new Machine(machineData.Data, world, coordinate) { Facing = facing });
    }
}
