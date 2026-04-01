using Cosmi.Level;
using Frent;
using Frent.Components;
using Frent.Core;
using Paper.Core.Batcher;
using System.Security.Cryptography.X509Certificates;

namespace Cosmi.Components;

internal struct Animation : IInitable, IUniformUpdate<Time, Sprite>
{
    public Rectangle InitalFrame;
    public Point FrameOffset;
    public string Spritesheet;
    public float FrameTime;
    public int FrameCount;
    public TextureHandle Texture;
    public Rectangle CurrentFrame;
    public float CurrentFrameTime;
    public int CurrentFrameIndex;

    public void Init(Entity self)
    {
        var graphics = self.World.UniformProvider.GetUniform<Graphics>();
        Texture = graphics.Batcher.GetTextureHandle(graphics.Content.Load<Texture2D>(Spritesheet));
        CurrentFrame = InitalFrame;

        if(self.TryGet<Sprite>(out var sprite))
        {
            sprite.Value.Texture = Texture;
            sprite.Value.Source = CurrentFrame;
        }
    }

    [Tick]
    public void Update(Time time, ref Sprite sprite)
    {
        CurrentFrameTime += time.FrameDeltaTime;
        if(CurrentFrameTime > FrameTime)
        {
            CurrentFrameTime -= FrameTime;
            CurrentFrameIndex++;

            if(CurrentFrameIndex >= FrameCount)
            {
                CurrentFrameIndex = 0;
                CurrentFrame = InitalFrame;
            }
            else
            {
                CurrentFrame.Location += FrameOffset;
            }

            sprite.Texture = Texture;
            sprite.Source = CurrentFrame;
        }
    }

    public static readonly Animation Conveyor = new()
    {
        InitalFrame = new Rectangle(Point.Zero, new(TileGrid.TilePixelSize)),
        FrameOffset = new(TileGrid.TilePixelSize, 0),
        Spritesheet = "conveyor",
        FrameTime = Components.Conveyor.Speed / 8,
        FrameCount = 8,
    };

    public static readonly Animation Mine = Conveyor with { Spritesheet = "mine" };
}
