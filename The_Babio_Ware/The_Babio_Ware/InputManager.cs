using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace The_Babio_Ware;

public static class InputManager
{
    public static MouseState mouse;

    public static Point MousePosition => new Point(mouse.X, mouse.Y);

    public static ButtonState lmb => mouse.LeftButton;
    public static ButtonState rmb => mouse.RightButton;

    public static void Update()
    {
        mouse = Mouse.GetState();
        //Console.WriteLine(MousePosition.X);
    }
}