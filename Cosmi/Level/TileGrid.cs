using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Cosmi.Level;

internal class TileGrid
{
    public const int TilePixelSize = 32;

    private Dictionary<Point, Chunk> _chunks = [];
    private ImmutableArray<Rectangle> TileSources =
    [
        CreateSourceRectangle(2, 1),
        CreateSourceRectangle(0, 0),
        CreateSourceRectangle(0, 0),
        CreateSourceRectangle(0, 0),

        CreateSourceRectangle(0, 0),
        CreateSourceRectangle(0, 0),
        CreateSourceRectangle(0, 0),
        CreateSourceRectangle(0, 0),

        CreateSourceRectangle(0, 0),
        CreateSourceRectangle(0, 0),
        CreateSourceRectangle(0, 0),
        CreateSourceRectangle(0, 0),

        CreateSourceRectangle(0, 0),
        CreateSourceRectangle(0, 0),
        CreateSourceRectangle(0, 0),
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
                    if(chunk.FloorAt(i, j) is not FloorTileKind.Grass)
                    {
                        throw new NotImplementedException();
                    }
                    else
                    {
                        int edges = 0;

                        if (chunk.FloorAt(i, j) is FloorTileKind.Grass) edges |= 0b_0001;
                        if (chunk.FloorAt(i - 1, j) is FloorTileKind.Grass) edges |= 0b_0010;
                        if (chunk.FloorAt(i, j - 1) is FloorTileKind.Grass) edges |= 0b_0100;
                        if (chunk.FloorAt(i - 1, j - 1) is FloorTileKind.Grass) edges |= 0b_1000;

                        g.Batcher.Draw(g.GrassTileset, chunkOriginOffset + new Vector2(i, j) * TilePixelSize)
                            .SetSource(TileSources[edges]);
                    }
                }
            }
        }
    }

    private static Rectangle CreateSourceRectangle(int x, int y)
    {
        return new Rectangle(x * TilePixelSize, y * TilePixelSize, TilePixelSize, TilePixelSize);
    }
}
