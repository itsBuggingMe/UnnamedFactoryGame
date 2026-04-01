using Cosmi.Screen;

namespace Cosmi;

public class GameRoot : Game
{
    private readonly GraphicsDeviceManager _graphics;

    public GameRoot()
    {
        _graphics = new GraphicsDeviceManager(this)
        {
            GraphicsProfile = GraphicsProfile.HiDef,
        };

        Window.AllowUserResizing = true;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        Graphics graphics = new(_graphics, Content, this);

        ServiceContainer serviceContainer = new();
        serviceContainer
            .Add(serviceContainer)

            .Add(Content)

            .Add(_graphics)
            .Add(_graphics.GraphicsDevice)

            .Add(new Time())

            .Add(graphics)
            .Add(graphics.Camera)
            .Add(graphics.SpriteBatch)
            .Add(graphics.WhitePixel)

            .Add(this)
            ;

        ScreenManager manager = ScreenManager.Create<MainGameScreen>(serviceContainer, this);
        serviceContainer.Add(manager);

        Components.Add(manager);
        base.Initialize();
    }
}