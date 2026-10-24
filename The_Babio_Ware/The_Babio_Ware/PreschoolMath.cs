using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace The_Babio_Ware;

public class Paper
{
    private Texture2D paperSheet;
    private Texture2D paperBall;
    private Texture2D curTex;
    public Vector2 _pos;
    public float spriteScale = 1f;
    private Vector2 _origin;
    public FloatingPointRectangle rect => new(_pos.X - (_origin.X * spriteScale), _pos.Y - (_origin.Y * spriteScale), curTex.Width * spriteScale, curTex.Height * spriteScale);


    public Paper(Vector2 pos)
    {
        paperSheet = Globals.cm.Load<Texture2D>("unpapel");
        paperBall = Globals.cm.Load<Texture2D>("papil");
        curTex = paperSheet;
        _origin = _origin = new Vector2(curTex.Width / 2, curTex.Height / 2);
        _pos = pos;
    }

    public void Youcanreallynamefunctionsanythigyouwantitdoesntmatter()
    {
        if(Math.Abs((InputManager.MousePosition.X + 0.1)/(_pos.X + 0.1)) <= 50)
        {
            _pos.X = InputManager.MousePosition.X;
        }
    }

    public void Draw()
    {
        Console.WriteLine("drw");
        Globals.sb.Draw(curTex, _pos, null, Color.White, 0f, _origin, spriteScale, SpriteEffects.None, 0);
    }
}

public class PreschoolMath(GameManager gameManager) : Scene(gameManager)
{
    private Texture2D screenTexture;
    private Texture2D handTex;
    public int needed;
    public List<Paper> papers;

    private AnimationManager _Anims = new();

    public int findnumbersthatadduptotheinputinteger() //discontinued
    {
        return -999999999;
    }

    public override void Load()
    {
        screenTexture = Globals.cm.Load<Texture2D>("stock");
        handTex = Globals.cm.Load<Texture2D>("handstuff");
        _Anims.AddAnimation(new Vector2(1,0), new(handTex, 0, 2, 0.3f, 2, 1, 0.8f));
    }
    
    public override void Activate()
    {
        papers = new();
        needed = 0;
        int loops = Globals.Random.Next(2, 5);
        float spacing = (float)Globals.VirtualWidth / (loops + 1); //"linear algebra" grr
        for(int i = 0; i < loops; i++)
        {
            float xPos = spacing * (i + 1); //algebros 
            var paper = new Paper(new(xPos, Globals.VirtualHeight/2));
            papers.Add(paper);
            needed += Globals.Random.Next(15, 31);
        }
    }

    public override void Deactivate()
    {
        
    }

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
        foreach(Paper paper in papers)
        {
            paper.Draw();
        }
        _Anims.Draw(new(InputManager.MousePosition.X, Globals.ScreenCenter.Y + (Globals.VirtualHeight/8)), SpriteEffects.None, 0f, null, new(handTex.Width * 0.25f, handTex.Height * 0.5f));
    }
}