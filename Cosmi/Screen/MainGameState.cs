using Cosmi.Components;
using Cosmi.Entities;
using Cosmi.Level;
using Frent;
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
    private readonly World _world;
    private readonly DefaultUniformProvider _uniforms;

    private SpriteBatch SpriteBatch;


    public MainGameScreen(Graphics graphics)
    {
        _graphics = graphics;
        _camera = graphics.Camera;
        _batcher = graphics.Batcher;

        _camera.Scale = new Vector2(4f);

        SpriteBatch = graphics.SpriteBatch;

        _tiles = new();

        _tiles.LoadChunk(default);

        _uniforms = new DefaultUniformProvider()
            .Add(graphics)
            .Add(graphics.Batcher)
            .Add(_tiles)
            ;

        _world = new World(_uniforms);

        for(int i = 0; i < 10; i++)
            _world.Create(new Transform(), new TileEntity(new(10, 5)), Animation.Conveyor, new Sprite());
    }

    public void Update(Time gameTime)
    {
        _uniforms.Add(gameTime);

        Vector2 mousePos = _camera.ScreenToWorld(InputHelper.MouseLocation.ToVector2());

        Vector2 cameraDelta = default;
        if (InputHelper.Down(Keys.W)) cameraDelta += Vector2.UnitY;
        if (InputHelper.Down(Keys.S)) cameraDelta -= Vector2.UnitY;
        if (InputHelper.Down(Keys.D)) cameraDelta -= Vector2.UnitX;
        if (InputHelper.Down(Keys.A)) cameraDelta += Vector2.UnitX;

        _camera.Position += cameraDelta * gameTime.FrameDeltaTime * 5;

        if (InputHelper.Down(MouseButton.Left))
            _tiles.FloorTileAt((_camera.ScreenToWorld(InputHelper.MouseLocation.ToVector2()) / 32).ToPoint()) = default;
        if (InputHelper.Down(MouseButton.Right))
            _tiles.FloorTileAt((_camera.ScreenToWorld(InputHelper.MouseLocation.ToVector2()) / 32).ToPoint()) = FloorTileKind.Grass;

        if (InputHelper.RisingEdge(MouseButton.Left))
        {
            _world.CreateBullet(mousePos, Vector2.UnitX * 16);
            _world.CasingFallingParticle(_graphics.Casing0, mousePos);
        }

        _world.Update<TickAttribute>();
    }

    public void Draw(Time gameTime)
    {
        _graphics.GraphicsDevice.Clear(Color.Black);

        SpriteBatch.Begin(transformMatrix: _camera.View * _camera.Projection);


        _tiles.Draw(_graphics);
        _world.Update<DrawAttribute>();

        _batcher.Submit(_camera.View, _camera.Projection);
        SpriteBatch.End();
    }
}
