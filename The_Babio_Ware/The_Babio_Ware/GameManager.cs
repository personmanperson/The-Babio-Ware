using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace The_Babio_Ware;

public static class GameManager
{
    public static void Update(GameTime gameTime)
    {
        InputManager.Update();
    }
}