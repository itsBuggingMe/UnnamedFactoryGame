using Frent;
using Frent.Components;
using Paper.Core.Batcher;
using System.Threading;

namespace Cosmi.Components;

internal struct Sprite : IUniformUpdate<Graphics, Transform>, IInitable
{
    public TextureHandle Texture;
    public Vector2 Origin;
    public SpriteEffects Effects;
    public Rectangle? Source;
    public Vector2 Scale = Vector2.One;
    public Color Tint = Color.White;
    private bool _center;

    public Sprite(TextureHandle texture,
                  Vector2 origin = default,
                  SpriteEffects effects = SpriteEffects.None,
                  Rectangle? source = null,
                  Vector2? scale = null,
                  Color? tint = null)
    {
        Texture = texture;
        Origin = origin;
        Effects = effects;
        Source = source;
        Scale = scale ?? Vector2.One;
        Tint = tint ?? Color.White;
    }

    public Sprite(TextureHandle texture, bool isCentered)
    {
        Texture = texture;
        _center = isCentered;
    }

    public Sprite()
    {
        Tint = Color.White;
        Scale = Vector2.One;
    }

    [Draw]
    public readonly void Update(Graphics g, ref Transform pos)
    {
        var sprite = g.Batcher.Draw(Texture, pos.Position, Origin);

        if (Effects is not SpriteEffects.None) sprite.ApplyEffect(Effects);
        if (Source is Rectangle r) sprite.SetSource(r);
        if (Scale != Vector2.One) sprite.Scale(Scale);
        if (Tint != Color.White) sprite.Tint(Tint);
        if (pos.Rotation != 0) sprite.Rotate(pos.Rotation);
    }

    public void Init(Entity self)
    {
        if(_center)
        {
            Origin = self.World.UniformProvider.GetUniform<AtlasBatcher>()
                .GetTextureSize(Texture) * 0.5f;
        }
    }
}
