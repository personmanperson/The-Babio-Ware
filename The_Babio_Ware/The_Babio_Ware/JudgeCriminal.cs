using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace The_Babio_Ware;

public class JudgeCriminal(GameManager gameManager) : Scene(gameManager)
{
    public Texture2D screenTexture;
    public SplashSprite criminal1;
    public SplashSprite criminal2;
    public SplashSprite criminal3;
    //maybe switch to a list, I'll see
    public SplashSprite ball;
    public SplashSprite hoop;
    public float local_et;
    public bool thrown;
    public float heightThrown;

    public override void Load()
    {
        screenTexture = Globals.cm.Load<Texture2D>("court");
        ball = new(Globals.cm.Load<Texture2D>("green"), new(Globals.Random.Next(0, Globals.VirtualWidth), Globals.Random.Next(0, Globals.VirtualHeight)), 1f);
        hoop = new(Globals.cm.Load<Texture2D>("hoop"), new(Globals.VirtualWidth/2, Globals.VirtualHeight/1.55f), 1f);
    }

    public override void Activate()
    {
        local_et = 0;
        ball.spriteScale = 1f;
        thrown = false;
    }

    public override void Deactivate()
    {
        
    }
    
    public void ResetBall()
    {
        thrown = false;
        ball._pos = new(Globals.Random.Next(0, Globals.VirtualWidth), Globals.Random.Next(0, Globals.VirtualHeight));
        ball.spriteScale = 1f;
    }

    public override void Update()
    {
        if(ball.spriteScale < 0.1f && thrown && ball.SCALEDRECT.IntersectsFloating(hoop.SCALEDRECT))
        {
            ResetBall();
        }

        if(InputManager.rmbClick && !thrown)
        {
            thrown = true;
            heightThrown = InputManager.MousePosition.Y;
        }

        if(thrown)
        {
            ball.spriteScale *= 0.975f;
            ball.velocity.Y -= float.Clamp(ball.velocity.Y - 300f, -5000f, 5000f);
            ball._pos.Y = float.Clamp(ball._pos.Y + ball.velocity.Y * Globals.dt, 0, Globals.VirtualHeight - (ball._tex.Height/2 * ball.spriteScale));
        }
        else if(ball.SCALEDRECT.IntersectsFloating(InputManager.MouseHitbox) && InputManager.lmb == ButtonState.Pressed)
        {
            ball._pos = new(InputManager.MousePosition.X, InputManager.MousePosition.Y);
            ball.velocity = new(0,0);
        }
        else
        {
            ball.velocity.Y -= float.Clamp(ball.velocity.Y - 600f, -5000f, 5000f);
            ball._pos.Y = float.Clamp(ball._pos.Y + ball.velocity.Y * Globals.dt, 0, Globals.VirtualHeight - (ball._tex.Height/2 * ball.spriteScale));
            ball._pos.X = float.Clamp(ball._pos.X + ball.velocity.X * Globals.dt, 0 + (ball._tex.Width/2 * ball.spriteScale), Globals.VirtualWidth - (ball._tex.Width/2 * ball.spriteScale));
        }

        if (ball._pos.Y > Globals.VirtualHeight/1.55f && thrown)
        {
            ResetBall();
        }
    }

    public override void Draw()
    {
        Globals.sb.Draw(screenTexture, Globals.gd.Viewport.Bounds, Color.White);
        if(ball.spriteScale < 0.1f)
        {
            ball.Draw(Globals.sb);
            hoop.Draw(Globals.sb);
            
        }
        else
        {
            hoop.Draw(Globals.sb);
            ball.Draw(Globals.sb);
        }
    }

}