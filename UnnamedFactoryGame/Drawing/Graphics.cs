using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Paper.Core;
using Paper.Core.Batcher;

namespace Cosmi.Drawing;

internal class Graphics : GraphicsBase
{
    public AtlasBatcher Batcher { get; }
    public TextureHandle SimpleTile { get; }
    public TextureHandle GrassTileset { get; }
    public TextureHandle Bullet0 { get; }
    public TextureHandle Casing0 { get; }
    public TextureHandle Round0 { get; }
    public TextureHandle TurretBase { get; }
    public TextureHandle TurretDouble { get; }
    public TextureHandle IronOre { get; }
    public TextureHandle IronOreItem { get; }
    public TextureHandle Mine { get; }

    public Graphics(GraphicsDeviceManager graphicsDeviceManager, ContentManager content, Game game) : base(graphicsDeviceManager, content, game)
    {
        Batcher = new AtlasBatcher(graphicsDeviceManager.GraphicsDevice, content);
        SimpleTile = LoadTexture("tile_simple");
        GrassTileset = LoadTexture("grass_dual");
        Round0 = LoadTexture("round_0");
        Bullet0 = LoadTexture("bullet_0");
        Casing0 = LoadTexture("casing_0");
        TurretBase = LoadTexture("turret_base");
        TurretDouble = LoadTexture("turret_double");
        IronOre = LoadTexture("iron_ore");
        IronOreItem = LoadTexture("iron_ore_item");
        Mine = LoadTexture("mine");

        TextureHandle LoadTexture(string assetName)
        {
            return Batcher.GetTextureHandle(content.Load<Texture2D>(assetName));
        }
    }
}