using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace The_Babio_Ware;

public static class InputManager
{
    public static MouseState lastMouse;
    public static MouseState mouse;

    public static Vector2 MouseDelta => new Vector2(mouse.X - lastMouse.X, mouse.Y - lastMouse.Y);
    public static Point MousePosition => new Point(mouse.X, mouse.Y);
    public static FloatingPointRectangle MouseHitbox => new(mouse.X, mouse.Y, 10, 10);

    public static ButtonState lmb => mouse.LeftButton;
    public static ButtonState rmb => mouse.RightButton;

    public static KeyboardState keyboard => Keyboard.GetState();
    public static Keys[] pressedKeys => keyboard.GetPressedKeys();
    public static bool KeyDown(Keys key) => keyboard.IsKeyDown(key);
    public static bool leftclick;

    public static bool lmbClick => (lastMouse.LeftButton == ButtonState.Released) && (mouse.LeftButton == ButtonState.Pressed);
    public static bool rmbClick => (lastMouse.RightButton == ButtonState.Released) && (mouse.RightButton == ButtonState.Pressed);

    public static void Update()
    {
        lastMouse = mouse;
        mouse = Mouse.GetState();
    }
}