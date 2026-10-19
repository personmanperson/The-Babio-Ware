using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace The_Babio_Ware;

public struct FloatingPointRectangle
{
    public float X;
    public float Y;
    public float Width;
    public float Height;
    public float Bottom => Y + Height;
    public float Top => Y;
    public float Left => X;
    public float Right => X + Width;
    public Vector2 Center => new Vector2(X + Width / 2, Y + Height / 2);
    public Rectangle toRect => new((int)X, (int)Y, (int)Width, (int)Height);

    public FloatingPointRectangle(float x, float y, float width, float height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    public FloatingPointRectangle(Vector2 vector, float width, float height)
    {
        X = vector.X;
        Y = vector.Y;
        Width = width;
        Height = height;
    }

    public bool IntersectsXFloating(FloatingPointRectangle other)
    {
        return Left < other.Right &&
        Right > other.Left;
    }

    public bool IntersectsYFloating(FloatingPointRectangle other)
    {
        return Top < other.Bottom &&
        Bottom > other.Top;
    }

    public bool IntersectsX(Rectangle other)
    {
        return Left < other.Right &&
        Right > other.Left;
    }   

    public bool IntersectsY(Rectangle other)
    {
        return Top < other.Bottom &&
        Bottom > other.Top;
    }

    public bool IntersectsFloating(FloatingPointRectangle other)
    {
        return !(X + Width < other.X || X > other.X + other.Width || Y + Height < other.Y || Y > other.Y + other.Height);
    }

    public bool Intersects(Rectangle other)
    {
        return Left < other.Right &&
        Right > other.Left &&
        Top < other.Bottom &&
        Bottom > other.Top;
    }

    public Rectangle ToRectangle()
    {
        return new Rectangle((int)X, (int)Y, (int)Width, (int)Height);
    }

    public void Describe()
    {
        Console.WriteLine($"X: {X}, Y: {Y}, Width: {Width}, Height: {Height}");
    }
}