using UnnamedFactoryGame.Components;
using UnnamedFactoryGame.Level;
using Frent;
using Frent.Serialization;
using Paper.Core.Batcher;
using UnnamedFactoryGame.Registry;

namespace UnnamedFactoryGame.Screen;

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
    private readonly FloorTileRegistry _floorTiles;
    private readonly ItemRegistry _items;
    private readonly MachineRegistry _machines;

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
            .Add(graphics.Camera)
            .Add(_tiles)
            .Add(_items = new(graphics))
            .Add(_floorTiles = new(graphics, _items))
            .Add(_machines = new(graphics))
            ;

        _world = new World(_uniforms);
        _uniforms.Add(_world);

        Entity mouseEntity = _world.Create(new Transform(), new MousePosition());

        int i = 1;
        for (; i < 10; i++)
        {
            Entity conveyor = _world.Create(new Transform(), new TileEntity(new(i, 5)), new ItemAcceptor(), new Conveyor(CardinalDirection.Right), Animation.Conveyor, new Sprite());
            
            for(int j = 0; j < 4; j++)
            {
                conveyor.Get<Conveyor>()[j] = _items.CreateItem(_world, default, "9mm");
            }
        }
        
        var turretBase = _world.Create(
            new Transform(),
            new TileEntity(new(i, 5)),
            new ItemAcceptor(),
            new Sprite(graphics.TurretBase)
            );

        _world.Create(
            new Transform(),
            new Turret(turretBase),
            new RotateTowards(mouseEntity),
            new Sprite(graphics.TurretDouble, new Vector2(16)));

        for (i = 5; i < 16; i++)
        {
            var e = _world.Create(new Transform(), new TileEntity(new(i, 8)), new ItemAcceptor(), new Conveyor(CardinalDirection.Right), Animation.Conveyor, new Sprite());
        }

        var mine = _world.Create(
            new Transform(),
            new TileEntity(new(4, 8)),
            new Mine() { Facing = CardinalDirection.Right },
            Animation.Mine,
            new Sprite(graphics.Mine)
            );

        _machines.CreateMachine(_world, "coke_oven", new Point(16, 8));
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

        if (InputHelper.Down(MouseButton.Left) && InputHelper.Down(Keys.LeftControl))
            _tiles.FloorTileAt((_camera.ScreenToWorld(InputHelper.MouseLocation.ToVector2()) / 32).ToPoint()) = FloorTileKind.Coal;
        if (InputHelper.Down(MouseButton.Right) && InputHelper.Down(Keys.LeftControl))
            _tiles.FloorTileAt((_camera.ScreenToWorld(InputHelper.MouseLocation.ToVector2()) / 32).ToPoint()) = FloorTileKind.Grass;

        _camera.Scale *= InputHelper.DeltaScroll switch
        {
            > 0 => 1.1f,
            < 0 => 1 / 1.1f,
            _ => 1
        };

        if(!InputHelper.Down(Keys.F3) || InputHelper.RisingEdge(Keys.P))
        {
            _world.Update<TickAttribute>();
            _world.Update<LateTickAttribute>();
        }
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
