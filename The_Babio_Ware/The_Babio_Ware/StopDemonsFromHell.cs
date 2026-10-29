using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace The_Babio_Ware;

public class Demon
{
    public Texture2D tex;
    public Vector2 origin;
    public AnimationManager anim = new();
    public Vector2 pos;
    public float scale = 1f;
    public bool vanquishedkilleddeadwhatisitcalled = false; //"but it's false on intialization"... yeah.. yeah sigh, I'm just a dummy right?
    public float deadtimer = 1f;
    public Color colores;

    public FloatingPointRectangle rect => new(pos.X - (origin.X * scale), pos.Y - (origin.Y * scale), tex.Width/5f * scale, tex.Height * scale);

    public Demon(Texture2D _tex, Vector2 _pos)
    {
        tex = _tex;
        pos = _pos;
        origin = new(_tex.Width/5f, _tex.Height/2f);
        colores = Color.White;
        anim.AddAnimation(new Vector2(1,0), new(tex, 0, 4, 0.1f, 5, 1)); //for convience, every demon has 5 frames
    }

    public void Shot()
    {
        vanquishedkilleddeadwhatisitcalled = true;
        colores = Color.Red;
    }

    public void Update()
    {

        anim.Update(new Vector2(1,0));

        if(!vanquishedkilleddeadwhatisitcalled)
        {
            scale = MathHelper.Clamp(scale + 0.005f * SceneManager.SpeedScale, -0.001f, 3f);
        }
        else
        {
            scale = MathHelper.Clamp(scale - 0.005f * SceneManager.SpeedScale, 0.001f, 3f);
            deadtimer -= Globals.dt * SceneManager.SpeedScale;
        }
    }

    public void Draw()
    {
        anim.Draw(pos, SpriteEffects.None, 0f, colores, origin, scale);
        //Globals.sb.Draw(Game1.DebugPixel, rect.ToRectangle(), Color.White);
    }
}

public class StopDemonsFromHell(GameManager gameManager) : Scene(gameManager)
{

    public Color GetColorFromTime(float time, float offset)
    {
        float hue = (time + offset) % 1.0f; // Ensure hue is in the range [0, 1]
        return Color.FromHSV(hue * 360.0f, 1.0f, 1.0f); // Convert to degrees for HSV
    }

    public Texture2D screenTexture;
    public Texture2D GunSpriteSheet;
    public List<Texture2D> DemonSpriteSheets = new();
    public List<Demon> Demons;
    public SplashSprite Crosshair;
    private AnimationManager _Anims = new();
    public Vector2 GunPos = new(Globals.VirtualWidth/2f, Globals.VirtualHeight/2f);
    public Vector2 gunOrigin;
    public SoundEffect gunShot;
    public SoundEffect squishShot;

    private float LevelTimer = 0f;

    public int demonDeathCount = 0;

    public readonly float CD = 0.31f;
    public readonly float Shot = 0.3f;
    public readonly float ShotSUPER = 0.06f;
    public readonly float SuperTime = 3f;
    public bool SuperCharge = true; //SECRET !!!!
    public float timer = 0f;
    public float timer2 = 0f;
    public float secretTimer = 0f;
    public float local_et;
    public float total_et; //persists bc lazy
    public bool shot = false;
    public Song levelAudio;

    
   public override void Load()
    {
        screenTexture = Globals.cm.Load<Texture2D>("hell");
        GunSpriteSheet = Globals.cm.Load<Texture2D>("gun");
        Crosshair = new(Globals.cm.Load<Texture2D>("gerberhair"), new(0,0), 0.15f);
        _Anims.AddAnimation(new Vector2(1,0), new(GunSpriteSheet, 0, 5, 0.05f, 6, 1));
        _Anims.AddAnimation(new Vector2(2,0), new(GunSpriteSheet, 0, 5, 0.01f, 6, 1));
        Texture2D demonTex1 = Globals.cm.Load<Texture2D>("flanders");
        Texture2D demonTex2 = Globals.cm.Load<Texture2D>("blueguy");
        Texture2D demonTex3 = Globals.cm.Load<Texture2D>("moe");
        //Texture2D demonTex4 = Globals.cm.Load<Texture2D>("empty");
        DemonSpriteSheets.Add(demonTex1);
        DemonSpriteSheets.Add(demonTex2);
        DemonSpriteSheets.Add(demonTex3);
        //DemonSpriteSheets.Add(demonTex4);
        gunShot = Globals.cm.Load<SoundEffect>("pew");
        squishShot = Globals.cm.Load<SoundEffect>("squish");
        levelAudio = Globals.cm.Load<Song>("spook");
    }

