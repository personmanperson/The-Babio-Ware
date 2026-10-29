using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace The_Babio_Ware;

public class Fly
{
    public Vector2 _pos;
    public Vector2 _startPos;
    public float _speed;
    public Texture2D _tex;
    public Rectangle _rect;
    public Vector2 _origin;
    public float _spriteScale = 1f;
    public float byebyeTimer = 0.5f;
    public bool flydead = false;
    public bool flyLeft = false;

    public Vector2 ChooseRandomPosition()
    {
        Vector2 randomPos = new Vector2(Globals.Random.Next(0, Globals.VirtualWidth - _tex.Width), Globals.Random.Next(0, Globals.VirtualHeight - _tex.Height));
        return randomPos;
    }

    public Fly(Texture2D tex, float speed)
    {
        _tex = tex;
        Vector2 pos = ChooseRandomPosition();
        _pos = pos;
        _startPos = pos;
        _speed = speed;
        _origin = new Vector2(tex.Width / 2, tex.Height / 2);
        Vector2 rectOriginPos = new Vector2(pos.X - _origin.X, pos.Y - _origin.Y);
        _rect = new Rectangle((int)rectOriginPos.X, (int)rectOriginPos.Y, tex.Width, tex.Height);
    }

    public void KILLFLY(Texture2D deathTex)
    {
        _tex = deathTex;
        flydead = true;
    }

    public void Update()
    {
        if (flydead)
        {
            byebyeTimer -= Globals.dt * SceneManager.SpeedScale;
            _spriteScale -= Globals.dt * SceneManager.SpeedScale;
        }
        else
        {
            _pos.X += _speed * Globals.dt * SceneManager.SpeedScale;
            if (_pos.X > Globals.VirtualWidth + _tex.Width)
            {
                flyLeft = true;
            }
            Vector2 rectOriginPos = new Vector2(_pos.X - _origin.X, _pos.Y - _origin.Y);
            _rect = new Rectangle((int)rectOriginPos.X, (int)rectOriginPos.Y, _tex.Width, _tex.Height);
        }
    }

    public void Draw(SpriteBatch sb)
    {
        sb.Draw(_tex, _pos, null, Color.White, 0f, _origin, _spriteScale, SpriteEffects.None, 0f);
    }
}

public class Hand
{
    public Vector2 _pos;
    public Vector2 _startPos;
    public float _speed;
    public Texture2D _tex;
    public Rectangle _rect;
    public Vector2 _origin;
    public Color color = Color.White;
    public float DisapearTimer = 0.5f;
    public float _spriteScale = 0.5f;
    public bool handHit = false;
    public bool handLeft = false;

    public Vector2 ChooseRandomPosition()
    {
        Vector2 randomPos = new Vector2(Globals.Random.Next(0, Globals.VirtualWidth - _tex.Width), Globals.Random.Next(0, Globals.VirtualHeight - _tex.Height));
        return randomPos;
    }

    public Hand(Texture2D tex, float speed)
    {
        _tex = tex;
        Vector2 pos = ChooseRandomPosition();
        _pos = pos;
        _startPos = pos;
        _speed = speed;
        _origin = new Vector2(tex.Width / 2, tex.Height / 2);
        Vector2 rectOriginPos = new Vector2(pos.X - _origin.X, pos.Y - _origin.Y);
        _rect = new Rectangle((int)rectOriginPos.X, (int)rectOriginPos.Y, tex.Width, tex.Height);
    }

    public void KILLHAND(Texture2D deathTex)
    {
        //_tex = deathTex;
        color = Color.Red;
        handHit = true;
    }

    public void Update()
    {
        if(handHit == true || handLeft == true)
        {
            DisapearTimer -= Globals.dt * SceneManager.SpeedScale;
            _spriteScale -= Globals.dt * SceneManager.SpeedScale;
        }
        else
        {
            _pos.X += _speed * (float)Globals.dt * SceneManager.SpeedScale;
            if (_pos.X > Globals.VirtualWidth + _tex.Width)
            {
                handLeft = true;
            }
            Vector2 rectOriginPos = new Vector2(_pos.X - _origin.X, _pos.Y - _origin.Y);
            _rect = new Rectangle((int)rectOriginPos.X, (int)rectOriginPos.Y, _tex.Width, _tex.Height);
        }
    }

    public void Draw(SpriteBatch sb)
    {
        sb.Draw(_tex, _pos, null, color, Globals.SinWave * 0.01f, _origin, _spriteScale, SpriteEffects.None, 0f);
    }
}

public class Smack
{
    public Texture2D tex;
    public Vector2 pos;
    public Vector2 origin => new(tex.Width/2, tex.Height/2);
    public float lifeTimer;
    private float spriteScale = 1.7f;

    public Smack(Texture2D _tex, Vector2 _pos, float time)
    {
        tex = _tex;
        pos = _pos;
        lifeTimer = time;
    }
    
    public void Update()
    {
        spriteScale -= Globals.dt * 3 * SceneManager.SpeedScale;
        lifeTimer -= Globals.dt * SceneManager.SpeedScale;
    }

    public void Draw()
    {
        Globals.sb.Draw(tex, pos, null, Color.White, 0f, origin, spriteScale, SpriteEffects.None, 0f);
    }
}

