using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.IO;

public class ObstaclesData
{
    static Dictionary<int,float> requiredSpeedMin = new Dictionary<int, float>();
    static Dictionary<int, float> requiredSpeedMax = new Dictionary<int, float>();
    static Dictionary<int, float> requiredEndurance = new Dictionary<int, float>();
    static Dictionary<int, float> requiredTrust = new Dictionary<int, float>();
    static Dictionary<int, float> maxSkittishness = new Dictionary<int, float>();
    static Dictionary<int, float> requiredObedience = new Dictionary<int, float>();
    static Dictionary<int, float> height = new Dictionary<int, float>();
    static Dictionary<int, float> width = new Dictionary<int, float>();
    static Dictionary<int, int> score = new Dictionary<int, int>();

    public Dictionary<int, float> RequiredSpeedMin
    {
        get { return requiredSpeedMin; }
    }

    public Dictionary<int, float> RequiredSpeedMax
    {
        get { return requiredSpeedMax; }
    }

    public Dictionary<int, float> RequiredEndurance
    {
        get { return requiredEndurance; }
    }

    public Dictionary<int, float> RequiredTrust
    {
        get { return requiredTrust; }
    }

    public Dictionary<int, float> MaxSkittishness
    {
        get { return maxSkittishness; }
    }

    public Dictionary<int, float> RequiredObedience
    {
        get { return requiredObedience; }
    }

    public Dictionary<int, float> Height
    {
        get { return height; }
    }

    public Dictionary<int, float> Width
    {
        get { return width; }
    }

    public Dictionary<int, int> Score
    {
        get { return score; }
    }

    void InitializeDictionaries()
    {
        requiredSpeedMin = new Dictionary<int, float>();
        requiredSpeedMax = new Dictionary<int, float>();
        requiredEndurance = new Dictionary<int, float>();
        requiredTrust = new Dictionary<int, float>();
        maxSkittishness = new Dictionary<int, float>();
        requiredObedience = new Dictionary<int, float>();
        height = new Dictionary<int, float>();
        width = new Dictionary<int, float>();
        score = new Dictionary<int, int>();
    }
    
    public ObstaclesData()
    {
        string configurationDataFileName = "ObstacleDataForTrek.csv";
        StreamReader fs;
        InitializeDictionaries();
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
                int code = (int)Convert.ToDouble(splitData[0]);
                score.Add(code,(int)Convert.ToDouble(splitData[1]));
                height.Add(code, (float)Convert.ToDouble(splitData[2], System.Globalization.CultureInfo.CreateSpecificCulture("en-US")));
                width.Add(code, (float)Convert.ToDouble(splitData[3], System.Globalization.CultureInfo.CreateSpecificCulture("en-US")));
                requiredSpeedMin.Add(code, (float)Convert.ToDouble(splitData[4], System.Globalization.CultureInfo.CreateSpecificCulture("en-US")));
                requiredSpeedMax.Add(code, (float)Convert.ToDouble(splitData[5], System.Globalization.CultureInfo.CreateSpecificCulture("en-US")));
                requiredEndurance.Add(code, (float)Convert.ToDouble(splitData[6], System.Globalization.CultureInfo.CreateSpecificCulture("en-US")));
                requiredTrust.Add(code, (float)Convert.ToDouble(splitData[7], System.Globalization.CultureInfo.CreateSpecificCulture("en-US")));
                maxSkittishness.Add(code, (float)Convert.ToDouble(splitData[8], System.Globalization.CultureInfo.CreateSpecificCulture("en-US")));
                requiredObedience.Add(code, (float)Convert.ToDouble(splitData[9], System.Globalization.CultureInfo.CreateSpecificCulture("en-US")));
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
}
