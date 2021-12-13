using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;

public class LastTrekResultsParser
{
    string lastTrekDataName = "LastTrekScoreData.csv";
    string lastDifficulty = "";
    string lastTries = "";
    string bestScore = "";
    string lastScore = "";
    string lastExpectedTime = "";
    bool bestFinished = false;

    public string LastTries
    {
        get { return lastTries; }
    }

    public string BestScore
    {
        get { return bestScore; }
    }

    public string LastScore
    {
        get { return lastScore; }
    }

    public string LastDifficulty
    {
        get { return lastDifficulty; }
    }

    public string LastExpectedTime
    {
        get { return lastExpectedTime; }
    }

    public bool BestFinished
    {
        get { return bestFinished; }
    }

    List<string> ReadOldTrekData()
    {
        StreamReader fs;
        List<string> data = new List<string>();
        const Int32 BufferSize = 128;
        using (var fileStream = File.OpenRead(lastTrekDataName))
        {
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                String line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    data.Add(line);
                }
            }
        }
        return data;
    }

    public LastTrekResultsParser(string path)
    {
        try
        {
            lastTrekDataName = Path.Combine(path, lastTrekDataName);
            List<string> data = ReadOldTrekData();
            bestFinished = (data[1] == "1");
            lastDifficulty = data[4];
            lastExpectedTime = data[7];
            lastTries = data[10];
            bestScore = data[14];
            lastScore = data[18];
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
}
