using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace The_Babio_Ware;

public class FIRE
{
    public SplashSprite sprite;
    public float heat = 1;
    public bool extinguished = false;

    public FIRE(Texture2D tex, Vector2 pos, float scale)
    {
        sprite = new(tex, pos, scale);
    }

    public void Update()
    {
        if (!extinguished)
        {
            sprite.spriteScale += Globals.dt * SceneManager.SpeedScale;
        }
    }

    public void Damage()
    {
        sprite.spriteScale = sprite.spriteScale > 1? sprite.spriteScale * 0.2f : sprite.spriteScale - 0.6f;
        if (sprite.spriteScale <= 0.01f)
        {
            extinguished = true;
        }
    }


    public void Draw()
    {
        sprite.Draw(Globals.sb, Color.White, SpriteEffects.None);
    }
}

public class STOPTHEFIRE(GameManager gameManager) : Scene(gameManager)
{
    public Texture2D screenTexture;
    public Texture2D fireTex;
    public Texture2D waterTex;
    public Texture2D steamTex;
    public List<SplashSprite> steams;
    public List<SplashSprite> droplets;
    public List<FIRE> fires;
    public float local_et;
    public int firesExtinguished;
    public float startupTime;
    public bool startBurstUsed = false;
    public int Points;
    public int PointsToWin;
    public float levelTimer;
    public Song levelAudio;

    public bool wateringCanUsed = false;
    public SplashSprite wateringCan;

    public override void Load()
    {
        screenTexture = Globals.cm.Load<Texture2D>("houseview");
        fireTex = Globals.cm.Load<Texture2D>("fire");
        waterTex = Globals.cm.Load<Texture2D>("waterdrop");
        steamTex = Globals.cm.Load<Texture2D>("smokeit");
        wateringCan = new(Globals.cm.Load<Texture2D>("thecan"), new(InputManager.MousePosition.X, InputManager.MousePosition.Y), 0.3f);
        levelAudio = Globals.cm.Load<Song>("fu");
    }

    public override void Activate()
    {
        local_et = 0;
        startupTime = 1;
        firesExtinguished = 0;
        levelTimer = 11f;
        PointsToWin = Globals.Random.Next(7,13);
        Points = 0;
        startBurstUsed = false;
        steams = new();
        droplets = new();
        fires = new();
        MediaPlayer.Play(levelAudio);
    }
    
    public override void Deactivate()
    {
        local_et = 0;
        startupTime = 1;
        firesExtinguished = 0;
        droplets.Clear();
        fires.Clear();
        steams.Clear();
        if (MediaPlayer.State != MediaState.Stopped)
        {
            MediaPlayer.Stop();
        }
    }

    public void DROPWATERFROMCURSOR()
    {
        SplashSprite newWater = new(waterTex, new(InputManager.MousePosition.X, InputManager.MousePosition.Y), 0.3f);
        droplets.Add(newWater);
        wateringCanUsed = true;
        wateringCan.spriteScale = 0.6f;
        wateringCan._rotation = 0.6f;
    }

    public override void Update()
    {
        wateringCanUsed = false;
        local_et += Globals.dt * SceneManager.SpeedScale;
        startupTime -= Globals.dt * SceneManager.SpeedScale;
        levelTimer -= Globals.dt * SceneManager.SpeedScale;

        if(!startBurstUsed && startupTime <= 0)
        {   
            startBurstUsed = true;
            for(int i = 0; i < PointsToWin + 2; i++)
            {
                int randPos = Globals.Random.Next(50, Globals.VirtualWidth - 50);
                FIRE newFire = new (fireTex, new(randPos, Globals.VirtualHeight/1.3f), 1);
                fires.Add(newFire);
            }
        }

        if (InputManager.lmbClick)
        {
            DROPWATERFROMCURSOR();
        }

        foreach(FIRE fireObj in fires)
        {
            fireObj.Update();
        }

        foreach (SplashSprite smoke in steams)
        {
            smoke._pos.Y -= 100f * Globals.dt * SceneManager.SpeedScale;
            smoke.defaultColor *= 0.99f;
            smoke.lifeTime -= Globals.dt * SceneManager.SpeedScale;
        }

        foreach(SplashSprite water in droplets)
        {
            if(water.kill == true){continue;}
            water._pos.Y += 1000f * Globals.dt * SceneManager.SpeedScale;
            if(water._pos.Y > Globals.VirtualHeight)
            {
                water.kill = true;
                continue;
            }

            foreach(FIRE fireObj in fires)
            {
                if (water.SCALEDRECT.Intersects(fireObj.sprite._rect))
                {
                    water.kill = true;
                    fireObj.Damage();
                    SplashSprite newSmoke = new(steamTex, water._pos, 1f, 0.1f);
                    steams.Add(newSmoke);
                    break;
                }
            }   
        }

        if(!wateringCanUsed)
        {
            wateringCan.spriteScale = float.Lerp(wateringCan.spriteScale, 0.3f, 0.1f);
            wateringCan._rotation = float.Lerp(wateringCan._rotation, 0.3f, 0.1f);
        }
        wateringCan._pos = new(InputManager.MousePosition.X, InputManager.MousePosition.Y);
        
        droplets.RemoveAll(water => water.kill);
        fires.RemoveAll(fires => 
            { 
                if (fires.extinguished) {Points++;}
                return fires.extinguished;
            }
        );
        steams.RemoveAll(smoke => smoke.lifeTime <= 0);

        if (levelTimer <= 0 || Points < -5)
        {
            Globals.currentLives -= 1;
            gameManager.RequestSceneChange();
        }

        if (Points >= PointsToWin && fires.Count == 0)
        {
            gameManager.RequestSceneChange();
        }
    }

    public override void Draw()
    {
        Globals.sb.Draw(screenTexture, Globals.gd.Viewport.Bounds, Color.White);

        foreach(SplashSprite water in droplets)
        {
            water.Draw(Globals.sb);
        }

        foreach(FIRE fireObj in fires)
        {
            fireObj.Draw();
        }

        foreach(SplashSprite smoke in steams)
        {
            smoke.Draw(Globals.sb);
        }

        wateringCan.Draw(Globals.sb);
    }

}