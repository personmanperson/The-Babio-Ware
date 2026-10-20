using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace The_Babio_Ware;

public class PreschoolMath(GameManager gameManager) : Scene(gameManager)
{
    private Texture2D screenTexture;
    private Texture2D handTex;
    private AnimationManager _Anims = new();

    public override void Load()
    {
        screenTexture = Globals.cm.Load<Texture2D>("stock");
        handTex = Globals.cm.Load<Texture2D>("handstuff");
        _Anims.AddAnimation(new Vector2(1,0), new(handTex, 0, 2, 0.3f, 2, 1, 0.8f));
    }
    
    public override void Activate()
    {}

    public override void Deactivate()
    {}

    public override void Update()
    {
        if(InputManager.lmbDown)
        {
            _Anims.SetFrame(new Vector2(1, 0), 1);
        }
        else
        {
            _Anims.SetFrame(new Vector2(1, 0), 0);
        }
    }

    public override void Draw()
    {
        Globals.sb.Draw(screenTexture, Globals.gd.Viewport.Bounds, Color.White);
        _Anims.Draw(new(InputManager.MousePosition.X, Globals.ScreenCenter.Y + (Globals.VirtualHeight/8)), SpriteEffects.None, 0f, null, new(handTex.Width * 0.25f, handTex.Height * 0.5f));
    }
}