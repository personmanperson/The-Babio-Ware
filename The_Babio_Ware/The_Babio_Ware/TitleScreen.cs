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
            
            if (startButton.spriteScale <= 0.32f) 
            {
                Globals.currentMinigame = 0;
                gameManager.RequestSceneChange();
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
            
            if (babioPaintButton.spriteScale <= 0.22f) 
            {
                Globals.currentMinigame = 0;
                gameManager.RequestSceneChange();
                Globals.ForceScene = Scenes.BabioPaint; //minigame !!!
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
    }
}