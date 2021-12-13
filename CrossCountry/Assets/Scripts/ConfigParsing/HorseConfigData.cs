using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.IO;

public class HorseConfigData
{
    // configuration data
    static float maxCanter;
    static float minCanter;
    static float maxTrot;
    static float minTrot;
    static float maxWalk;
    static float endurance;
    static float personalStamina;
    static float jumpingHeight;
    static float jumpingWidth;
    static float skittishness;
    static float obedience;
    static float trust;
    static float speedQuant;
    static float rotateDegreesPerSecond;
    static int lookAhead;
    static float jumpHorizontalSpeed;
    static float enduranceMinForRunOut;
    static float enduranceMinForJump;


    public float MaxCanter
    {
        get { return maxCanter; }
    }

    public float MinCanter
    {
        get { return minCanter; }
    }

    public float MaxTrot
    {
        get { return maxTrot; }
    }

    public float MinTrot
    {
        get { return minTrot; }
    }

    public float MaxWalk
    {
        get { return maxWalk; }
    }

    public float Endurance
    {
        get { return endurance; }
    }

    public float PersonalStamina
    {
        get { return personalStamina; }
    }

    public float JumpingHeight
    {
        get { return jumpingHeight; }
    }

    public float JumpingWidth
    {
        get { return jumpingWidth; }
    }

    public float Skittishness
    {
        get { return skittishness; }
    }

    public float Obedience
    {
        get { return obedience; }
    }

    public float Trust
    {
        get { return trust; }
    }

    public float SpeedQuant
    {
        get { return speedQuant; }
    }

    public float RotateDegreesPerSecond
    {
        get { return rotateDegreesPerSecond; }
    }

    public int LookAhead
    {
        get { return lookAhead; }
    }

    public float JumpHorizontalSpeed
    {
        get { return jumpHorizontalSpeed; }
    }

    public float EnduranceMinForJump
    {
        get { return enduranceMinForJump; }
    }

    public float EnduranceMinForRunOut
    {
        get { return enduranceMinForRunOut; }
    }


    public HorseConfigData(string configurationDataFileName)
    {
        StreamReader fs;
        try
        {
            const Int32 BufferSize = 128;
            using (var fileStream = File.OpenRead(Path.Combine(Application.streamingAssetsPath, configurationDataFileName)))
            {
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                {
                    String line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        ParseData(line);
                    }
                }
            }
        }
        catch (Exception e)
        {
        }
    }

    void ParseData(string data)
    {
        try
        {
            if (!data.StartsWith("#"))
            {
                String[] splitData = data.Split(',');
                maxCanter = (float)Convert.ToDouble(splitData[0], System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
                minCanter = (float)Convert.ToDouble(splitData[1], System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
                maxTrot = (float)Convert.ToDouble(splitData[2], System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
                minTrot = (float)Convert.ToDouble(splitData[3], System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
                maxWalk = (float)Convert.ToDouble(splitData[4], System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
                endurance = (float)Convert.ToDouble(splitData[5], System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
                personalStamina = (float)Convert.ToDouble(splitData[6], System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
                jumpingHeight = (float)Convert.ToDouble(splitData[7], System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
                jumpingWidth = (float)Convert.ToDouble(splitData[8], System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
                skittishness = (float)Convert.ToDouble(splitData[9], System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
                obedience = (float)Convert.ToDouble(splitData[10], System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
                trust = (float)Convert.ToDouble(splitData[11], System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
                speedQuant = (float)Convert.ToDouble(splitData[12], System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
                rotateDegreesPerSecond = (float)Convert.ToDouble(splitData[13], System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
                lookAhead = (int)Convert.ToDouble(splitData[14]);
                jumpHorizontalSpeed = (float)Convert.ToDouble(splitData[15], System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
                enduranceMinForJump = (float)Convert.ToDouble(splitData[16], System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
                enduranceMinForRunOut = (float)Convert.ToDouble(splitData[17], System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
            }
        }
        catch (Exception e)
        {
        }
    }
}
