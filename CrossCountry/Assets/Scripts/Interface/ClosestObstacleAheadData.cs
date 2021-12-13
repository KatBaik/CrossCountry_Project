using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ClosestObstacleAheadData : MonoBehaviour
{
    [SerializeField]
    Text title;
    [SerializeField]
    Text idCode;
    [SerializeField]
    Text size;
    [SerializeField]
    Text score;
    [SerializeField]
    Text speed;
    [SerializeField]
    Text endurance;
    [SerializeField]
    Text skittishness;
    [SerializeField]
    Text trust;
    [SerializeField]
    Text obedience;
    Dictionary<int, Obstacle> obstacleDict = new Dictionary<int, Obstacle>();
    
    public void NewUpdateClosestObstacleData(int id)
    {
        if (id > 0)
        {
            title.text = "Obstacle: ";
            title.color = obstacleDict[id].Colour;
            idCode.text = "#" + id.ToString();
            idCode.color = obstacleDict[id].Colour;
            score.text = "Score: " + obstacleDict[id].Score.ToString();
            score.color = obstacleDict[id].Colour;
            size.text = "Size: " + obstacleDict[id].Width.ToString()+"X"+obstacleDict[id].Height.ToString();
            size.color = obstacleDict[id].Colour;
            speed.text = obstacleDict[id].RequiredSpeedMin.ToString() + " < speed < " + obstacleDict[id].RequiredSpeedMax.ToString();
            speed.color = obstacleDict[id].Colour;
            endurance.text = "Endurance > "+ obstacleDict[id].RequiredEndurance.ToString();
            endurance.color = obstacleDict[id].Colour;
            skittishness.text = "Skittishness < "+ (obstacleDict[id].MaxSkittishness*100).ToString();
            skittishness.color = obstacleDict[id].Colour;
            obedience.text = "Obedience > "+ (obstacleDict[id].RequiredObedience * 100).ToString();
            obedience.color = obstacleDict[id].Colour;
            trust.text = "Trust > "+ (obstacleDict[id].RequiredTrust * 100).ToString();
            trust.color = obstacleDict[id].Colour;
        }        
    }

    public void EmptyClosestObstacleData()
    {
        title.text = "Obstacle: None";
        title.color = Color.white;
        idCode.text = "";
        score.text = "";
        size.text = "";
        speed.text = "";
        endurance.text = "";
        skittishness.text = "";
        obedience.text = "";
        trust.text = "";
    }

    void CreateObstacleDict()
    {
        foreach (GameObject obstacle in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            Obstacle script = obstacle.GetComponent<Obstacle>();
            obstacleDict.Add(script.Id, script);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        CreateObstacleDict();
        EventManager.AddClosestObstacleFoundListener(NewUpdateClosestObstacleData);
        EventManager.AddNoObstacleDetectedListener(EmptyClosestObstacleData);
        EmptyClosestObstacleData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
