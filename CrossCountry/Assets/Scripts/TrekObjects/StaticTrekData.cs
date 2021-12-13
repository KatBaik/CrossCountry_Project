using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticTrekData
{
    static Vector3 start = new Vector3(0, 0, 0);
    static Vector3 horseStartDirection = new Vector3(0, 0, 0);
    static int nextObstacle = 1;
    static int trekDifficulty = 0;
    static string expectedTime = "00:00:00:00";
    static string timeScore = "00:00:00:00";
    static float penaltyScore = 0;
    static float obstacleScore = 0;
    static float horseHeight = 12.62f; //TODO change
    static bool addedAdListener = false;
    static bool loadedTrek = false;
    static bool finished = false;
    static bool stopped = false;
    static string horseConfigFile = "";

    public static Vector3 Start
    {
        get { return start; }
        set { start = value; }
    }

    public static Vector3 DirectHorseTo
    {
        get { return horseStartDirection; }
        set { horseStartDirection = value; }
    }

    public static float HorseHeight
    {
        get { return horseHeight; }
    }

    public static bool Finished
    {
        get { return finished; }
        set { finished = value; }
    }

    public static bool Stopped
    {
        get { return stopped; }
        set { stopped = value; }
    }

    public static int NextObstacleId
    {
        get { return nextObstacle; }
        set { nextObstacle = value; }
    }

    public static int TrekDifficulty
    {
        get { return trekDifficulty; }
        set { trekDifficulty = value; }
    }

    public static string ExpectedTime
    {
        get { return expectedTime; }
        set { expectedTime = value; }
    }

    public static string TimeScore
    {
        get { return timeScore; } 
        set { timeScore = value; }
    }

    public static float PenaltyScore
    {
        get { return penaltyScore; }
        set { penaltyScore = value; }
    }

    public static float ObstacleScore
    {
        get { return obstacleScore; }
        set { obstacleScore = value; }
    }

    public static bool IsLoaded
    {
        get { return loadedTrek; }
        set { loadedTrek = value; }
    }

    public static string HorseConfigFile
    {
        get { return horseConfigFile; }
        set { horseConfigFile = value; }
    }

    public static void Initialize(string configFileName)
    {
    }
}
