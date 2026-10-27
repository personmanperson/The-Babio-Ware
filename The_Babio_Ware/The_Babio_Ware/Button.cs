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
    public Vector2 POS {get {return _pos;}}
    private Rectangle _rect;
    private Vector2 _origin;
    private Color _curColor = Color.White;
    public bool IsClicked = false;
    public bool IsHovered = false;
    public float spriteScale = 1f;
    public float _rotation = 0f;

    private FloatingPointRectangle scaled => new(_pos.X - _origin.X * spriteScale, _pos.Y - _origin.Y * spriteScale, _tex.Width * spriteScale, _tex.Height * spriteScale);
    public FloatingPointRectangle Rect {get {return scaled;}}

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
        FloatingPointRectangle mouseRect = new(InputManager.MousePosition, 1f, 1f);
        if (mouseRect.IntersectsFloating(scaled))
        {
            IsHovered = true;
            _curColor = Color.DarkGray; //Grey
            //Console.WriteLine("Hovered");
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
            
            IsHovered = false;
            _curColor = Color.White;
        }
    }

    public virtual void Draw(SpriteBatch sb, Color? color = null)
    {
        if (!color.HasValue)
            color = _curColor;

        sb.Draw(_tex, _pos, null, color.Value, _rotation, _origin, spriteScale, SpriteEffects.None, 0f);
    }
}