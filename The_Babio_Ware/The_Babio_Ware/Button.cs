using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace The_Babio_Ware;

public class Button
{
    private readonly Texture2D _tex;
    private Vector2 _pos;
    private Rectangle _rect;
    private Color _curColor = Color.White;
    
    private int DEBUG = 0;

    public Button(Texture2D tex, Vector2 pos)
    {
        _tex = tex;
        _pos = pos;
        _rect = new Rectangle((int)pos.X, (int) pos.Y, 50, 50); //tex.Width, tex.Height
    }

    public virtual void Update()
    {
        Rectangle mouseRect = new Rectangle(InputManager.MousePosition.X, InputManager.MousePosition.Y, 1, 1);

        if (mouseRect.Intersects(_rect))
        {
            _curColor = Color.DarkGray; //Grey
            if (InputManager.lmb == ButtonState.Pressed)
            {
                DEBUG += 1;
                Console.WriteLine(DEBUG);
            }
        }
        else
        {
            _curColor = Color.White;
        }
    }

    public virtual void Draw(SpriteBatch sb)
    {
        sb.Draw(Game1.DebugPixel, _pos, new((int)_pos.X, (int)_pos.Y, 50, 50), _curColor);
    }
}