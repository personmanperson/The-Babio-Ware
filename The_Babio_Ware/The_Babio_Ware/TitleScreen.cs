using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace The_Babio_Ware;

public class TitleScreen(GameManager gameManager) : Scene(gameManager)
{
    private Texture2D screenTexture;
    public Button startButton;
    public Button babioPaintButton;

    public SplashSprite mj1;
    public SplashSprite mj2;
    public SplashSprite mj3;

    private string[]  startMessages= new string[]
    {
        "Start",
        "Start",
        "Start",
        "Start",
        "Start",
        "Start",
        "Start",
        "Start",
        "Start",
        "Start",
        "Start",
        "Start",
        "Start",
        "Start",
        "Start",
        "Start",
        "Start",
        "Start",
        "Begin",
        "Play",
        "Go",
        "Launch",
        "Commence",
        "Activate",
        "Engage",
        "Initiate",
        "Proceed",
        "russlan is didi",
        "now with 10% less phonk",
        "press [null] to start",
        "man without a body or a nose",
        "NO original music",
        "DONT press",
        "hi!!!!!",
        "press start to press start",
        "hold me to start",
        "hold!",
        "press start to start",
        "some original music",
        "why is the start menu quiet?",
        "do I upset you?",
        "press start to start the game",
        "boo!",
        "my favorite instrument is the piano",
        "gameManager.RequestSceneChange()",
        "hola",
        "bonjour",
        "ciao",
        "hallo",
        "konnichiwa",
        "hello in english",
        "babio soon",
        "babio not paint?",
        "michael jackson button!?!",
        "free API keys!!!",
        "squids = clones",
        "clones = squids",
        "Secret message",
        "less secret message",
        "less secret message 2",
        "less secret message 3",
        "less secret message 4",
        "less secret message 5",
        "least secret message",
        "fatal error, deleting all system files",
    };

    private string startMessage = "This is a fatal fallback message if this ever shows up on your screen rip lmao";

    private bool isTransitioning = false;

    public Color GetColorFromTime(float time, float offset)
    {
        float hue = (time + offset) % 1.0f; // Ensure hue is in the range [0, 1]
        return Color.FromHSV(hue * 360.0f, 1.0f, 1.0f); // Convert to degrees for HSV
    }

    public override void Load()
    {
        screenTexture = Globals.cm.Load<Texture2D>("TitleScreen");
        mj1 = new(Globals.cm.Load<Texture2D>("MJ-3"), new Vector2(Globals.VirtualWidth / 2 - 670, Globals.VirtualHeight / 2 - 50), 0.3f);
        mj2 = new(Globals.cm.Load<Texture2D>("MJ-3"), new Vector2(Globals.VirtualWidth / 2 - 625, Globals.VirtualHeight / 2 - 50), 0.4f);
        mj3 = new(Globals.cm.Load<Texture2D>("MJ-3"), new Vector2(Globals.VirtualWidth / 2 - 575, Globals.VirtualHeight / 2 - 50), 0.5f);
        startButton = new Button(Globals.cm.Load<Texture2D>("StartButton"), new Vector2(Globals.VirtualWidth / 2 - 25, Globals.VirtualHeight / 2 - 25), 0.4f);
        babioPaintButton = new Button(Globals.cm.Load<Texture2D>("PAINT"), new Vector2(Globals.VirtualWidth / 3 - 25, Globals.VirtualHeight / 2 - 25), 0.3f);
    }

    public override void Activate()
    {
        Globals.ResetGlobals();
        startButton.spriteScale = 0.6f;
        startButton.IsClicked = false;
        babioPaintButton.spriteScale = 0.5f;
        babioPaintButton.IsClicked = false;
        isTransitioning = false;

        startMessage = startMessages[Globals.Random.Next(0, startMessages.Length)];
    }

    public override void Deactivate()
    {
        
    }

