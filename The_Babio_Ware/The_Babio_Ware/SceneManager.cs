using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace The_Babio_Ware;

public class SceneManager
{
    public Scenes ActiveScene { get; private set; }
    private readonly Dictionary<Scenes, Scene> _scenes;
    private bool isSearchingForScene = false;
    public bool ISSEARCHINGFORSCENE {get {return isSearchingForScene;}}
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
            { Scenes.BabioPaint, new BabioPaint(gameManager) }, //Minigame in a minigame, title screen secret
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
    
    public Scenes GetUnbannedRandomScene()
    {
        isSearchingForScene = true;
        Scenes next;
        do {
            next = GetRandomScene();
        } while (BannedScenes.Contains(next));
        isSearchingForScene = false;
        return next;
    }

    public void RequestChange(Scenes? scene = null)
    {
        TransitionState = true;
        QueuedScene = scene.HasValue && !BannedScenes.Contains(scene.Value) ? scene.Value : GetUnbannedRandomScene();
        SceneQueued = true;
    }

    public void ForceChange(Scenes scene)
    {
        TransitionState = true;
        QueuedScene = scene;
        Console.WriteLine($"Force changing to scene: {scene}");
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