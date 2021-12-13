using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObstaclesUtils
{
    static ObstaclesData obstaclesData;

    public static float RequiredSpeedMin(int code)
    {
        return obstaclesData.RequiredSpeedMin[code];
    }

    public static float RequiredSpeedMax(int code)
    {
        return obstaclesData.RequiredSpeedMax[code]; 
    }

    public static float RequiredEndurance(int code)
    {
        return obstaclesData.RequiredEndurance[code]; 
    }

    public static float RequiredTrust(int code)
    {
        return obstaclesData.RequiredTrust[code]; 
    }

    public static float MaxSkittishness(int code)
    {
        return obstaclesData.MaxSkittishness[code]; 
    }

    public static float RequiredObedience(int code)
    {
        return obstaclesData.RequiredObedience[code]; 
    }

    public static float Height(int code)
    {
        return obstaclesData.Height[code]; 
    }

    public static float Width(int code)
    {
        return obstaclesData.Width[code]; 
    }

    public static int Score(int code)
    {
        return obstaclesData.Score[code];
    }

    public static void Initialize()
    {
        obstaclesData = new ObstaclesData();
    }
}
