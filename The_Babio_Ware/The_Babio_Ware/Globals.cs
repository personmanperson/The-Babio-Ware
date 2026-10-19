using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace The_Babio_Ware;

public static class Globals
{
    public static int currentLives = 10;
    public static int currentMinigame = 0;
    public static int currentScore = 0;
    public static bool SwitchScene = false;
    public static bool TransitionState = false;
    public static Scenes? ForceScene = null;

    public static int gerberSnacks = 0;

    public static void ResetGlobals()
    {
        currentLives = 10;
        currentMinigame = 0;
        currentScore = 0;
        gerberSnacks = 0;
    }

    public static RenderTarget2D GetNewRenderTarget()
    {
        return new RenderTarget2D(gd, VirtualWidth, VirtualHeight);
    }

    /// <summary>
    /// Below here is more general information that would go in any project
    /// </summary summer-ee>
    
    public static ContentManager cm;
    public static GraphicsDevice gd;
    public static SpriteBatch sb;
    public static GraphicsDeviceManager gdm;

    public static Random Random = new Random();

    public static T GetRandomItem<T>(List<T>  list)
    {
        if (list == null || list.Count == 0)
            throw new ArgumentException("List cannot be null or empty.");

        return list[Random.Next(list.Count)];
    }

    
    public const int VirtualWidth = 1920;
    public const int VirtualHeight = 1080;
    
    public static Vector2 ScreenCenter => new(VirtualWidth/2, VirtualHeight/2);

    public static WindowSettings windowSettings = new WindowSettings() { Width = VirtualWidth, Height = VirtualHeight, Fullscreen = false };

    public static float dt;
    public static float et = 0f;
    public static float SinWave;

    public static void GameTimeVoid(GameTime gameTime)
    {
        dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        et = (float)gameTime.TotalGameTime.TotalSeconds;
        SinWave = (float)Math.Sin(et * 2) * 0.5f + 0.5f;

    }
}