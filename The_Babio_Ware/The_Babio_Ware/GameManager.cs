using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;


namespace The_Babio_Ware;

public class GameManager
{
    private SceneManager _sceneManager;
    private Curtains _curtain;

    public GameManager()
    {
        _sceneManager = new SceneManager(this);
        _curtain = new Curtains();
    }

    public void RequestSceneChange()
    {
        _sceneManager.RequestChange();
    }

    public void Update(GameTime gameTime)
    {
        if (InputManager.KeyDown(Keys.F1))
        {
            _sceneManager.RequestChange();
            Globals.ForceScene = Scenes.TitleScreen;
        }


        if(_sceneManager.NEXTSCENE && !_sceneManager.TRANSITIONSCENE)
        {
            if(Globals.ForceScene.HasValue)
            {
                _sceneManager.SwitchScene(Globals.ForceScene.Value);
                Globals.ForceScene = null;
            }
            else
            {
                _sceneManager.SwitchScene(Scenes.PreschoolMath);
            }
        }

        _sceneManager.Update();
        _curtain.Update(_sceneManager);
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