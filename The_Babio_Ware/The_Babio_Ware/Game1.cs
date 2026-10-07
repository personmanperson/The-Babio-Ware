using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace The_Babio_Ware;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public static Texture2D DebugPixel;

    public GameManager gameManager;
    
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        Globals.gdm = _graphics;
        Globals.gd = GraphicsDevice;
        Globals.cm = Content;
        Globals.windowSettings.Init(1920, 1080, false);
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        Globals.sb = _spriteBatch;

        DebugPixel = new Texture2D(GraphicsDevice, 1, 1);
        DebugPixel.SetData(new[] {Color.White});

        gameManager = new GameManager();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        Globals.GameTimeVoid(gameTime);
        InputManager.Update();
        gameManager.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        gameManager.Draw(_spriteBatch);
        base.Draw(gameTime);
    }
}
