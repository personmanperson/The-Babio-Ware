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
    private GerberSnacks _gerberSnacks;

    public GameManager()
    {
        _sceneManager = new SceneManager(this);
        _curtain = new Curtains();
        _gerberSnacks = new GerberSnacks();
        _gerberSnacks.ResetGerberSnacks();
    }

    public void RequestSceneChange(bool won, Scenes? scene = null)
    {
        if(!won)
        {
            _gerberSnacks.SetRightmostGerberSnackToExplosion();
        }
         _sceneManager.RequestChange(_gerberSnacks, scene);
    }

    public void ForceSceneChange(Scenes scene)
    { _sceneManager.ForceChange(scene);}

    public void Update(GameTime gameTime)
    {
        if (InputManager.KeyPressed(Keys.F1))
        {
            _gerberSnacks.ResetGerberSnacks();
            ForceSceneChange(Scenes.TitleScreen);
        }

        if(_sceneManager.NEXTSCENE && !_sceneManager.TRANSITIONSCENE)
        {
            {
                _sceneManager.SwitchScene();
            }
        }

        _sceneManager.Update();
        _gerberSnacks.Update();
        _curtain.Update(_sceneManager);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        var frame = _sceneManager.GetFrame();

        spriteBatch.Begin();
        Globals.sb.Draw(frame, new Rectangle(0, 0, Globals.VirtualWidth, Globals.VirtualHeight), Color.White);      
        if (_sceneManager.ActiveScene != Scenes.TitleScreen && _sceneManager.ActiveScene != Scenes.BabioPaint)
        {
            _gerberSnacks.Draw(spriteBatch);
        }
        _curtain.Draw();
        spriteBatch.End();
    }
}