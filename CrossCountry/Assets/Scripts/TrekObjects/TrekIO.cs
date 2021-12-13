using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Globalization;
using System.Text;

public class TrekIO
{
    string trekDataFileName;
    string obstacleDataFileName;
    string trekResultsFileName;
    bool bestFinished = false;

    public TrekIO(string path)
    {
        trekDataFileName = path + "/StreamingAssets/TrekData.csv";
        obstacleDataFileName = path + "/StreamingAssets/ObstacleDataForTrek.csv";
        trekResultsFileName = path + "/StreamingAssets/LastTrekScoreData.csv";
    }

    public Dictionary<int, System.Tuple<float, string>> WriteObstaclesDoc(int obstacleNum)
    {
        int overallScore = 0;
        string[] lineList = new string[obstacleNum + 1];
        string title = "#IdCode,score,height,width,requiredSpeedMin,requiredSpeedMax,requiredEndurance,requiredTrust,maxSkittishness,requiredObedience";
        Dictionary<int, System.Tuple<float, string>> heights = new Dictionary<int, System.Tuple<float, string>>();
        lineList[0] = title;
        for (int i = 1; i < obstacleNum + 1; i++)
        {
            float height = Random.Range(5f, 12f);
            float width = Random.Range(1f, 4f);
            float requiredSpeedMin = Random.Range(6f, 20f);
            float requiredSpeedMax = Random.Range(requiredSpeedMin + 15f, 50f);
            float requiredEndurance = Random.Range(20f, Mathf.Sqrt(requiredSpeedMax * requiredSpeedMin));
            float trust = Random.Range(0.05f, 0.5f);
            float skittishness = Random.Range(0.5f, 1f);
            float obedience = Random.Range(0.05f, 0.4f);
            int score = (int)Mathf.Ceil(height * width * requiredEndurance * (1f + trust) * (1f + obedience) * (2f - skittishness) / (requiredSpeedMax - requiredSpeedMin));
            string lineToSave = score.ToString() + ","
                + height.ToString("0.00", CultureInfo.InvariantCulture) + ","
                + width.ToString("0.00", CultureInfo.InvariantCulture) + ","
                + requiredSpeedMin.ToString("0.00", CultureInfo.InvariantCulture) + ","
                + requiredSpeedMax.ToString("0.00", CultureInfo.InvariantCulture) + ","
                + requiredEndurance.ToString("0.00", CultureInfo.InvariantCulture) + ","
                + trust.ToString("0.00", CultureInfo.InvariantCulture) + ","
                + skittishness.ToString("0.00", CultureInfo.InvariantCulture) + ","
                + obedience.ToString("0.00", CultureInfo.InvariantCulture);
            string line = i.ToString() + "," + lineToSave;//id
            overallScore += score;
            lineList[i] = line;
            heights.Add(i, new System.Tuple<float,string>(height, lineToSave));
        }
        File.WriteAllLines(obstacleDataFileName, lineList);
        StaticTrekData.TrekDifficulty = overallScore;
        return heights;
    }

    public void ReWriteObstaclesDoc(List<string> lines)
    {
        int overallScore = 0;
        string[] lineList = new string[lines.Count+1];
        string title = "#IdCode,score,height,width,requiredSpeedMin,requiredSpeedMax,requiredEndurance,requiredTrust,maxSkittishness,requiredObedience";
        lineList[0] = title;
        for (int i = 1; i < lines.Count+1; i++)
        {
            string[] data = lines[i-1].Split(',');
            string line = data[0] + "," //id
                + data[5] + "," //score
                + data[6] + "," //height
                + data[7] + "," //width
                + data[8] + "," //requiredSpeedMin
                + data[9] + "," //requiredSpeedMax
                + data[10] + "," //requiredEndurance
                + data[11] + "," //trust
                + data[12] + "," //skittishness
                + data[13]; //obedience
            overallScore += System.Convert.ToInt32(data[5]);
            lineList[i] = line;
        }
        StaticTrekData.TrekDifficulty = overallScore;
        File.WriteAllLines(obstacleDataFileName, lineList);
    }