public class FlySwat(GameManager gameManager) : Scene(gameManager)
{
    //Smack flies, 1-20 based on when the level shows up and difficulty
    public Texture2D swatterTextureUp;
    public Rectangle HITBOX => new(InputManager.mouse.X, InputManager.mouse.Y, 3, 3);
    public List<Smack> smacks = new();
    public bool swatterDown => InputManager.lmbClick;
    public Texture2D flygone;
    public Texture2D flyTexture;
    public List<Fly> flies = new();
    public List<Hand> hands = new();
    public Texture2D smackTex;
    public Texture2D handTex;

    public Texture2D screenTexture;

    public float levelTimer = 15f;
    public bool levelWon = false;
    public int Points = 0;
    public float startupTime = 0.5f;
    public float local_et = 0f;
    public int win;

    float min = 0.1f;
    float max = 0.5f;
    //in reference to time

    public override void Load()
    {
        screenTexture = Globals.cm.Load<Texture2D>("landfill");
        swatterTextureUp = Globals.cm.Load<Texture2D>("SwatterUp");
        flyTexture = Globals.cm.Load<Texture2D>("Fly");
        flygone = Globals.cm.Load<Texture2D>("deadfly");
        smackTex = Globals.cm.Load<Texture2D>("Smack1");
        handTex = Globals.cm.Load<Texture2D>("hand");
    }

    public override void Activate()
    {
        Points = 0;
        local_et = 0;
        levelWon = false;
        levelTimer = 15f;
        win = Globals.Random.Next(5, 12);
        Active = true;
    }

    public override void Update()
    {
        local_et += Globals.dt * SceneManager.SpeedScale;
        startupTime -= Globals.dt * SceneManager.SpeedScale;
        levelTimer -= Globals.dt * SceneManager.SpeedScale;
        if (local_et >= 0.75 && startupTime <= 0)
        {
            local_et = 0;
            int rand = Globals.Random.Next(1,4);
            for(int i = 0; i < rand; i++)
            {
                Fly newfly = new Fly(flyTexture, 50 * Globals.Random.Next(1,8));
                flies.Add(newfly);
            }

            int rand2 = Globals.Random.Next(1,4);
            for(int i = 0; i < rand2; i++)
            {
                Hand newHand = new Hand(handTex, 50 * Globals.Random.Next(1,8));
                hands.Add(newHand);
            }
        }
        
        bool hitFound = false;
        foreach (Fly fly in flies)
        {
            if(swatterDown)
            {
                if(!hitFound && !fly.flydead && HITBOX.Intersects(fly._rect))
                {
                    Vector2 CatchMouse = new Vector2(InputManager.MousePosition.X, InputManager.MousePosition.Y);
                    fly.KILLFLY(flygone);
                    Points++;
                    Smack newSmack = new(smackTex, CatchMouse, (Globals.Random.NextSingle() * (max - min) + min));
                    smacks.Add(newSmack);
                    hitFound = true;
                }
            }
            fly.Update();
        }

        foreach (Hand hand in hands)
        {
            if(swatterDown)
            {
                if(!hitFound && !hand.handHit && HITBOX.Intersects(hand._rect))
                {
                    Vector2 CatchMouse = new Vector2(InputManager.MousePosition.X, InputManager.MousePosition.Y);
                    hand.KILLHAND(flygone);
                    Points--;
                    Smack newSmack = new(smackTex, CatchMouse, (Globals.Random.NextSingle() * (max - min) + min));
                    smacks.Add(newSmack);
                    hitFound = true;
                }
            }
            hand.Update();
        }

        foreach (Smack smack in smacks)
        {
            smack.Update();
        }

        flies.RemoveAll(fly => fly.byebyeTimer <= 0);
        flies.RemoveAll(fly => 
            { 
                if (fly.flyLeft == true) {Points--;}
                return fly.flyLeft;
            }
        );
        smacks.RemoveAll(smack => smack.lifeTimer <= 0);
        hands.RemoveAll(hand => hand.DisapearTimer <= 0);
        hands.RemoveAll(hand => 
            { 
                if (hand.handLeft == true) {Points++;}
                return hand.handLeft;
            }
        );

        if (levelTimer <= 0 && Active || Points < -5 && Active)
        {
            gameManager.RequestSceneChange(false);
            Active = false;
        }

        if (Points > win && Active)
        {
            gameManager.RequestSceneChange(true);
            Active = false;
        }
    }

    public override void Deactivate()
    {
        flies.Clear();
        smacks.Clear();
        hands.Clear();
    }

    public override void Draw()
    {
        Globals.sb.Draw(screenTexture, Globals.gd.Viewport.Bounds, Color.White);
        foreach (Fly fly in flies)
        {
            fly.Draw(Globals.sb);
        }

        foreach (Hand hand in hands)
        {
            hand.Draw(Globals.sb);
        }
        foreach (Smack smack in smacks)
        {
            smack.Draw();
        }
        Globals.sb.Draw(swatterTextureUp, new(InputManager.MousePosition.X + 30, InputManager.MousePosition.Y + 100), null, Color.White, -0.3f, new(swatterTextureUp.Width/2, swatterTextureUp.Height/2), 0.1f, SpriteEffects.None, 0f);
    }
}