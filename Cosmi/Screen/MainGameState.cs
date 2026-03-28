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

        SpriteBatch = graphics.SpriteBatch;
    }

    public void Update(Time gameTime)
    {
        Vector2 cameraDelta = default;
        if (InputHelper.Down(Keys.W)) cameraDelta += Vector2.UnitY;
        if (InputHelper.Down(Keys.S)) cameraDelta -= Vector2.UnitY;
        if (InputHelper.Down(Keys.D)) cameraDelta -= Vector2.UnitX;
        if (InputHelper.Down(Keys.A)) cameraDelta += Vector2.UnitX;

        if (InputHelper.Down(MouseButton.Left))
            _tiles.FloorTileAt((_camera.ScreenToWorld(InputHelper.MouseLocation.ToVector2()) / 32).ToPoint()) = default;
        if (InputHelper.Down(MouseButton.Right))
            _tiles.FloorTileAt((_camera.ScreenToWorld(InputHelper.MouseLocation.ToVector2()) / 32).ToPoint()) = FloorTileKind.Grass;
        _camera.Position += cameraDelta;
    }

    public void Draw(Time gameTime)
    {
        _graphics.GraphicsDevice.Clear(Color.Black);

        SpriteBatch.Begin(transformMatrix: _camera.View * _camera.Projection);

        _tiles.Draw(_graphics);
        _batcher.Submit(_camera.View, _camera.Projection);

        
        SpriteBatch.End();
    }
}
