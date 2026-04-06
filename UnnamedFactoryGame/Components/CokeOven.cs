using Frent;
using Frent.Components;
using System;
using System.Collections.Generic;
using System.Text;
using UnnamedFactoryGame.Models;

namespace UnnamedFactoryGame.Components;

internal struct CokeOven : IInitable
{
    private Entity _coalInputBottom, _coalInputTop, _cokeOutput;
    public float Temperature;
    public float TimeUntilFinish;
    public float BurnTime;

    public void Init(Entity self)
    {
        ref Machine machineData = ref self.Get<Machine>();
        _coalInputTop = machineData.Inputs[0];
        _coalInputBottom = machineData.Inputs[0];
        _cokeOutput = machineData.Outputs[0];
    }

    [Tick]
    public void Update()
    {
        BurnTime = Math.Max(0, BurnTime - 1);

        if (BurnTime <= 0 && _coalInputBottom.GetConveyorOutput() is Entity e &&
            e.TryGet<Item>(out var i) &&
            i.Value.ItemKind is "coal_ore")
        {
            e.Delete();

            BurnTime = 10;
        }

        if(BurnTime > 0)
        {

        }
    }
}
