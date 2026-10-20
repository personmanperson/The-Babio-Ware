using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace The_Babio_Ware;

public class BabioPaint(GameManager gameManager) : Scene(gameManager)
{
    public Texture2D _currentFoodTex;
    public Vector2 _currentFoodPos = new Vector2(0, Globals.VirtualHeight/2);
    public RenderTarget2D myCanvas;
    public List<FloatingPointRectangle> connectedSlashes; //I want to be a 2D array when I grow up, so users can erase entire line strokes!

    public override void Load()
    {
        
    }
    public override void Activate()
    {
        connectedSlashes = new();
    }

    public override void Deactivate()
    {
        
    }

    public override void Update()
    {
        float mouseDeltaLength = InputManager.MouseDelta.Length();
        float mouseSpeed = mouseDeltaLength / (Globals.dt * 300f);

        if(InputManager.lmbDown)
        {
            Vector2 A = InputManager.LastMousePosition;
            Vector2 B = InputManager.MousePosition;
            float dist = Vector2.Distance(A, B);
            float step = 4f; //this is a constant but I HATE you guys, so it's an EVIL constant, so it will forever stay inside the update loop
            
            for(float d = 0; d < dist; d += step)
            {
                float p = d/dist;

                Vector2 cPos = Vector2.Lerp(A, B, p);
                float safeSpeed = Math.Clamp(mouseSpeed, 0.2f, 5.0f);
                float size = MathHelper.Clamp(12f / safeSpeed, 4f, 12f);
                FloatingPointRectangle r = new (cPos.X, cPos.Y, size, size);
                connectedSlashes.Add(r);
            }
        }

        if(InputManager.KeyDown(Keys.LeftControl) && InputManager.KeyDown(Keys.Z))
        {
            if(connectedSlashes.Count - 1 > -1)
            {
                connectedSlashes.RemoveAt(connectedSlashes.Count - 1);
            }
            else
            {
                Console.WriteLine("Non-Fatal Error: List is empty.");
            }
        }
    }

    public override void Draw()
    {
        Globals.sb.Draw(Game1.DebugPixel, new(InputManager.MousePosition.X, InputManager.MousePosition.Y), null, Color.Red * 0.9f, 0f, new Vector2(0.5f, 0.5f), new Vector2(50, 50), SpriteEffects.None, 0f);
        Globals.sb.Draw(Game1.DebugPixel, new(2 * Globals.ScreenCenter.X - InputManager.MousePosition.X, 2 * Globals.ScreenCenter.Y - InputManager.MousePosition.Y), null, Color.Yellow * 0.9f, 0f, new Vector2(0.5f, 0.5f), new Vector2(50, 50), SpriteEffects.None, 0f);
        Globals.sb.Draw(Game1.DebugPixel, new(InputManager.MousePosition.X, Globals.VirtualHeight/2), null, Color.Blue * 0.9f, 0f, new Vector2(0.5f, 0.5f), new Vector2(50, 50), SpriteEffects.None, 0f);
        Globals.sb.Draw(Game1.DebugPixel, new(Globals.VirtualWidth/2, InputManager.MousePosition.Y), null, Color.Green * 0.9f, 0f, new Vector2(0.5f, 0.5f), new Vector2(50, 50), SpriteEffects.None, 0f);
        Globals.sb.Draw(Game1.DebugPixel, new(Globals.VirtualWidth - InputManager.MousePosition.X, Globals.VirtualHeight/2), null, Color.Magenta * 0.9f, 0f, new Vector2(0.5f, 0.5f), new Vector2(50, 50), SpriteEffects.None, 0f);
        Globals.sb.Draw(Game1.DebugPixel, new(Globals.VirtualWidth/2, Globals.VirtualHeight - InputManager.MousePosition.Y), null, Color.Cyan * 0.9f, 0f, new Vector2(0.5f, 0.5f), new Vector2(50, 50), SpriteEffects.None, 0f);
        Globals.sb.Draw(Game1.DebugPixel, new(Globals.VirtualWidth / 2 + (InputManager.MousePosition.X - Globals.VirtualWidth / 2), Globals.VirtualHeight / 2 + (InputManager.MousePosition.X - Globals.VirtualWidth / 2)), null, Color.White * 0.9f, 0f, new Vector2(0.5f, 0.5f), new Vector2(50, 50), SpriteEffects.None, 0f);
        Globals.sb.Draw(Game1.DebugPixel, new(Globals.VirtualWidth / 2 - (InputManager.MousePosition.X - Globals.VirtualWidth / 2), Globals.VirtualHeight / 2 - (InputManager.MousePosition.X - Globals.VirtualWidth / 2)), null, Color.Gray * 0.9f, 0f, new Vector2(0.5f, 0.5f), new Vector2(50, 50), SpriteEffects.None, 0f);
        foreach (FloatingPointRectangle slash in connectedSlashes)
        {
            Globals.sb.Draw(Game1.DebugPixel, slash.toRect, null, Color.White, 0f, new(0.5f, 0.5f), SpriteEffects.None, 1f);
        }
    }
}