   public override void Activate()
    {
        gunOrigin = new Vector2(GunSpriteSheet.Width/6f, GunSpriteSheet.Height / 2f);
        Demons = new();
        local_et = 0f;
        SuperCharge = true;
        var sheet = Globals.GetRandomItem(DemonSpriteSheets);
        Demon newDemon = new(sheet, Globals.ScreenCenter);
        Demons.Add(newDemon);
        LevelTimer = 0f;
        MediaPlayer.Play(levelAudio);
        Active = true;
    }
   
   public override void Deactivate()
    {
        if (MediaPlayer.State != MediaState.Stopped)
        {
            MediaPlayer.Stop();
        }
    }

    private void ShootGun(float minRange, float maxRange) //positive please
    {
        Crosshair.spriteScale = 0.4f;
        FloatingPointRectangle stupidDummyMouseRectwhydoIneedsomanyomgwhydontIjustmakeaglobalvalueurgghhhh = new(Crosshair._pos.X, Crosshair._pos.Y, 50f, 50f);
        gunShot.Play(1f, MathHelper.Clamp(Globals.Random.Next(-10, 11) * 0.1f, -minRange, maxRange), 0f);
        squishShot.Play(1f, MathHelper.Clamp(Globals.Random.Next(-10, 11) * 0.1f, -minRange, maxRange), 0f);
        foreach(Demon demon in Demons)
        {
            if (stupidDummyMouseRectwhydoIneedsomanyomgwhydontIjustmakeaglobalvalueurgghhhh.IntersectsFloating(demon.rect))
            {
                demon.Shot();
            }
        }
    }

   public override void Update()
    {
        total_et += Globals.dt * SceneManager.SpeedScale;
        local_et += Globals.dt * SceneManager.SpeedScale;
        LevelTimer += Globals.dt * SceneManager.SpeedScale;

        if (local_et >= 0.08f)
        {
            local_et = 0f;
            var sheet = Globals.GetRandomItem(DemonSpriteSheets);
            Demon newDemon = new(sheet, new Vector2(Globals.Random.Next(250, Globals.VirtualWidth - 250), Globals.Random.Next(250, Globals.VirtualHeight - 250)));
            Demons.Add(newDemon);
        }

        Crosshair._pos = new(InputManager.mouse.X, InputManager.mouse.Y);
        timer -= Globals.dt * SceneManager.SpeedScale;
        timer2 -= Globals.dt * SceneManager.SpeedScale;
        secretTimer -= Globals.dt * SceneManager.SpeedScale;
        
        if (InputManager.rmbClick && SuperCharge)
        {
            SuperCharge = false;
            secretTimer = SuperTime;
        }


        if (InputManager.lmbClick && timer <= 0)
        {
            if (secretTimer > 0)
            {
                timer = ShotSUPER + 0.001f;
                timer2 = ShotSUPER;
                ShootGun(10, 10);
            }
            else
            {
                timer = CD;
                timer2 = Shot;
                ShootGun(1, 1);
            }
        }   

        if(timer2 >= 0 && secretTimer >= 0)
        {
            _Anims.Update(new Vector2(2, 0));
        }
        else if(timer2 >= 0)
        {
            _Anims.Update(new Vector2(1, 0));
        }
        else
        {
            _Anims.ResetAnimation(new Vector2(2, 0));
            _Anims.ResetAnimation(new Vector2(1, 0));
        }

        foreach(Demon demon in Demons)
        {
            demon.Update();
        }

        Crosshair.spriteScale -= 0.025f;
        Crosshair.spriteScale = MathHelper.Clamp(Crosshair.spriteScale, 0.15f, 2f);

        Demons.RemoveAll(demon => 
            { 
                if (demon.deadtimer <= 0) {demonDeathCount++;}
                return demon.deadtimer <= 0;
            }
        );

        if (demonDeathCount >= 20 && Active)
        {
            gameManager.RequestSceneChange(true);
            Active = false;
        }

        if (Demons.Count >= 100 && Active || LevelTimer >= 10f && Active)
        {
            gameManager.RequestSceneChange(false);
            Active = false;
        }
    }
    
   public override void Draw()
    {
        Globals.sb.Draw(screenTexture, Globals.gd.Viewport.Bounds, Color.White);
        
        foreach(Demon demon in Demons)
        {
            demon.Draw();
        }

        Crosshair.Draw(Globals.sb);
        if(secretTimer > 0)
        {
            _Anims.Draw(GunPos, SpriteEffects.FlipHorizontally, MathF.Atan2(InputManager.mouse.Y - GunPos.Y, InputManager.mouse.X - GunPos.X), GetColorFromTime(total_et, 1f), gunOrigin);  

        }
        else
        {
            _Anims.Draw(GunPos, SpriteEffects.FlipHorizontally, MathF.Atan2(InputManager.mouse.Y - GunPos.Y, InputManager.mouse.X - GunPos.X), Color.White, gunOrigin);  

        }
        //I have made the bad circle logic intentional so it is funny now and no one can complain so don't.
    }
}