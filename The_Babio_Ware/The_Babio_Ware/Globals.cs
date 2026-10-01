using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace The_Babio_Ware;

public static class Globals
{
    public static int currentLives = 10;
    public static int currentMinigame = 0;
    public static int currentScore = 0;

    public static int gerberSnacks = 0;

    public static Random r = new Random();

    public static Dictionary<int, string> Minigames = new Dictionary<int, string>()
    {
        { 0, "Don't wake baby" },
        { 1, "Whack flying wasp ants" },
        { 2, "That one minigame" },
        { 3, "The_Babio_Ware.Minigames.Minigame4" },
        { 4, "The_Babio_Ware.Minigames.Minigame5" },
        { 5, "The_Babio_Ware.Minigames.Minigame6" },
        { 6, "The_Babio_Ware.Minigames.Minigame7" },
        { 7, "The_Babio_Ware.Minigames.Minigame8" },
        { 8, "The_Babio_Ware.Minigames.Minigame9" },
        { 9, "The_Babio_Ware.Minigames.Minigame10" }
    };

    public static void ResetGlobals()
    {
        currentLives = 10;
        currentMinigame = 0;
        currentScore = 0;
        gerberSnacks = 0;
    }
}