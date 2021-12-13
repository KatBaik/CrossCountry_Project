using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ObstacleListInterface : MonoBehaviour
{
    [SerializeField]
    Text obstacleListText;
    Dictionary<int, Obstacle> obstacleDict = new Dictionary<int, Obstacle>();
    List<string> lines = new List<string>();
    bool tmpWait = true;


    public void UpdateObstacleList(int id)
    {
        lines[id] = " <color=#bbff99>V</color> " + " " + " " + " " + lines[id];
        UpdateText();
    }

    void CreateObstacleDict()
    {
        foreach (GameObject obstacle in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            Obstacle script = obstacle.GetComponent<Obstacle>();
            obstacleDict.Add(script.Id, script);
        }
    }

    void CreateObstacleText()
    {
        lines.Add("Obstacles" + System.Environment.NewLine);
        for (int i = 1; i <= obstacleDict.Keys.Count; i++)
        {
            lines.Add(" <color=#" + ColorUtility.ToHtmlStringRGBA(obstacleDict[i].Colour) + ">#"
                + i.ToString() + ": " + obstacleDict[i].Score.ToString() + "</color>" + System.Environment.NewLine);
        }        
    }

    void UpdateText()
    {
        obstacleListText.text = "";
        for(int i=0; i < lines.Count; i++)
        {
            obstacleListText.text += lines[i];
        }
    }

    public void TmpFixedStart()
    {
        CreateObstacleDict();
        EventManager.AddObstacleSuccessListener(UpdateObstacleList);
        CreateObstacleText();
        UpdateText();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (tmpWait)
        {
            TmpFixedStart();
            tmpWait = false;
        }
    }
}
