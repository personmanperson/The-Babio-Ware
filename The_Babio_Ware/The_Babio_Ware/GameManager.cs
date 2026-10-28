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
    public bool currentGameEnd = false;

    public GameManager()
    {
        _sceneManager = new SceneManager(this);
        _curtain = new Curtains();
    }

    public void RequestSceneChange(Scenes? scene = null)
    { _sceneManager.RequestChange(scene);}

    public void ForceSceneChange(Scenes scene)
    { _sceneManager.ForceChange(scene);}

    public void Update(GameTime gameTime)
    {
        if (InputManager.KeyPressed(Keys.F1))
        {
            ForceSceneChange(Scenes.TitleScreen);
        }

        if(_sceneManager.NEXTSCENE && !_sceneManager.TRANSITIONSCENE)
        {
            {
                _sceneManager.SwitchScene();
                currentGameEnd = false;
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