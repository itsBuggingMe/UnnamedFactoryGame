using Frent;
using System;
using System.Buffers;

namespace Cosmi.Level;

internal class Chunk
{
    public const int ChunkSize = 32;

    private FloorTileKind[] _floor;
    private Entity[] _tiles;

    public readonly Point Coordinate;

    public Chunk(Point coordinate)
    {
        Coordinate = coordinate;

        _floor = ArrayPool<FloorTileKind>.Shared.Rent(ChunkSize * ChunkSize);
        _tiles = ArrayPool<Entity>.Shared.Rent(ChunkSize * ChunkSize);
        _floor.AsSpan().Fill(FloorTileKind.Grass);
    }

    public ref Entity this[Point chunkCoord] => ref this[chunkCoord.X, chunkCoord.Y]; 
    public ref Entity this[int cx, int cy]
    {
        get
        {
            ArgumentOutOfRangeException.ThrowIfLessThan((uint)cx, (uint)ChunkSize);
            ArgumentOutOfRangeException.ThrowIfLessThan((uint)cy, (uint)ChunkSize);
            return ref _tiles[cx + cy * ChunkSize];
        }
    }
    public ref FloorTileKind FloorAt(int cx, int cy)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual((uint)cx, (uint)ChunkSize);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual((uint)cy, (uint)ChunkSize);
        return ref _floor[cx + cy * ChunkSize];
    }

    public Span<FloorTileKind> FloorAsSpan() => _floor;
}
