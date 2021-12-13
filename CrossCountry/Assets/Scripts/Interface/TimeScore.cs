using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using static StaticTrekData;
using static Utils;

public class TimeScore : MonoBehaviour
{
    [SerializeField]
    Text time;
    float score = 0;
    TrekTiming trekTiming;

    public float Score
    {
        get { return score; }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        trekTiming = ((GameObject.FindGameObjectsWithTag("Trek"))[0]).GetComponent<TrekTiming>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!trekTiming.IsSecondLap)
        {
            time.color = Color.black;
            time.text = "Time left: " + Utils.GetFormattedTime((double)trekTiming.TimeLeft);
            StaticTrekData.TimeScore = Utils.GetFormattedTime((double)trekTiming.ElapsedTime - score);
        }
        else
        {
            time.color = Color.red;
            time.text = "Extra time: " + Utils.GetFormattedTime((double)trekTiming.ElapsedTime);
            StaticTrekData.TimeScore = Utils.GetFormattedTime((double)trekTiming.LimitTime+trekTiming.ElapsedTime - score);
        }        
    }
}
