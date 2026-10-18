using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace The_Babio_Ware;

public class CookFood(GameManager gameManager) : Scene(gameManager)
{
    public Texture2D _currentFoodTex;
    public Vector2 _currentFoodPos = new Vector2(0, Globals.VirtualHeight/2);

    public override void Load()
    {
        
    }
    public override void Activate()
    {
        
    }

    public override void Deactivate()
    {
        
    }

    public override void Update()
    {
        float mouseDeltaLength = InputManager.MouseDelta.Length();
        float mouseSpeed = mouseDeltaLength / (float)Globals.dt;
        Console.WriteLine($"Mouse Speed: {mouseSpeed}");
        _currentFoodPos.X += mouseSpeed * Globals.dt * (InputManager.MouseDelta.X > 0 ? 1 : -1);
        _currentFoodPos.X = MathHelper.Clamp(_currentFoodPos.X, 0, Globals.VirtualWidth);
    }

    public override void Draw()
    {
        Globals.sb.Draw(Game1.DebugPixel, _currentFoodPos, null, Color.Red, 0f, Vector2.Zero, new Vector2(50, 50), SpriteEffects.None, 0f);
    }
}