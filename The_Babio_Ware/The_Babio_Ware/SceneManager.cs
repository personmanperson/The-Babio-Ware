using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace The_Babio_Ware;

public class SceneManager
{
    public Scenes ActiveScene { get; private set; }
    private readonly Dictionary<Scenes, Scene> _scenes;

    public SceneManager(GameManager gameManager)
    {
        _scenes = new Dictionary<Scenes, Scene>
        {
            { Scenes.TitleScreen, new TitleScreen(gameManager) },
            { Scenes.FlySwat, new FlySwat(gameManager) },
            { Scenes.STOPTHEFIRE, new STOPTHEFIRE(gameManager) },
            { Scenes.JudgeCriminal, new JudgeCriminal(gameManager) },
            { Scenes.StopDemonsFromHell, new StopDemonsFromHell(gameManager) },
            { Scenes.BabioPaint, new BabioPaint(gameManager) }, //Minigame in a minigame, secret title screen
            { Scenes.PreschoolMath, new PreschoolMath(gameManager) },
            //{ Scenes.Minigame7, new Minigame7(gameManager) },
            //{ Scenes.Minigame8, new Minigame8(gameManager) },
            //{ Scenes.Minigame9, new Minigame9(gameManager) },
            //{ Scenes.Minigame10, new Minigame10(gameManager) }
        };

        ActiveScene = Scenes.TitleScreen;
        _scenes[ActiveScene].Activate();
    }

    
    public static void RandomScene()
    {
        
    }

    public void SwitchScene(Scenes newScene)
    {
        _scenes[ActiveScene].Deactivate();
        ActiveScene = newScene;
        _scenes[ActiveScene].Activate();
    }

    public void Update()
    {
        _scenes[ActiveScene].Update();
    }

    public RenderTarget2D GetFrame()
    {
        return _scenes[ActiveScene].GetFrame();
    }

    public void Draw()
    {
        _scenes[ActiveScene].Draw();
    }
}