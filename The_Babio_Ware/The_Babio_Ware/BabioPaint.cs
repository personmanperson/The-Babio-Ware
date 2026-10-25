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
    public List<Vector2> points;
    private RenderTarget2D slashRenderTarget;

    public override void Load()
    {
        
    }
    public override void Activate()
    {
        connectedSlashes = new();
        points = new();
        slashRenderTarget = new RenderTarget2D(
            Globals.gd, 
            Globals.VirtualWidth, 
            Globals.VirtualHeight
        );
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
        else if(InputManager.rmbDown)
        {
            points.Clear();

            float cX = Globals.VirtualWidth / 2; //center X
            float cY = Globals.VirtualHeight / 2; //center Y
            float dx = InputManager.MousePosition.X - cX; //delta X

            Vector2 a = InputManager.MousePosition;
            Vector2 b = new(2 * Globals.ScreenCenter.X - InputManager.MousePosition.X, 2 * Globals.ScreenCenter.Y - InputManager.MousePosition.Y);
            Vector2 c = new(InputManager.MousePosition.X, Globals.VirtualHeight/2);
            Vector2 d = new(Globals.VirtualWidth/2, InputManager.MousePosition.Y);
            Vector2 e = new(Globals.VirtualWidth - InputManager.MousePosition.X, Globals.VirtualHeight/2);
            Vector2 f = new(Globals.VirtualWidth/2, Globals.VirtualHeight - InputManager.MousePosition.Y);
            Vector2 g = new(cX + dx, cY + dx);
            Vector2 h = new(cX - dx, cY - dx);
            Vector2 x = new(cX + dx, cY - dx);
            Vector2 y = new(cX - dx, cY + dx);

            points.Add(a);
            points.Add(b);
            points.Add(c);
            points.Add(d);
            points.Add(e);
            points.Add(f);
            points.Add(g);
            points.Add(h);
            points.Add(x);
            points.Add(y);

            float safeSpeed = Math.Clamp(mouseSpeed, 0.2f, 5.0f);
            float size = MathHelper.Clamp(12f / safeSpeed, 4f, 12f);

            for(int i = 0; i < points.Count; i++)
            {
                for(int k = i+1; k < points.Count; k++)
                {
                    Vector2 p1 = points[i];
                    Vector2 p2 = points[k];

                    float dist = Vector2.Distance(p1, p2);

                    for(float evil = 0; evil < dist; evil += 8f)
                    {
                       float p = evil/dist;

                        Vector2 cPos = Vector2.Lerp(p1, p2, p);
                        
                        FloatingPointRectangle r = new (cPos.X, cPos.Y, size, size);
                        connectedSlashes.Add(r);
                    }
                }
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

    public override RenderTarget2D GetFrame()
    {
        Globals.gd.SetRenderTarget(slashRenderTarget);
        Globals.gd.Clear(Color.Transparent);
        
        Globals.sb.Begin();
        foreach (FloatingPointRectangle slash in connectedSlashes)
        {
            Globals.sb.Draw(Game1.DebugPixel, slash.toRect, null, Color.White, 0f, new(0.5f, 0.5f), SpriteEffects.None, 1f);
        }

        Globals.sb.Draw(Game1.DebugPixel, new(InputManager.MousePosition.X, InputManager.MousePosition.Y), null, Color.Red * 0.9f, 0f, new Vector2(0.5f, 0.5f), new Vector2(50, 50), SpriteEffects.None, 0f);
        Globals.sb.Draw(Game1.DebugPixel, new(2 * Globals.ScreenCenter.X - InputManager.MousePosition.X, 2 * Globals.ScreenCenter.Y - InputManager.MousePosition.Y), null, Color.Yellow * 0.9f, 0f, new Vector2(0.5f, 0.5f), new Vector2(50, 50), SpriteEffects.None, 0f);
        Globals.sb.Draw(Game1.DebugPixel, new(InputManager.MousePosition.X, Globals.VirtualHeight/2), null, Color.Blue * 0.9f, 0f, new Vector2(0.5f, 0.5f), new Vector2(50, 50), SpriteEffects.None, 0f);
        Globals.sb.Draw(Game1.DebugPixel, new(Globals.VirtualWidth/2, InputManager.MousePosition.Y), null, Color.Green * 0.9f, 0f, new Vector2(0.5f, 0.5f), new Vector2(50, 50), SpriteEffects.None, 0f);
        Globals.sb.Draw(Game1.DebugPixel, new(Globals.VirtualWidth - InputManager.MousePosition.X, Globals.VirtualHeight/2), null, Color.Magenta * 0.9f, 0f, new Vector2(0.5f, 0.5f), new Vector2(50, 50), SpriteEffects.None, 0f);
        Globals.sb.Draw(Game1.DebugPixel, new(Globals.VirtualWidth/2, Globals.VirtualHeight - InputManager.MousePosition.Y), null, Color.Cyan * 0.9f, 0f, new Vector2(0.5f, 0.5f), new Vector2(50, 50), SpriteEffects.None, 0f);
        Globals.sb.Draw(Game1.DebugPixel, new(Globals.VirtualWidth / 2 + (InputManager.MousePosition.X - Globals.VirtualWidth / 2), Globals.VirtualHeight / 2 + (InputManager.MousePosition.X - Globals.VirtualWidth / 2)), null, Color.White * 0.9f, 0f, new Vector2(0.5f, 0.5f), new Vector2(50, 50), SpriteEffects.None, 0f);
        Globals.sb.Draw(Game1.DebugPixel, new(Globals.VirtualWidth / 2 - (InputManager.MousePosition.X - Globals.VirtualWidth / 2), Globals.VirtualHeight / 2 - (InputManager.MousePosition.X - Globals.VirtualWidth / 2)), null, Color.Gray * 0.9f, 0f, new Vector2(0.5f, 0.5f), new Vector2(50, 50), SpriteEffects.None, 0f);
        Globals.sb.Draw(Game1.DebugPixel, new(Globals.VirtualWidth / 2 - (InputManager.MousePosition.X - Globals.VirtualWidth / 2), Globals.VirtualHeight / 2 + (InputManager.MousePosition.X - Globals.VirtualWidth / 2)), null, Color.Purple * 0.9f, 0f, new Vector2(0.5f, 0.5f), new Vector2(50, 50), SpriteEffects.None, 0f);
        Globals.sb.Draw(Game1.DebugPixel, new(Globals.VirtualWidth / 2 + (InputManager.MousePosition.X - Globals.VirtualWidth / 2), Globals.VirtualHeight / 2 - (InputManager.MousePosition.X - Globals.VirtualWidth / 2)), null, Color.Beige * 0.9f, 0f, new Vector2(0.5f, 0.5f), new Vector2(50, 50), SpriteEffects.None, 0f);

        Globals.sb.End();
    
        Globals.gd.SetRenderTarget(null);
        
        return slashRenderTarget;
    }

    public override void Draw()
    {
    
    }
}