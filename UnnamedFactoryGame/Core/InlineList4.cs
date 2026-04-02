using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace UnnamedFactoryGame.Core;

internal struct InlineList4<T> where T : notnull
{
    private InlineArray4<T> _items;
    public int Count { get; private set; }

    [UnscopedRef]
    public Span<T> AsSpan() => ((Span<T>)_items)[..Count];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add(T item)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(Count, 4);

        _items[Count] = item;
        Count++;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool RemoveAt(int index)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(index);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, Count);

        // Shift remaining items down using span copy
        ((Span<T>)_items).Slice(index + 1, Count - index - 1).CopyTo(((Span<T>)_items).Slice(index, Count - index - 1));

        Count--;
        return true;
    }

    public bool Remove(T item)
    {
        var span = AsSpan();

        for (int i = 0; i < span.Length; i++)
        {
            if (span[i].Equals(item))
                return RemoveAt(i);
        }

        return false;
    }

    public void Clear() => Count = 0;

    [UnscopedRef]
    public ref T this[int index]
    {
        get
        {
            ArgumentOutOfRangeException.ThrowIfNegative(index);
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, Count);

            return ref _items[index];
        }
    }

    [UnscopedRef] public Enumerator GetEnumerator() => new(ref this);

    public ref struct Enumerator
    {
        private ref InlineList4<T> _list;
        private int _index;

        internal Enumerator(ref InlineList4<T> list)
        {
            _list = ref list;
            _index = -1;
        }

        [UnscopedRef]
        public ref T Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref _list[_index];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            _index++;
            return _index < _list.Count;
        }
    }
}

