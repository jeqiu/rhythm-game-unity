using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SharedData
{
    public static int score { get; set; }
    public static int maxScore { get; set; }

    public static float sfxVolume { get; set; } = 0.5f;
    public static float musicVolume { get; set; } = 0.5f;

    public static string trackName { get; set; }

    public static bool debugMode { get; set; }
}
