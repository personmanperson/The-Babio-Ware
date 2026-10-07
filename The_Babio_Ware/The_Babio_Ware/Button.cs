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
    private Vector2 _origin;
    private Color _curColor = Color.White;
    public bool IsClicked = false;
    public bool IsHovered => _curColor == Color.DarkGray;
    public float spriteScale = 1f;
    public float _rotation = 0f;

    public Button(Texture2D tex, Vector2 pos, float scale = 1f)
    {
        _tex = tex;
        _pos = pos;
        _origin = new Vector2(tex.Width / 2, tex.Height / 2);
        spriteScale = scale;
        Vector2 rectOriginPos = new Vector2(pos.X - _origin.X, pos.Y - _origin.Y);
        _rect = new Rectangle((int)rectOriginPos.X, (int)rectOriginPos.Y, tex.Width, tex.Height);
    }

    public virtual void Update()
    {
        Rectangle mouseRect = new Rectangle(InputManager.MousePosition.X, InputManager.MousePosition.Y, 1, 1);

        if (mouseRect.Intersects(_rect))
        {
            _curColor = Color.DarkGray; //Grey
            if (InputManager.lmb == ButtonState.Pressed)
            {
                IsClicked = true;
            }
            else if (InputManager.lmb == ButtonState.Released && IsClicked)
            {
                IsClicked = false;
                //Debug.WriteLine("Button Clicked");
            }
        }
        else
        {
            _curColor = Color.White;
        }
    }

    public virtual void Draw(SpriteBatch sb)
    {
        sb.Draw(_tex, _pos, null, _curColor, _rotation, _origin, spriteScale, SpriteEffects.None, 0f);
    }
}