    public override void Update()
    {
        float sinValue = (float)Math.Sin(Globals.et * 2) * 0.5f + 0.5f;

        if (!startButton.IsClicked)
        {
            if (!startButton.IsHovered)
            {
                startButton.spriteScale = float.Lerp(startButton.spriteScale, 0.5f, 0.05f);
                startButton._rotation = float.Lerp(startButton._rotation, sinValue * 0.05f, 0.1f);
            }
            else
            {
                startButton.spriteScale = float.Lerp(startButton.spriteScale, 0.75f, 0.05f);
                startButton._rotation = float.Lerp(startButton._rotation, sinValue * 0.2f, 0.1f);
            }
        }
        else
        {
            startButton.spriteScale = float.Lerp(startButton.spriteScale, 0.3f, 0.1f);
            
            if (startButton.spriteScale <= 0.32f && !isTransitioning) 
            {
                Globals.currentMinigame = 0;
                gameManager.RequestSceneChange();
                isTransitioning = true;
            }
        }

        if (!babioPaintButton.IsClicked)
        {
            if (!babioPaintButton.IsHovered)
            {
                babioPaintButton.spriteScale = float.Lerp(babioPaintButton.spriteScale, 0.3f, 0.05f);
                babioPaintButton._rotation = float.Lerp(babioPaintButton._rotation, sinValue * 0.2f, 0.1f);
            }
            else
            {
                babioPaintButton.spriteScale = float.Lerp(babioPaintButton.spriteScale, 0.55f, 0.05f);
                babioPaintButton._rotation = float.Lerp(babioPaintButton._rotation, sinValue * 0.4f, 0.1f);
            }
        }
        else
        {
            babioPaintButton.spriteScale = float.Lerp(babioPaintButton.spriteScale, 0.2f, 0.1f);
            
            if (babioPaintButton.spriteScale <= 0.22f && !isTransitioning) 
            {
                Globals.currentMinigame = 0;
                gameManager.ForceSceneChange(Scenes.BabioPaint);
                isTransitioning = true;
            }
        }
        
        mj1._pos.Y = mj2.isHovered? float.Lerp(mj1._pos.Y, mj1._startPos.Y - sinValue * 100, 0.1f) : float.Lerp(mj1._pos.Y, mj1._startPos.Y - sinValue * 10, 0.1f);
        mj2._pos.Y = mj2.isHovered? float.Lerp(mj2._pos.Y, mj1._startPos.Y - sinValue * 100, 0.1f) : float.Lerp(mj1._pos.Y, mj1._startPos.Y - sinValue * 10, 0.1f);
        mj3._pos.Y = mj2.isHovered? float.Lerp(mj3._pos.Y, mj1._startPos.Y - sinValue * 100, 0.1f) : float.Lerp(mj1._pos.Y, mj1._startPos.Y - sinValue * 10, 0.1f);

        startButton.Update();
        babioPaintButton.Update();
        mj1.Update();
        mj2.Update();
        mj3.Update();
    }

    public override void Draw()
    {
        Globals.sb.Draw(screenTexture, Globals.gd.Viewport.Bounds, Color.White);
        mj1.Draw(Globals.sb, GetColorFromTime(Globals.SinWave * 0.1f, 0f));
        mj2.Draw(Globals.sb, GetColorFromTime(Globals.SinWave * 0.1f, 0f));
        mj3.Draw(Globals.sb, GetColorFromTime(Globals.SinWave * 0.1f, 0f));
        startButton.Draw(Globals.sb);
        babioPaintButton.Draw(Globals.sb);
        Globals.sb.DrawString(Game1.font, startMessage, new Vector2(startButton.Rect.X + startButton.Rect.Width/2, startButton.Rect.Bottom), Color.Black, Globals.SinWave * 0.0001f, Game1.font.MeasureString(startMessage)/ 2, 3 * startButton.spriteScale, SpriteEffects.None, 0f);
        Globals.sb.DrawString(Game1.font, "Babio Paint", new Vector2(babioPaintButton.Rect.X + babioPaintButton.Rect.Width/2, babioPaintButton.Rect.Top), Color.Black, Globals.SinWave * 0.1f, Game1.font.MeasureString("Babio Paint") / 2, 2.4f * babioPaintButton.spriteScale, SpriteEffects.None, 0f);
    }
}