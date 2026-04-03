using UnnamedFactoryGame.Level;
using Frent;
using Frent.Components;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;
using Paper.Core.Editor;
using Frent.Marshalling;

namespace UnnamedFactoryGame.Components;

internal struct Conveyor(CardinalDirection cardinalDirection) :
    IUniformUpdate<(Time Time, TileGrid Tiles), TileEntity, Animation, ItemAcceptor>,
    IUniformUpdate<TileGrid, TileEntity>,
    IInitable
{
    // simple implementation for now
    private float _timer;
    private InlineArray3<Entity> _items;
    private Entity _self;

    [UnscopedRef]
    public ref Entity this[int index] => ref index == 0 ?
        ref _self.Get<ItemAcceptor>().CurrentItem :
        ref _items[index - 1];

    [UnscopedRef] private Span<Entity> Items => _items;
    public CardinalDirection Direction = cardinalDirection;

    // when an item is pushed to the next conveyor, it is temporaily stored here
    private Entity _tempItem;

    // frames per item
    public const float Speed = 30;

    [Tick]
    public void Update((Time Time, TileGrid Tiles) u, ref TileEntity positioned, ref Animation animation, ref ItemAcceptor slot1)
    {
        _timer += u.Time.FrameDeltaTime;
        if(_timer > Speed)
        {
            _timer -= Speed;

            if(!Items[^1].IsAlive ||
                u.Tiles[positioned.Coordinate + Direction.UnitVector].TryGet<ItemAcceptor>(out var acc) &&
                acc.Value.CanPlace)
            {
                _tempItem = Items[^1];
                Items[..2].CopyTo(Items[1..]);
                this[1] = slot1.CurrentItem;
                slot1.CurrentItem = default;
            }
            else
            {// we are blocked :(
                _timer = Speed;
            }
        }

        Vector2 startOffset = (Direction switch
        {
            CardinalDirection.Up => new Vector2(0.5f, 1),
            CardinalDirection.Down => new Vector2(0.5f, 0),
            CardinalDirection.Left => new Vector2(1f, 0.5f),
            CardinalDirection.Right => new Vector2(0, 0.5f),
            _ => throw new UnreachableException(),
        } + positioned.Coordinate.ToVector2()) * TileGrid.TilePixelSize;

        Vector2 itemSpacing = Direction.UnitVector.ToVector2() * 0.25f * TileGrid.TilePixelSize;

        const int ItemCount = 4;
        for(int i = 0; i < ItemCount; i++)
        {
            var e = this[i];
            if (e.TryGet<Transform>(out var itemPos))
            {
                float itemOffset = e.Get<Item>().ConveyorOffset;

                var newpos = (
                    startOffset +
                    itemSpacing * i +
                    Vector2.Lerp(Vector2.Zero, itemSpacing, _timer * (1f / Speed)) +
                    ((CardinalDirection)((int)Direction + 1)).UnitVector.ToVector2() * itemOffset)
                    .ToPoint()
                    .ToVector2();

                itemPos.Value.Position = newpos;
            }
        }
    }

    public void Init(Entity self) => _self = self;

    [LateTick]
    public void Update(TileGrid grid, ref TileEntity arg)
    {
        var x = grid[arg.Coordinate + Direction.UnitVector];
        if (_tempItem.IsAlive &&
            grid[arg.Coordinate + Direction.UnitVector].TryGet<ItemAcceptor>(out var acc) && 
            acc.Value.TryPlace(_tempItem))
        {
            _tempItem = default;
        }
    }
}
