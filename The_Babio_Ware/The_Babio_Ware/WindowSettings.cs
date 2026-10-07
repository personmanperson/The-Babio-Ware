using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace The_Babio_Ware;


public struct WindowSettings
{
    public int Width;
    public int Height;
    public bool Fullscreen;

    public WindowSettings(int width, int height, bool fullscreen)
    {
        Width = width;
        Height = height;
        Fullscreen = fullscreen;
    }

    public void Init(int width, int height, bool fullscreen)
    {
        Globals.gdm.PreferredBackBufferWidth = width;
        Globals.gdm.PreferredBackBufferHeight = height;
        Globals.gdm.IsFullScreen = fullscreen;
        Globals.gdm.ApplyChanges();
    }

    public void ToggleFullscreen(GraphicsDeviceManager graphics)
    {
        if (Globals.gdm.IsFullScreen)
        {
            Globals.gdm.IsFullScreen = false;
            Globals.gdm.PreferredBackBufferWidth = Width;
            Globals.gdm.PreferredBackBufferHeight = Height;
            Globals.gdm.ApplyChanges();
        }
        else
        {
            Globals.gdm.IsFullScreen = true;
            Width = Globals.gdm.PreferredBackBufferWidth;
            Height = Globals.gdm.PreferredBackBufferHeight;
            Globals.gdm.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            Globals.gdm.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            Globals.gdm.ApplyChanges();
        }
    }

    public void SetResolution(int width, int height)
    {
        Width = width;
        Height = height;
        Globals.gdm.PreferredBackBufferWidth = Width;
        Globals.gdm.PreferredBackBufferHeight = Height;
        Globals.gdm.ApplyChanges();
    }
}