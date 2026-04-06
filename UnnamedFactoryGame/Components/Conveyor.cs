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
    IUniformUpdate<(Time Time, TileGrid Tiles), Transform, TileEntity, Animation, ItemAcceptor>,
    IUniformUpdate<TileGrid, TileEntity>,
    IInitable
{
    [EditorInclude]
    private InlineArray3<Entity> _items;
    [EditorInclude]
    private InlineArray4<float> _timers;
    private Entity _self;

    [UnscopedRef]
    public ref Entity this[int index] => ref index == 0 ?
        ref _self.Get<ItemAcceptor>().CurrentItem :
        ref _items[index - 1];

    [EditorInclude]
    public bool DisableOutput;

    [UnscopedRef] private Span<Entity> Items => _items;
    public CardinalDirection Direction = cardinalDirection;

    // when an item is pushed to the next conveyor, it is temporaily stored here
    private Entity _tempItem;

    // frames per item
    public const float Speed = 30;

    [Tick]
    public void Update((Time Time, TileGrid Tiles) u, ref Transform transform, ref TileEntity positioned, ref Animation animation, ref ItemAcceptor slot0)
    {
        for(int i = 3; i >= 0; i--)
        {
            ref Entity item = ref this[i];
            ref float timer = ref _timers[i];

            if (!item.IsAlive)
            {
                timer = 0;
                continue;
            }

            timer = Math.Min(Speed, timer + u.Time.FrameDeltaTime);
            if(timer == Speed)
            {
                if(i == 3)
                {
                    if(!DisableOutput &&
                        u.Tiles[positioned.Coordinate + Direction.UnitVector].TryGet<ItemAcceptor>(out var acc) &&
                        acc.Value.TryPlace(item))
                    {
                        item = default;
                        timer = 0;
                    }
                }
                else
                {
                    ref Entity nextSlot = ref this[i + 1];

                    if (!nextSlot.IsAlive)
                    {
                        nextSlot = item;
                        item = default;
                        timer = 0;
                    }
                }
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

        transform.Rotation = (int)Direction * MathHelper.PiOver2;

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
                    Vector2.Lerp(Vector2.Zero, itemSpacing, _timers[i] * (1f / Speed)) +
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

    }
}
