using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace The_Babio_Ware;

public class Hack(GameManager gameManager) : Scene(gameManager)
{
    public override void Load()
    {
        
    }

    public override void Activate()
    {
        Active = true;
    }

    public override void Deactivate()
    {
        Active = false;
    }

    public override void Update()
    {
        
    }

    public override void Draw()
    {

    }
}