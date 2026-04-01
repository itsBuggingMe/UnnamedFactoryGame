using Cosmi.Level;
using Frent;
using Frent.Components;
using System;

namespace Cosmi.Components;

internal struct Item : IInitable
{
    public float ConveyorOffset;

    public void Init(Entity self)
    {
        ConveyorOffset = Random.Shared.NextSingle(-0.1f, 0.1f) * TileGrid.TilePixelSize;
    }
}
