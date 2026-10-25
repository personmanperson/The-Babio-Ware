using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace The_Babio_Ware;

public class SceneManager
{
    public Scenes ActiveScene { get; private set; }
    private readonly Dictionary<Scenes, Scene> _scenes;
    private Scenes? QueuedScene {get; set;}
    private bool SceneQueued = false; 
    public bool NEXTSCENE {get {return SceneQueued;}}
    private bool TransitionState = false;
    public bool TRANSITIONSCENE {get {return TransitionState;}}


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

    public HashSet<Scenes> BannedScenes = new HashSet<Scenes> { Scenes.TitleScreen, Scenes.BabioPaint };

    public Scenes GetRandomScene()
    {
        Scenes[] allScenes = Enum.GetValues<Scenes>();
        return allScenes[Random.Shared.Next(allScenes.Length)];
    }

    public void EndTransition()
    {
        TransitionState = false;
    }
    
    public void RandomScene()
    {
        QueuedScene = GetRandomScene();
    }

    public void RequestChange(Scenes? scene = null)
    {
        QueuedScene = !BannedScenes.Contains(scene.Value) ? scene.Value : GetRandomScene();
        TransitionState = true;
        SceneQueued = true;
    }


    public void SwitchScene(Scenes? newScene = null)
    {
        _scenes[ActiveScene].Deactivate();
        Scenes TargetScene = newScene ?? QueuedScene.Value;
        ActiveScene = TargetScene;
        _scenes[ActiveScene].Activate();
        SceneQueued = false;
        QueuedScene = null;
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