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
    public int value;
    public bool toDelete;

    public Paper(Vector2 pos, int value = 0)
    {
        paperSheet = Globals.cm.Load<Texture2D>("unpapel");
        paperBall = Globals.cm.Load<Texture2D>("papil");
        curTex = paperSheet;
        _origin = new Vector2(curTex.Width / 2, curTex.Height / 2);
        _pos = pos;
        this.value = value;
    }

    public bool Update()
    {
        if(InputManager.MouseHitbox.IntersectsFloating(rect) && InputManager.lmbDown)
        {
            _pos.X = InputManager.MousePosition.X;
            curTex = paperBall;
            _origin = new Vector2(curTex.Width / 2, curTex.Height / 2);
            spriteScale = 0.3f;
            return true;
        }
        curTex = paperSheet;
        _origin = new Vector2(curTex.Width / 2, curTex.Height / 2);
        spriteScale = 1f;
        return false;
    }

    public void Draw()
    {
        Globals.sb.Draw(curTex, _pos, null, Color.White, 0f, _origin, spriteScale, SpriteEffects.None, 0);
        Globals.sb.DrawString(Game1.font, value.ToString(), rect.Center, Color.Black);
    }
}

public class PreschoolMath(GameManager gameManager) : Scene(gameManager)
{
    private Texture2D screenTexture;
    private Texture2D handTex;
    public int needed;
    public List<Paper> papers;
    public Paper? grabbed = null;

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
            int next = Globals.Random.Next(1, 31);
            float xPos = spacing * (i + 1); //algebros 
            var paper = new Paper(new(xPos, Globals.VirtualHeight/2), next);
            papers.Add(paper);
            needed += next;
        }
    }

    public override void Deactivate()
    {
        papers.Clear();
        grabbed = null;   
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

        foreach(Paper paper in papers)
        {
           bool clicked = paper.Update();
           if(clicked)
           {
               grabbed = paper;
           }
        }

        if(grabbed != null)
        {
            foreach(var otherPaper in papers)
            {
                if(otherPaper != grabbed && grabbed.rect.IntersectsFloating(otherPaper.rect) && !otherPaper.toDelete)
                {
                   grabbed.value += otherPaper.value;
                   Console.WriteLine("Merging papers: " + grabbed.value + " and " + otherPaper.value);
                   otherPaper.value = 0; //it's useless but makes me feel safer
                   otherPaper.toDelete = true;
                }
            }

            grabbed = null;
        }

        papers.RemoveAll(paper => paper.toDelete);
        Console.WriteLine(papers.Count);
        if(papers.Count == 1)
        {
            if(papers[0].value == needed)
            {
                gameManager.RequestSceneChange();
            }
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