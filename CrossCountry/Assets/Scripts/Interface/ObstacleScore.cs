using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using static StaticTrekData;

public class ObstacleScore : MonoBehaviour
{
    [SerializeField]
    Text obstacles;
    int score;
    string beginning = "Obstacles: ";

    public int Score
    {
        get { return score; }
    }
    
    public void ScoreObstacleSuccess(int id)
    {
        foreach (GameObject obstacle in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            Obstacle script = obstacle.GetComponent<Obstacle>();
            if (id == script.Id) 
            {
                score += script.Score;
                break;
            }
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        EventManager.AddObstacleSuccessListener(ScoreObstacleSuccess);        
    }

    // Update is called once per frame
    void Update()
    {
        obstacles.text = beginning + score.ToString();
        StaticTrekData.ObstacleScore = score;
    }
}
