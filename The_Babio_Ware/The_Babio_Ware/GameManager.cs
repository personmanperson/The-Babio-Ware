using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;


namespace The_Babio_Ware;

public class GameManager
{
    private readonly SceneManager _sceneManager;
    private readonly Curtains _curtain;

    public GameManager()
    {
        _sceneManager = new SceneManager(this);
        _curtain = new Curtains();
    }

    public void Update(GameTime gameTime)
    {
        if (InputManager.KeyDown(Keys.F1))
        {
            _sceneManager.SwitchScene(Scenes.TitleScreen);
            Globals.SwitchScene = false;
        }

        if (InputManager.KeyDown(Keys.F2))
        {
            _sceneManager.SwitchScene(Scenes.FlySwat);
            Globals.SwitchScene = false;
        }
        
        if (InputManager.KeyDown(Keys.F3))
        {
            _sceneManager.SwitchScene(Scenes.STOPTHEFIRE);
            Globals.SwitchScene = false;
        }

        if (InputManager.KeyDown(Keys.F4))
        {
            _sceneManager.SwitchScene(Scenes.JudgeCriminal);
            Globals.SwitchScene = false;
        }


        if(Globals.SwitchScene && !Globals.TransitionState)
        {
            if(Globals.ForceScene.HasValue)
            {
                _sceneManager.SwitchScene(Globals.ForceScene.Value);
                Globals.ForceScene = null;
            }
            else
            {
                _sceneManager.SwitchScene(Scenes.StopDemonsFromHell);
            }
            
            Globals.SwitchScene = false;
        }

        _sceneManager.Update();
        _curtain.Update();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        var frame = _sceneManager.GetFrame();

        spriteBatch.Begin();
        Globals.sb.Draw(frame, new Rectangle(0, 0, Globals.VirtualWidth, Globals.VirtualHeight), Color.White);
        _curtain.Draw();
        spriteBatch.End();
    }
}