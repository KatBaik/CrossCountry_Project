using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static EventManager;

public class TrekTiming : MonoBehaviour
{
    bool firstLap = true;
    Timer timer;
    float limitTime = 100f;
    TimeRanOutEvent timeRanOutEvent;

    public float LimitTime
    {
        get { return limitTime; }
        set { limitTime = value; }
    }

    public float ElapsedTime
    {
        get { return timer.ElapsedSeconds; }
    }

    public float TimeLeft
    {
        get { return limitTime - timer.ElapsedSeconds; }
    }

    public bool IsSecondLap
    {
        get { return !firstLap; }
    }

    public void AddTimeRanOutEventListener(UnityAction listener)
    {
        timeRanOutEvent.AddListener(listener);
    }

    public void Run()
    {
        if (!timer.Running)
        {
            timer.Duration = limitTime;
            timer.Run();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        timeRanOutEvent = new TimeRanOutEvent();
        EventManager.AddTimeRanOutInvoker(this);
        timer = gameObject.AddComponent<Timer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer.Finished)
        {
            if (firstLap)
            {
                firstLap = false;
                timer.Duration = limitTime;
                timer.Run();
            }
            else if (!StaticTrekData.Stopped)
            {
                StaticTrekData.Stopped = true;
                timeRanOutEvent.Invoke();
            }
        }
        else if (StaticTrekData.Stopped)
        {
            timer.Abort();
        }
    }
}
