using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace The_Babio_Ware;

public class GerberSnack
{
    private Texture2D goodGerber;
    private Texture2D badGerber;
    public bool isGood = true;
    private float positionX;
    private float positionY = 0;
    private Vector2 Position => new Vector2(positionX, positionY + goodGerber.Height * scale / 2f);
    private float rotation = 0f;
    private float scale = 0.09f;

    public GerberSnack(Texture2D goodGerber, Texture2D badGerber, Vector2 position)
    {
        this.goodGerber = goodGerber;
        this.badGerber = badGerber;
        positionX = position.X;
    }

    public void Update()
    {
        scale = float.Lerp(scale, Globals.SinWave * 0.01f + 0.08f, 0.01f);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (isGood)
        {
            spriteBatch.Draw(goodGerber, Position, null, Color.White, rotation, new Vector2(goodGerber.Width / 2, goodGerber.Height / 2), scale, SpriteEffects.None, 0f);
        }
        else
        {
            spriteBatch.Draw(badGerber, Position, null, Color.White, 0f, new Vector2(badGerber.Width / 2, badGerber.Height / 2), scale, SpriteEffects.None, 0f);
        }
    }
}

public class GerberSnacks //health
{
    private Dictionary<int, GerberSnack> snacks = new Dictionary<int, GerberSnack>();
    public Vector2 middlePos = new(Globals.VirtualWidth / 2.42f, 0);
    public Texture2D good = Globals.cm.Load<Texture2D>("gs1");
    public Texture2D good2 = Globals.cm.Load<Texture2D>("gs2");
    public Texture2D bad = Globals.cm.Load<Texture2D>("gs3");
    public int Lives = 5;
    
    public void ResetGerberSnacks()
    {
        snacks.Clear();

        for(int i = 0; i < 5; i++)
        {   
            Texture2D goodGerber = (i % 2 == 0) ? good : good2;
            snacks.Add(i, new GerberSnack(goodGerber, bad, new Vector2(middlePos.X + i * 100, middlePos.Y)));
        }
        Lives = 5;
    }

    public void SetRightmostGerberSnackToExplosion()
    {
        if (snacks.Count > 0)
        {
            int rightmostKey = 0;
            foreach (var key in snacks.Keys)
            {
                if (key > rightmostKey && snacks[key].isGood)
                {
                    rightmostKey = key;
                }
            }
            snacks[rightmostKey].isGood = false;
        }

        Lives--;
    }


    public void Update()
    {
        foreach (var snack in snacks)
        {
            snack.Value.Update();
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var snack in snacks)
        {
            snack.Value.Draw(spriteBatch);
        }
    }
}
