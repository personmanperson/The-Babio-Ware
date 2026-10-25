using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace The_Babio_Ware;

public class Curtains
{
    public SplashSprite curtainTextureLeft;
    public SplashSprite curtainTextureRight;
    public float slightDelay = 0.1f;

    public Curtains()
    {
        curtainTextureLeft = new(Globals.cm.Load<Texture2D>("curtain"), new Vector2(0, Globals.VirtualHeight / 2.2f), 1.5f);
        curtainTextureRight = new(Globals.cm.Load<Texture2D>("curtain"), new Vector2(Globals.VirtualWidth, Globals.VirtualHeight / 2.2f), 1.5f);
    }

    public void Update(SceneManager _sceneManager)
    {
        if (_sceneManager.TRANSITIONSCENE == true)
        {
            slightDelay = 0.1f;
            bool closed = MathF.Abs(Globals.VirtualWidth/2 - curtainTextureLeft._pos.X) < 460;
            if (closed) {_sceneManager.EndTransition();}
            else {curtainTextureLeft._pos.X = float.Lerp(curtainTextureLeft._pos.X, Globals.VirtualWidth/3.8f, 0.14f);
            curtainTextureRight._pos.X = float.Lerp(curtainTextureRight._pos.X, Globals.VirtualWidth - Globals.VirtualWidth/3.8f, 0.14f);}
        }
        else
        {
            if(slightDelay <= 0)
            {
                curtainTextureLeft._pos.X = float.Lerp(curtainTextureLeft._pos.X, 0 - curtainTextureLeft._tex.Width/2, 0.03f);
                curtainTextureRight._pos.X = float.Lerp(curtainTextureRight._pos.X, Globals.VirtualWidth + curtainTextureRight._tex.Width/2, 0.03f);
            }
            else {slightDelay -= Globals.dt;}
        }
    }

    public void Draw()
    {
        curtainTextureLeft.Draw(Globals.sb);
        curtainTextureRight.Draw(Globals.sb, null, SpriteEffects.FlipHorizontally);
    }
}