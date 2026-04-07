using Frent;
using Frent.Serialization;
using ImGuiNET;
using Paper.Core.Batcher;
using Paper.Core.Editor;
using System;
using System.IO;
using System.Linq;
using UnnamedFactoryGame.Components;
using UnnamedFactoryGame.Level;
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

    private ImguiEditor _debugEditor;

    private Entity _closestEntity;

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


        #region Editor Setup
        _debugEditor = new ImguiEditor(graphics.Game, _world, 
        typeof(MainGameScreen)
                .Assembly
                .GetTypes()
                .Where(t => t.IsAssignableTo(typeof(IFieldModifer)) && !t.IsGenericTypeDefinition)
                .Select(Activator.CreateInstance)
                .Cast<IFieldModifer>());

        _uniforms.Add(_debugEditor);
        _uniforms.Add(_debugEditor.Renderer);
        #endregion

        Entity mouseEntity = _world.Create(new Transform(), new MousePosition());

        /*
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
            new Sprite(graphics.TurretDouble, new Vector2(16)));*/

        for (int i = 5; i < 9; i++)
        {
            _world.Create(new Transform(), new TileEntity(new(i, 8)), new ItemAcceptor(), new Conveyor(CardinalDirection.Right), Animation.Conveyor, new Sprite());
        }

        var mine = _world.Create(
            new Transform(),
            new TileEntity(new(4, 8)),
            new Mine() { Facing = CardinalDirection.Right },
            Animation.Mine,
            new Sprite(graphics.Mine),
            new LateDraw()
            );
        Keyboard.GetState();
        _machines.CreateMachine(_world, "splitter", new Point(9, 8))
            .Add(default(Splitter));// TODO: not hard code this

        for(int i = 11; i < 14; i++)
        {
            _world.Create(new Transform(), new TileEntity(new(i, 8)), new ItemAcceptor(), new Conveyor(CardinalDirection.Right), Animation.Conveyor, new Sprite());
            _world.Create(new Transform(), new TileEntity(new(i, 9)), new ItemAcceptor(), new Conveyor(CardinalDirection.Right), Animation.Conveyor, new Sprite());
        }

        _machines.CreateMachine(_world, "coke_oven", new Point(14, 8));
    }

    public void Update(Time gameTime)
    {
        if (InputHelper.Down(Keys.Escape))
            _graphics.Game.Exit();
        _uniforms.Add(gameTime);

        Vector2 mousePos = _camera.ScreenToWorld(InputHelper.MouseLocation.ToVector2());

        if(!ImGui.GetIO().WantCaptureMouse)
        {
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
        }

        Vector2 cameraDelta = default;
        if (InputHelper.Down(Keys.W)) cameraDelta += Vector2.UnitY;
        if (InputHelper.Down(Keys.S)) cameraDelta -= Vector2.UnitY;
        if (InputHelper.Down(Keys.D)) cameraDelta -= Vector2.UnitX;
        if (InputHelper.Down(Keys.A)) cameraDelta += Vector2.UnitX;

        _camera.Position += cameraDelta * gameTime.FrameDeltaTime * 5;

        if (!InputHelper.Down(Keys.F3) || InputHelper.RisingEdge(Keys.P))
        {
            _world.Update<TickAttribute>();
            _world.Update<LateTickAttribute>();
        }

        Entity closest = default;
        float closestDistance = float.MaxValue;

        foreach (var (e, pos) in _world.CreateQuery()
            .With<Transform>()
            .With<Sprite>()
            .Build()
            .EnumerateWithEntities<Transform>())
        {
            float dist = Vector2.DistanceSquared(pos.Value.Position, mousePos);
            if (dist < closestDistance)
            {
                closest = e;
                closestDistance = dist;
            }
        }

        _closestEntity = closest;

        if (InputHelper.RisingEdge(MouseButton.Left) && GuessBoundingBox(_closestEntity) is { } box && box.Contains(mousePos))
        {
            _debugEditor.SelectedEntity = closest;
        }
    }

    public void Draw(Time gameTime)
    {
        _graphics.GraphicsDevice.Clear(Color.Black);

        SpriteBatch.Begin(transformMatrix: _camera.View);

         
        _tiles.Draw(_graphics);
        _world.Update<EarlyDrawAttribute>();
        _world.Update<DrawAttribute>();
        _world.Update<LateDrawAttribute>();
        DrawEntitySelection(_closestEntity, true);
        DrawEntitySelection(_debugEditor.SelectedEntity, false);

        _batcher.Submit(_camera.View, _camera.Projection);

        SpriteBatch.End();
            
        _debugEditor.Draw(gameTime.GameTime);
    }

    private void DrawEntitySelection(Entity e, bool onlyDrawWhenOver)
    {
        if (GuessBoundingBox(e) is not { } rectangleToDraw)
            return;

        Vector2 mousePos = _camera.ScreenToWorld(InputHelper.MouseLocation.ToVector2());
        if (onlyDrawWhenOver && !rectangleToDraw.Contains(mousePos))
            return;


        const int Width = 1;
        SpriteBatch.Draw(_graphics.WhitePixel, new Rectangle(rectangleToDraw.Left, rectangleToDraw.Top, rectangleToDraw.Width, Width), Color.Red);
        SpriteBatch.Draw(_graphics.WhitePixel, new Rectangle(rectangleToDraw.Left, rectangleToDraw.Bottom - Width, rectangleToDraw.Width, Width), Color.Red);
        SpriteBatch.Draw(_graphics.WhitePixel, new Rectangle(rectangleToDraw.Left, rectangleToDraw.Top, Width, rectangleToDraw.Height), Color.Red);
        SpriteBatch.Draw(_graphics.WhitePixel, new Rectangle(rectangleToDraw.Right - Width, rectangleToDraw.Top, Width, rectangleToDraw.Height), Color.Red);
        SpriteBatch.Draw(_graphics.WhitePixel, e.Get<Transform>().Position, Color.Blue);
    }

    private Rectangle? GuessBoundingBox(Entity e)
    {
        if (!e.IsAlive)
            return null;

        if (!e.TryGet<Transform>(out var transform))
            return null;

        Rectangle? bounds = e.TryGet<Sprite>(out var sprite) ?
            GetRectangleSprite(sprite.Value, transform.Value.Position) :
            null;

        return bounds;

        Rectangle GetRectangleSprite(in Sprite s, Vector2 pos)
        {
            Vector2 position = pos - s.Origin;
            Vector2 size = s.Source?.Size.ToVector2() ?? _batcher.GetTextureSize(s.Texture) * s.Scale;
            return new Rectangle(position.ToPoint(), size.ToPoint());
        }
    }
}
