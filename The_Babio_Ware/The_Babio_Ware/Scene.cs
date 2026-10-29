using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace The_Babio_Ware;

public abstract class Scene
{
    public readonly RenderTarget2D RenderTarget;
    public GameManager gameManager;
    public bool Active = true;

    public Scene(GameManager gameManager)
    {
        this.gameManager = gameManager;
        RenderTarget = Globals.GetNewRenderTarget();
        Load();
    }

    public abstract void Load();
    public abstract void Draw();
    public abstract void Update();
    public abstract void Activate();
    public abstract void Deactivate();

    public virtual RenderTarget2D GetFrame()
    {
        Globals.gd.SetRenderTarget(RenderTarget);
        Globals.gd.Clear(Color.Black);

        Globals.sb.Begin();
        Draw();
        Globals.sb.End();

        Globals.gd.SetRenderTarget(null);
        return RenderTarget;
    }

}