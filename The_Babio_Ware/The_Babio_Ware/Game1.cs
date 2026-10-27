using System;
using System.Collections.Generic;
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

    public Texture2D fontTexture;
    public static SpriteFont font;
    
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

        fontTexture = Content.Load<Texture2D>("font");

        var characterMap = new Dictionary<char, Rectangle>();
        string imageLayout = @"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()-_=+[]{}|;:'"",.<>?/`~ ";

        for (int i = 0; i < imageLayout.Length; i++)
        {
            char c = imageLayout[i];

            if (characterMap.ContainsKey(c)) continue;

            int x = i % (fontTexture.Width / 8) * 8;
            int y = i / (fontTexture.Width / 8) * 8;

            characterMap[c] = new Rectangle(x, y, 8, 8);
            
        }

        var characters = new List<char>(characterMap.Keys);
        characters.Sort();

        var glyphs = new List<Rectangle>();
        var cropping = new List<Rectangle>();
        var kerning = new List<Vector3>();

        foreach (char c in characters)
        {
            glyphs.Add(characterMap[c]);
            cropping.Add(new Rectangle(0, 0, 8, 8));
            kerning.Add(new Vector3(0, 0, 8));
        }

        font = new SpriteFont(fontTexture, glyphs, cropping, characters, 8, 0f, kerning, '?');

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
