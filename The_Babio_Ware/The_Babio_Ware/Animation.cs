using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace The_Babio_Ware;

public class Animation
{
    public Texture2D _Texture;  
    private readonly List<Rectangle> _sourceRectangles = new();
    private readonly int _frameCount;   
    private int _currentFrame;
    private readonly float _frameTime;
    private float _frameTimeLeft;
    private Vector2 originSize;

    public float _textureScale { get; private set; }
    private bool _isPlaying = true;

    public Animation(Texture2D Texture, int start, int end, float frameTime, int totalColumns, int totalRows,  float texturescale = 1f)
    {
        _Texture = Texture;
        _frameTime = frameTime;
        _frameTimeLeft = _frameTime;

        _textureScale = texturescale;
        var frameWidth = _Texture.Width / totalColumns;
        var frameHeight = _Texture.Height / totalRows;

        for (int i = start; i < end; i++)
        {
            int column = i % totalColumns;
            int row = i / totalColumns;

            _sourceRectangles.Add(new Rectangle(column * frameWidth, row * frameHeight, frameWidth, frameHeight));
        }

        originSize = new(frameWidth/2, frameHeight/2);

        _frameCount = _sourceRectangles.Count;
    }

    public void Stop()
    {
        _isPlaying = false;
    }

    public void Start()
    {
        _isPlaying = true;
    }

    public void Reset()
    {
        _currentFrame = 0;
        _frameTimeLeft = _frameTime;
    }

    public void SetFrame(int frame)
    {
        _currentFrame = frame;
        _frameTimeLeft = _frameTime;
    }

    public void Update()
    {
        if (!_isPlaying) return;

        _frameTimeLeft -= Globals.dt * SceneManager.SpeedScale; // SCALE IT

        if (_frameTimeLeft <= 0)
        {
            _frameTimeLeft = _frameTime;
            _currentFrame = (_currentFrame + 1) % _frameCount;
        }
    }

    public void Draw(Vector2 pos, Color color, float rotation = 0f, SpriteEffects effects = SpriteEffects.None, float? scale = null)
    {
        scale = scale.HasValue? scale : _textureScale;
        Globals.sb.Draw(_Texture, pos, _sourceRectangles[_currentFrame], color, rotation, Vector2.Zero, scale.Value, effects, 1);
    }

    public void Draw(Vector2 pos, Color color, Vector2 origin, float rotation = 0f, SpriteEffects effects = SpriteEffects.None, float? scale = null)
    {
        scale = scale.HasValue? scale : _textureScale;
        Globals.sb.Draw(_Texture, pos, _sourceRectangles[_currentFrame], color, rotation, origin, scale.Value, effects, 1);
    }
}