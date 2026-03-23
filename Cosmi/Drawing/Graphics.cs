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

    public Graphics(GraphicsDeviceManager graphicsDeviceManager, ContentManager content, Game game) : base(graphicsDeviceManager, content, game)
    {
        Batcher = new AtlasBatcher(graphicsDeviceManager.GraphicsDevice, content);
        SimpleTile = LoadTexture("tile_simple");
        GrassTileset = LoadTexture("grass_dual");

        TextureHandle LoadTexture(string assetName)
        {
            return Batcher.CreateHandle(content.Load<Texture2D>(assetName));
        }
    }
}