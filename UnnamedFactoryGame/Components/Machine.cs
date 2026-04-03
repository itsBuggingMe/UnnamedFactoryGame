using Frent;
using Frent.Components;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using UnnamedFactoryGame.Level;
using UnnamedFactoryGame.Models;

namespace UnnamedFactoryGame.Components;

internal struct Machine : IInitable
{
    public Point Coordinate;
    public CardinalDirection Facing;
    private InlineList4<Entity> _inputs;
    private InlineList4<Entity> _outputs;
    [UnscopedRef] public Span<Entity> Inputs => _inputs.AsSpan();
    [UnscopedRef] public Span<Entity> Outputs => _outputs.AsSpan();

    public Machine(MachineModel model, World world, Point coordinate)
    {
        Coordinate = coordinate;

        foreach (Input input in model.Inputs)
        {
            Point inputCoord = coordinate + new Point(input.X, input.Y);
            Entity inputEntity = world.Create(
                new Transform(),
                new TileEntity(inputCoord),
                new ItemAcceptor(),
                new Conveyor(input.Direction),
                new MachineInput(default),
                Animation.Conveyor,
                new Sprite());

            _inputs.Add(inputEntity);
        }

        foreach (Output output in model.Outputs)
        {
            Point outputCoord = coordinate + new Point(output.X, output.Y);
            Entity outputEntity = world.Create(
                new Transform(),
                new TileEntity(outputCoord),
                new ItemAcceptor(),
                new Conveyor(output.Direction),
                Animation.Conveyor,
                new Sprite());

            _outputs.Add(outputEntity);
        }
    }

    public void Init(Entity self)
    {
        foreach (ref Entity inputEntity in _inputs)
        {
            inputEntity.Get<MachineInput>().Machine = self;
        }
    }
}

