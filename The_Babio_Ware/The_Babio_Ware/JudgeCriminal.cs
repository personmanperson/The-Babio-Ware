using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;


namespace The_Babio_Ware;

public class JudgeCriminal(GameManager gameManager) : Scene(gameManager)
{
    public Texture2D screenTexture;
    public SplashSprite criminal1;
    public SplashSprite criminal2;
    public SplashSprite criminal3;
    //maybe switch to a list, I'll see
    public SpriteFont arcadeFont;
    public SplashSprite ball;
    public SplashSprite hoop;
    public float local_et;
    public bool thrown;
    public float heightThrown;
    public SoundEffect levelAudio;
    public SoundEffectInstance audioInstance;

    public int streak;
    public float comboTimer = 0f;
    public float hoopDirection = 1f;
    public Vector2 screenShakeOffset = Vector2.Zero;
    public float shakeIntensity = 0f;
    private float TIMERTIMERTIEMR;
    private int basketsToReach;

    public override void Load()
    {
        screenTexture = Globals.cm.Load<Texture2D>("court");
        ball = new(Globals.cm.Load<Texture2D>("green"), new(Globals.Random.Next(0, Globals.VirtualWidth), Globals.Random.Next(0, Globals.VirtualHeight)), 1f);
        hoop = new(Globals.cm.Load<Texture2D>("hoop"), new(Globals.VirtualWidth/2, Globals.VirtualHeight/1.55f), 1f);
        criminal1 = new(Globals.cm.Load<Texture2D>("criminal1"), new(Globals.VirtualWidth/2, Globals.VirtualHeight/2), 1f);
        criminal2 = new(Globals.cm.Load<Texture2D>("criminal2"), new(Globals.VirtualWidth/2, Globals.VirtualHeight/2), 1f);
        levelAudio = Globals.cm.Load<SoundEffect>("crumbllabubu");
    }

    public override void Activate()
    {
        local_et = 0;
        ball.spriteScale = 1f;
        thrown = false;
        streak = 0;
        comboTimer = 0f;
        shakeIntensity = 0f;
        screenShakeOffset = Vector2.Zero;
        audioInstance = levelAudio.CreateInstance();
        audioInstance.IsLooped = true;
        audioInstance.Play();
        basketsToReach = Globals.Random.Next(1, 3);
        TIMERTIMERTIEMR = 4f * basketsToReach;
    }

    public override void Deactivate()
    {
        audioInstance.Stop();
    }
    
    public void ResetBall(bool greenash)
    {
        thrown = false;
        ball._pos = new(Globals.Random.Next(0, Globals.VirtualWidth), Globals.Random.Next(0, Globals.VirtualHeight));
        ball.spriteScale = 1f;

        if (greenash)
        {
            streak++;
            comboTimer += 1f;
            shakeIntensity += 1f;
        }
        else
        {
            streak = 0;
            comboTimer -= 1f; //these variables are FAKE and will NEVER be USED maybe
            shakeIntensity -= 0.5f;
        }
    }

    public override void Update()
    {
        local_et += Globals.dt * SceneManager.SpeedScale;
        TIMERTIMERTIEMR -= Globals.dt;
        audioInstance.Pitch = Globals.SinWave * 0.5f * hoopDirection * SceneManager.SpeedScale;

        hoop._pos.X += 50 * hoopDirection * Globals.dt * SceneManager.SpeedScale;
        criminal1._pos.X += 100 * hoopDirection * Globals.dt * SceneManager.SpeedScale;
        criminal2._pos.X += 70 * hoopDirection * Globals.dt * SceneManager.SpeedScale;
        if (hoop._pos.X < 300 || hoop._pos.X > Globals.VirtualWidth - 300)
        {
            hoopDirection *= -1f; // Bounce off the walls
            hoop._pos.X = Math.Clamp(hoop._pos.X, 100, Globals.VirtualWidth - 100) + hoopDirection * 50 * Globals.dt * SceneManager.SpeedScale; // Move it back inside the bounds
        }

        if(ball.spriteScale < 0.1f && thrown && ball.SCALEDRECT.IntersectsFloating(hoop.SCALEDRECT))
        {
            ResetBall(true);
        }

        if(InputManager.rmbClick && !thrown)
        {
            thrown = true;
            heightThrown = InputManager.MousePosition.Y;
        }

        if(thrown)
        {
            ball.spriteScale *= 0.975f;
            ball.velocity.Y -= float.Clamp(ball.velocity.Y - 300f, -5000f, 5000f) * Globals.dt * SceneManager.SpeedScale;
            ball._pos.Y = float.Clamp(ball._pos.Y + ball.velocity.Y * Globals.dt * SceneManager.SpeedScale, 0, Globals.VirtualHeight - (ball._tex.Height/2 * ball.spriteScale)) ;
        }
        else if(ball.SCALEDRECT.IntersectsFloating(InputManager.MouseHitbox) && InputManager.lmb == ButtonState.Pressed)
        {
            ball._pos = new(InputManager.MousePosition.X, InputManager.MousePosition.Y);
            ball.velocity = new(0,0);
        }
        else
        {
            ball.velocity.Y -= float.Clamp(ball.velocity.Y - 600f, -5000f, 5000f) * Globals.dt * SceneManager.SpeedScale;
            ball._pos.Y = float.Clamp(ball._pos.Y + ball.velocity.Y * Globals.dt * SceneManager.SpeedScale, 0, Globals.VirtualHeight - (ball._tex.Height/2 * ball.spriteScale));
            ball._pos.X = float.Clamp(ball._pos.X + ball.velocity.X * Globals.dt * SceneManager.SpeedScale, 0 + (ball._tex.Width/2 * ball.spriteScale), Globals.VirtualWidth - (ball._tex.Width/2 * ball.spriteScale));
        }

        if (ball._pos.Y > Globals.VirtualHeight/1.55f && thrown)
        {
            ResetBall(false);
        }

        if (streak >= basketsToReach)
        {
            gameManager.RequestSceneChange();
        }

        if (TIMERTIMERTIEMR <= 0)
        {
            Globals.currentLives -= 1;
            gameManager.RequestSceneChange();
        }
    }

    public override void Draw()
    {
        Globals.sb.Draw(screenTexture, Globals.gd.Viewport.Bounds, Color.White);
        criminal2.Draw(Globals.sb);
        criminal1.Draw(Globals.sb);
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