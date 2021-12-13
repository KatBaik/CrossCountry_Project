using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HorseConfigUtils
{
    static HorseConfigData horseConfigData;

    public static float MaxCanter
    {
        get { return horseConfigData.MaxCanter; }
    }

    public static float MinCanter
    {
        get { return horseConfigData.MinCanter; }
    }

    public static float MaxTrot
    {
        get { return horseConfigData.MaxTrot; }
    }

    public static float MinTrot
    {
        get { return horseConfigData.MinTrot; }
    }

    public static float MaxWalk
    {
        get { return horseConfigData.MaxWalk; }
    }

    public static float Endurance
    {
        get { return horseConfigData.Endurance; }
    }

    public static float PersonalStamina
    {
        get { return horseConfigData.PersonalStamina; }
    }

    public static float JumpingHeight
    {
        get { return horseConfigData.JumpingHeight; }
    }

    public static float JumpingWidth
    {
        get { return horseConfigData.JumpingWidth; }
    }

    public static float Skittishness
    {
        get { return horseConfigData.Skittishness; }
    }

    public static float Obedience
    {
        get { return horseConfigData.Obedience; }
    }

    public static float Trust
    {
        get { return horseConfigData.Trust; }
    }

    public static float SpeedQuant
    {
        get { return horseConfigData.SpeedQuant; }
    }

    public static float RotateDegreesPerSecond
    {
        get { return horseConfigData.RotateDegreesPerSecond; }
    }

    public static int LookAhead
    {
        get { return horseConfigData.LookAhead; }
    }

    public static float JumpHorizontalSpeed
    {
        get { return horseConfigData.JumpHorizontalSpeed; }
    }

    public static float EnduranceMinForJump
    {
        get { return horseConfigData.EnduranceMinForJump; }
    }

    public static float EnduranceMinForRunOut
    {
        get { return horseConfigData.EnduranceMinForRunOut; }
    }

    public static void Initialize(string configFileName)
    {
        if (horseConfigData == null)
        {
            horseConfigData = new HorseConfigData(configFileName);
        }
    }
}
