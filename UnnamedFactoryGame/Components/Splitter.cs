using Frent;
using Frent.Components;
using Paper.Core.Editor;

namespace UnnamedFactoryGame.Components;

internal struct Splitter : IUpdate, IInitable
{
    private Entity _outputA;
    private Entity _outputB;
    private Entity _input;

    public bool _tick;

    [Tick]
    public void Update()
    {
        if(_input.TryGet<Conveyor>(out var inputConveyor))
        {
            ref Entity inputItem = ref inputConveyor.Value[3];
            Entity outputConveyor = _tick ? _outputA : _outputB;

            ref var outputItemAcceptor = ref outputConveyor.Get<ItemAcceptor>();

            if(outputItemAcceptor.TryPlace(inputItem))
            {
                _tick = !_tick;
                inputItem = default;
            }
            else if((_tick ? _outputB : _outputA).TryGet<ItemAcceptor>(out var acc) && acc.Value.TryPlace(inputItem))
            {
                inputItem = default;

            }
        }
    }

    public void Init(Entity self)
    {
        ref Machine machineData = ref self.Get<Machine>();
        _input = machineData.Inputs[0];
        _outputA = machineData.Outputs[0];
        _outputB = machineData.Outputs[1];
    }
}
