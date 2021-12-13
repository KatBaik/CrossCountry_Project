using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class FinishTrekGUI : MonoBehaviour
{
    [SerializeField]
    Text lastTrekData;
    [SerializeField]
    Text gameResultsData;

    string lastDifficulty = "";
    string lastTries = "";
    string lastBestScoreObstacles = "";
    string lastBestScoreTime = "";
    string lastExpectedTime = "";
    string lastBestScorePenalty = "";

    bool LoadLastTrekData()
    {
        try
        {
            LastTrekResultsParser lastTrekResultParser = new LastTrekResultsParser(Application.streamingAssetsPath);
            lastDifficulty = lastTrekResultParser.LastDifficulty;
            lastExpectedTime = lastTrekResultParser.LastExpectedTime;
            lastTries = lastTrekResultParser.LastTries;
            String[] splitData = lastTrekResultParser.LastScore.Split(',');
            lastBestScoreObstacles = splitData[0];
            lastBestScoreTime = splitData[1];
            lastBestScorePenalty = splitData[2];
            return true;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (StaticTrekData.Finished)
        {
            gameResultsData.text = "Success!!!";
        }
        else
        {
            gameResultsData.text = "Game Over";
        }
        if (LoadLastTrekData())
        {
            lastTrekData.text = "Difficulty: " + lastDifficulty + System.Environment.NewLine + System.Environment.NewLine +
                "Tries: " + lastTries + System.Environment.NewLine + System.Environment.NewLine +
                "Last Results: " + System.Environment.NewLine + "<color=red>Penalties: " + lastBestScorePenalty + "</color>" +
                " | Obstacles: " + lastBestScoreObstacles + " | Time Elapsed: " + lastBestScoreTime + " of " + lastExpectedTime;
        }
        else
        {
            lastTrekData.text = "";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
