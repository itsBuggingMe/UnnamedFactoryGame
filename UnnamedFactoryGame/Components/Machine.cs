using System.Runtime.CompilerServices;
using UnnamedFactoryGame.Level;
using Frent;
using Frent.Components;
using UnnamedFactoryGame.Models;

namespace UnnamedFactoryGame.Components;

internal struct Machine : IInitable
{
    public CardinalDirection Facing;
    private InlineList4<Entity> _inputs;

    public Machine(MachineModel model, World world, Point coordinate)
    {
        foreach (Input input in model.Inputs)
        {
            Point inputCoord = coordinate + new Point(input.X, input.Y);
            Entity inputEntity = world.Create(
                new Transform(),
                new TileEntity(inputCoord),
                new ItemAcceptor(),
                new Conveyor(),
                new MachineInput(default),
                Animation.Conveyor,
                new Sprite());

            _inputs.Add(inputEntity);
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

