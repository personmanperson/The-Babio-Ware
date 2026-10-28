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
        Globals.sb.DrawString(Game1.font, $"Lives: {Globals.currentLives}", Globals.ScreenCenter, Color.White, 0f, new Vector2(Game1.font.MeasureString($"Lives: {Globals.currentLives}").X/2f, 0), 2f, SpriteEffects.None, 0); 
        spriteBatch.End();
    }
}