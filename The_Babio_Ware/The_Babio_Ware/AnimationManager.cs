using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace The_Babio_Ware;

public class AnimationManager
{
    private readonly Dictionary<object, Animation> _anims = new();
    private object _prevAnimKey;
    private Color _defaultColor = Color.White;

    public void AddAnimation(object key, Animation anim)
    {
        _anims.Add(key, anim);
        _prevAnimKey ??= key;
    }

    public void ResetAnimation(object key)
    {
        _anims[key].Reset();
    }

    public void Update(object key)
    {
        if (_anims.ContainsKey(key))
        {
            _anims[key].Start();
            _anims[key].Update();
            _prevAnimKey = key;
        }
        else
        {
            _anims[_prevAnimKey].Stop();
            _anims[_prevAnimKey].Reset();
        }
    }

    public void StopAnimation(object key)
    {
        if (_anims.ContainsKey(key))
        {
            _anims[key].Stop();
        }
    }

    public void Draw(Vector2 pos, SpriteEffects effects = SpriteEffects.None, float rotation = 0f, Color? color = null, Vector2? origin = null, float? scale = null)
    {
        if (!color.HasValue)
            color = _defaultColor;
        
        if (!origin.HasValue)
            _anims[_prevAnimKey].Draw(pos, color.Value, rotation, effects, scale);
        else    
            _anims[_prevAnimKey].Draw(pos, color.Value, origin.Value, rotation, effects, scale);
    }

}