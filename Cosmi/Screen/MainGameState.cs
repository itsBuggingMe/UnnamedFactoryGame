using Cosmi.Level;
using Paper.Core.Batcher;

namespace Cosmi.Screen;

internal class MainGameScreen : IScreen
{
    // Graphics objects
    private readonly Graphics _graphics;
    private readonly AtlasBatcher _batcher;
    private readonly Camera2D _camera;

    // World stuff
    private readonly TileGrid _tiles;

    private SpriteBatch SpriteBatch;

    public MainGameScreen(Graphics graphics)
    {
        _graphics = graphics;
        _camera = graphics.Camera;
        _batcher = graphics.Batcher;

        _camera.Scale = new Vector2(4f);

        _tiles = new();

        _tiles.LoadChunk(default);

        SpriteBatch = new SpriteBatch(graphics.GraphicsDevice);
    }

    public void Update(Time gameTime)
    {
        
    }

    public void Draw(Time gameTime)
    {
        _graphics.GraphicsDevice.Clear(Color.Black);

        _tiles.Draw(_graphics);


        _batcher.Submit(_camera.View, _camera.Projection);

        //SpriteBatch.Begin();
        //SpriteBatch.Draw(_batcher._atlas, default(Vector2), Color.White);
        //SpriteBatch.End();
    }
}