    public List<string> ReadOldTrekData()
    {
        List<string> data = new List<string>();
        StreamReader fs;
        try
        {
            const System.Int32 BufferSize = 128;
            using (var fileStream = File.OpenRead(Path.Combine(Application.streamingAssetsPath, trekDataFileName)))
            {
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                {
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        if (line != "" && !line.StartsWith("#"))
                        {
                            data.Add(line);
                        }
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
        return data;
    }

    public void SaveTrek(List<string> lines)
    {
        if (lines.Count > 0)
        {
            File.WriteAllLines(trekDataFileName, lines.ToArray());
        }
    }

    System.Tuple<int, string> ParseOldTrekResults()
    {
        try
        {
            LastTrekResultsParser lastTrekResultParser = new LastTrekResultsParser(Application.streamingAssetsPath);
            bestFinished = lastTrekResultParser.BestFinished;
            return new System.Tuple<int, string>(System.Convert.ToInt32(lastTrekResultParser.LastTries), lastTrekResultParser.BestScore);
        }
        catch (System.Exception e)
        {
            return new System.Tuple<int, string>(0, "");
        }
    }

    string GetBestScore(string bestScore, string lastScore)
    {
        if (bestScore == "")
        {
            return lastScore;
        }
        if (bestFinished && !StaticTrekData.Finished)
        {
            return bestScore;
        }

        string[] bestScoreArr = bestScore.Split(',');
        string[] lastScoreArr = lastScore.Split(',');
        if (System.Convert.ToDouble(lastScoreArr[0], System.Globalization.CultureInfo.CreateSpecificCulture("en-US")) > 
            System.Convert.ToDouble(bestScoreArr[0], System.Globalization.CultureInfo.CreateSpecificCulture("en-US")))
        {
            return lastScore;
        }
        else if (System.Convert.ToDouble(lastScoreArr[0], System.Globalization.CultureInfo.CreateSpecificCulture("en-US")) <
            System.Convert.ToDouble(bestScoreArr[0], System.Globalization.CultureInfo.CreateSpecificCulture("en-US")))
        {
            return bestScore;
        }

        if (System.Convert.ToDouble(lastScoreArr[2], System.Globalization.CultureInfo.CreateSpecificCulture("en-US")) < 
            System.Convert.ToDouble(bestScoreArr[2], System.Globalization.CultureInfo.CreateSpecificCulture("en-US")))
        {
            return lastScore;
        }
        else if (System.Convert.ToDouble(lastScoreArr[2], System.Globalization.CultureInfo.CreateSpecificCulture("en-US")) >
            System.Convert.ToDouble(bestScoreArr[2], System.Globalization.CultureInfo.CreateSpecificCulture("en-US")))
        {
            return bestScore;
        }
        else
        {
            string[] bestTimeArr = bestScoreArr[1].Split(':');
            string[] lastTimeArr = lastScoreArr[1].Split(':');
            for(int i = 0; i<bestTimeArr.Length; i++)
            {
                if (System.Convert.ToDouble(lastTimeArr[i]) < System.Convert.ToDouble(bestTimeArr[i]))
                {
                    return lastScore;
                }
            }
        }
        return bestScore;
    }

    string GetFinished()
    {
        if (!bestFinished && !StaticTrekData.Finished)
        {
            return "0";
        }
        return "1";
    }
    
    public void SaveTrekResults()
    {
        System.Tuple<int, string> results;
        if (StaticTrekData.IsLoaded)
        {
            results = ParseOldTrekResults();
        }
        else
        {
            results = new System.Tuple<int, string>(0, "");
        }
        string[] lines = new string[19];
        lines[0] = "#BestFinished:";
        lines[1] = GetFinished();
        lines[2] = "";
        lines[3] = "#Difficulty:";
        lines[4] = StaticTrekData.TrekDifficulty.ToString();
        lines[5] = "";
        lines[6] = "#ExpectedTime";
        lines[7] = StaticTrekData.ExpectedTime;
        lines[8] = "";             
        lines[9] = "#Number of tries:";
        lines[10] = (results.Item1+1).ToString();
        lines[11] = "";
        string lastScore = StaticTrekData.ObstacleScore.ToString("0.00", CultureInfo.InvariantCulture) + "," +
            StaticTrekData.TimeScore + "," +
            StaticTrekData.PenaltyScore.ToString("0.00", CultureInfo.InvariantCulture);
        lines[12] = "#Best Score:";
        lines[13] = "#Obstacles,Time,Penalty:";
        lines[14] = GetBestScore(results.Item2, lastScore);
        lines[15] = "";
        lines[16] = "#Last Score:";
        lines[17] = "#Obstacles,Time,Penalty:";
        lines[18] = lastScore;
        File.WriteAllLines(trekResultsFileName, lines);
    }
}
