using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace The_Babio_Ware;

public class SceneManager
{
    public static float SpeedScale = 1f;
    private int gamesWon = 0;
    public Scenes ActiveScene { get; private set; } = Scenes.TitleScreen;
    private readonly Dictionary<Scenes, Scene> _scenes;
    private bool isSearchingForScene = false;
    public bool ISSEARCHINGFORSCENE {get {return isSearchingForScene;}}
    private Scenes? QueuedScene {get; set;}
    private bool SceneQueued = false; 
    public bool NEXTSCENE {get {return SceneQueued;}}
    private bool TransitionState = false;
    public bool TRANSITIONSCENE {get {return TransitionState;}}
    public Dictionary<Scenes, float> _sceneWeights;


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
            { Scenes.Lose, new Lose(gameManager) },
            //{ Scenes.Minigame7, new Minigame7(gameManager) },
            //{ Scenes.Minigame8, new Minigame8(gameManager) },
            //{ Scenes.Minigame9, new Minigame9(gameManager) },
            //{ Scenes.Minigame10, new Minigame10(gameManager) }
        };

        _sceneWeights = new Dictionary<Scenes, float>
        {
            { Scenes.TitleScreen, -1f },
            { Scenes.FlySwat, 2f },
            { Scenes.STOPTHEFIRE, 1f },
            { Scenes.JudgeCriminal, 1f },
            { Scenes.StopDemonsFromHell, 1f },
            { Scenes.BabioPaint, -1f },
            { Scenes.PreschoolMath, 2f },
            { Scenes.Lose, -1f }
        };

        ActiveScene = Scenes.TitleScreen;
        _scenes[ActiveScene].Activate();
    }

    public HashSet<Scenes> BannedScenes = new HashSet<Scenes> { Scenes.TitleScreen, Scenes.BabioPaint }; // Add any scenes you want to ban here
    //some banned scenes are temporary bans, because they don't have a win condition, or are not fully implemented yet. Some are permanent bans, like the title screen and babio paint, minigames or not meant to be yet
    
    public Scenes GetRandomSceneWeighted()
    {
        var availableKeys = _sceneWeights.Where(evilkvp => !BannedScenes.Contains(evilkvp.Key)).Where(kvp => kvp.Key != ActiveScene || kvp.Value == 0).Select(kvp => kvp.Key).ToList();

        if (availableKeys.Count == 0)
        {
            Console.WriteLine("fatal fatal fatal error!! faiitl.");
            return ActiveScene; // No available scenes, return the current one
        }

        foreach(var scene in availableKeys)
        {
            _sceneWeights[scene] += 1f;
        }

        Scenes selectedScene = availableKeys.MaxBy(scene => _sceneWeights[scene]);
        return selectedScene;
    }

    public Scenes GetRandomScene()
    {
        Scenes[] allScenes = Enum.GetValues<Scenes>();
        allScenes = allScenes.Where(scene => !_sceneWeights[scene].Equals(0f)).ToArray();
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
            next = GetRandomSceneWeighted();
        } while (BannedScenes.Contains(next));
        isSearchingForScene = false;
        return next;
    } //eventually I'll stop it from doing the same scene twice, or even like 5 scenes back, always a more fresh scene

    public void RequestChange(GerberSnacks gerber, Scenes? scene = null)
    {
        TransitionState = true;
        QueuedScene = scene.HasValue && !BannedScenes.Contains(scene.Value) ? scene.Value : GetUnbannedRandomScene();
        if(gerber.Lives <= 0)
        {
            QueuedScene = Scenes.Lose;
        }
        SceneQueued = true;
        gamesWon++;
        if(gamesWon > 0 && gamesWon % 2 == 0)
        {
            SpeedScale += 0.001f;
        }
    }

    public void ForceChange(Scenes scene)
    {
        TransitionState = true;
        QueuedScene = scene;
        Console.WriteLine($"Force changing to scene: {scene}"); //these return to title or minigame (or boss?), we can safely reset speed scale !!!
        SpeedScale = 1f;
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
        //Console.WriteLine($"Switched to scene: {ActiveScene}"); //Should NOT trigger 35 times
        //Console.WriteLine($"{Globals.currentLives}"); //Should NOT trigger 35 times
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