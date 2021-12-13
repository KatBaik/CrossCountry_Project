using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class EventPanel : MonoBehaviour
{
    [SerializeField]
    Text panel;
    Timer timer;
    float showDurationSec = 3f;
    float showDurationAfterEndSec = 5f;
    bool final = false;
    SceneFinishedEvent sceneFinishedEvent = new SceneFinishedEvent();

    public void ChangeRunOutPanel(int i)
    {
        panel.text = "Run Out!";
    }

    public void ChangeRefusalPanel(int i)
    {
        panel.text = "Refusal!";        
    }

    public void ChangeFinishPanel()
    {
        if (timer.Running)
        {
            timer.Abort();
        }
        panel.text = "Congratulations!!!";
        timer.Duration = showDurationAfterEndSec;
        timer.Run();
        final = true;
    }

    public void ChangeObstacleSuccessPanel(int i)
    {
        if (timer.Running)
        {
            timer.Abort();
        }
        panel.text = "Success!";
        timer.Duration = showDurationSec;
        timer.Run();
    }

    public void ChangeDangerousRidingPanel()
    {
        if (timer.Running)
        {
            timer.Abort();
        }
        panel.text = "Dangerous Riding!";
        timer.Duration = showDurationSec;
        timer.Run();
    }

    public void ChangeHorseExaustedPanel()
    {
        if (timer.Running)
        {
            timer.Abort();
        }
        panel.text = "Horse Exausted!";
        timer.Duration = showDurationAfterEndSec;
        StaticTrekData.Finished = false;
        timer.Run();
        final = true;
    }

    public void ChangeRefusalEliminationPanel()
    {
        if (timer.Running)
        {
            timer.Abort();
        }
        panel.text = "Elimination: Third Refusal!";
        timer.Duration = showDurationAfterEndSec;
        StaticTrekData.Finished = false;
        timer.Run();
        final = true;
    }

    public void ChangeSimpleRefusalPanel(string kind)
    {
        if (timer.Running)
        {
            timer.Abort();
        }
        panel.text = kind + "!";
        timer.Duration = showDurationSec;
        timer.Run();
    }

    public void ChangeTimeRanOutEliminationPanel()
    {
        if (timer.Running)
        {
            timer.Abort();
        }
        panel.text = "Elimination: Extra Time Ran Out!";
        timer.Duration = showDurationAfterEndSec;
        StaticTrekData.Finished = false;
        timer.Run();
        final = true;
    }

    public void AddSceneFinishedEventListener(UnityAction listener)
    {
        sceneFinishedEvent.AddListener(listener);
    }

    // Start is called before the first frame update
    void Start()
    {
        panel.text = "";
        timer = gameObject.AddComponent<Timer>();
        EventManager.AddTimeRanOutListener(ChangeTimeRanOutEliminationPanel);
        EventManager.AddDangerousRidingListener(ChangeDangerousRidingPanel);
        EventManager.AddObstacleSuccessListener(ChangeObstacleSuccessPanel);
        EventManager.AddFinishListener(ChangeFinishPanel);
        EventManager.AddHorseExaustedListener(ChangeHorseExaustedPanel);
        EventManager.AddRefusalEliminationListener(ChangeRefusalEliminationPanel);
        EventManager.AddSimpleRefusalListener(ChangeSimpleRefusalPanel);
        EventManager.AddSceneFinishedInvoker(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (timer.Finished)
        {
            panel.text = "";
            if (final)
            {
                sceneFinishedEvent.Invoke();
            }
        }
    }
}
