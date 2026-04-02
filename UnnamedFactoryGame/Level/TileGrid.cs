using Frent;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnnamedFactoryGame.Level;

internal class TileGrid
{
    public const int TilePixelSize = 32;

    public ref FloorTileKind FloorTileAt(Point worldCoord)
    {
        int lowerMask = Chunk.ChunkSize - 1;
        Point chunkCoord = worldCoord >> Chunk.Log2ChunkSize;
        Point tileCoord = worldCoord & lowerMask;
        return ref _chunks[chunkCoord].FloorAt(tileCoord.X, tileCoord.Y);
    }

    public ref Entity this[Point worldCoord]
    {
        get
        {
            int lowerMask = Chunk.ChunkSize - 1;
            Point chunkCoord = worldCoord >> Chunk.Log2ChunkSize;
            Point tileCoord = worldCoord & lowerMask;
            return ref _chunks[chunkCoord][tileCoord.X, tileCoord.Y];
        }
    }

    private readonly Dictionary<Point, Chunk> _chunks = [];
    private static ImmutableArray<Rectangle> TileSources =
    [
        CreateSourceRectangle(2, 1),
        CreateSourceRectangle(1, 1),
        CreateSourceRectangle(2, 0),
        CreateSourceRectangle(3, 0),

        CreateSourceRectangle(3, 1),
        CreateSourceRectangle(2, 3),
        CreateSourceRectangle(3, 2),
        CreateSourceRectangle(0, 0),

        CreateSourceRectangle(2, 2),
        CreateSourceRectangle(1, 0),
        CreateSourceRectangle(0, 1),
        CreateSourceRectangle(1, 3),

        CreateSourceRectangle(1, 2),
        CreateSourceRectangle(0, 2),
        CreateSourceRectangle(3, 3),
        CreateSourceRectangle(0, 3),
    ];

    public TileGrid()
    {

    }

    public void LoadChunk(Point p)
    {
        _chunks.Add(p, new Chunk(p));
    }

    public void UnloadChunk(Point p)
    {

    }

    public void Draw(Graphics g)
    {
        foreach ((Point coord, Chunk chunk) in _chunks)
        {
            Vector2 chunkOrigin = coord.ToVector2() * Chunk.ChunkSize * TilePixelSize;
            Vector2 chunkOriginOffset = chunkOrigin - new Vector2(TilePixelSize * 0.5f);

            for(int i = 1; i < Chunk.ChunkSize; i++)
            {
                for (int j = 1; j < Chunk.ChunkSize; j++)
                {
                    var tileKind = chunk.FloorAt(i - 1, j - 1);
                    g.Batcher.Draw(g.GrassTileset, chunkOriginOffset + new Vector2(i, j) * TilePixelSize)
                        .SetSource(MapSourceRectangle(
                            tileKind,
                            chunk.FloorAt(i, j - 1),
                            chunk.FloorAt(i - 1, j),
                            chunk.FloorAt(i, j))
                        );
                    if(tileKind is not FloorTileKind.Grass and not FloorTileKind.Void)
                    {
                        g.Batcher.Draw(tileKind switch
                        {
                            FloorTileKind.Coal => g.CoalOre,
                            FloorTileKind.Iron => g.IronOre,
                            _ => throw new NotImplementedException()
                        }, chunkOrigin + new Vector2(i - 1, j - 1) * TilePixelSize);
                    }
                }
            }
        }
    }

    private static Rectangle MapSourceRectangle(
        FloorTileKind tl,
        FloorTileKind tr,
        FloorTileKind bl,
        FloorTileKind br)
    {
        int edges = 0;

        if (tl is FloorTileKind.Grass) edges |= 0b_0001;
        if (tr is FloorTileKind.Grass) edges |= 0b_0010;
        if (br is FloorTileKind.Grass) edges |= 0b_0100;
        if (bl is FloorTileKind.Grass) edges |= 0b_1000;

        return TileSources[edges];
    }

    private static Rectangle CreateSourceRectangle(int x, int y)
    {
        return new Rectangle(x * TilePixelSize, y * TilePixelSize, TilePixelSize, TilePixelSize);
    }
}
