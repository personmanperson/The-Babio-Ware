using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace The_Babio_Ware;

public class SplashSprite
{
    public readonly Texture2D _tex;
    public Vector2 _pos;
    public readonly Vector2 _startPos;
    public Rectangle _rect;
    private Vector2 _origin;
    public float spriteScale = 1f;
    public float _rotation = 0f;
    public Color defaultColor = Color.White;
    public bool isHovered = false;
    public bool onTimer = false;
    public bool kill = false;
    public float lifeTime;
    public FloatingPointRectangle SCALEDRECT => new(_pos.X - (_origin.X * spriteScale), _pos.Y - (_origin.Y * spriteScale), _tex.Width * spriteScale, _tex.Height * spriteScale);
    public Vector2 velocity = new(0,0); //this will be used like once


    public SplashSprite(Texture2D tex, Vector2 pos, float scale = 1f)
    {
        _tex = tex;
        _pos = pos;
        _startPos = pos;
        _origin = new Vector2(tex.Width / 2, tex.Height / 2);
        spriteScale = scale;
        Vector2 rectOriginPos = new Vector2(pos.X - _origin.X, pos.Y - _origin.Y);
        _rect = new Rectangle((int)rectOriginPos.X, (int)rectOriginPos.Y, tex.Width, tex.Height);
    }

    public SplashSprite(Texture2D tex, Vector2 pos, float lifetime, float scale = 1f) //on timer
    {
        _tex = tex;
        _pos = pos;
        _startPos = pos;
        _origin = new Vector2(tex.Width / 2, tex.Height / 2);
        spriteScale = scale;
        Vector2 rectOriginPos = new Vector2(pos.X - _origin.X, pos.Y - _origin.Y);
        _rect = new Rectangle((int)rectOriginPos.X, (int)rectOriginPos.Y, tex.Width, tex.Height);
        onTimer = true;
        lifeTime = lifetime;
    }

    public virtual void Update()
    {
        if (InputManager.MouseHitbox.Intersects(_rect))
        {
            isHovered = true;
        }
        else
        {
            isHovered = false;
        }

        if (onTimer)
        {
            lifeTime -= Globals.dt;
        }
    }

    public void Draw(SpriteBatch sb, Color? color = null, SpriteEffects spfx = SpriteEffects.None)
    {
        Color finalColor = color ?? defaultColor;
        sb.Draw(_tex, _pos, null, finalColor, _rotation, _origin, spriteScale, spfx, 0f);
    }
}