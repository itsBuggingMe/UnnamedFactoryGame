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
        if(_input.TryGet<ItemAcceptor>(out var inputItemAcceptor) && inputItemAcceptor.Value.HasItem)
        {
            Entity outputConveyor = _tick ? _outputA : _outputB;

            ref var outputItemAcceptor = ref outputConveyor.Get<ItemAcceptor>();

            if(outputItemAcceptor.TryPlace(inputItemAcceptor.Value.CurrentItem))
            {
                _tick = !_tick;
                inputItemAcceptor.Value.CurrentItem = default;
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
