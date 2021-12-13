using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static StaticTrekData;

public class PenaltyScore : MonoBehaviour
{
    [SerializeField]
    Text penalty;
    string beginning = "Penalties: ";
    float score = 0;
    TrekTiming trekTiming;
    float lastElapsedSeconds = 0;
    Dictionary<int, int> obstaclesFailed = new Dictionary<int, int>();
    RefusalEliminationEvent refusalEliminationEvent = new RefusalEliminationEvent();
    SimpleRefusalEvent simpleRefusalEvent = new SimpleRefusalEvent();

    public float Score
    {
        get { return score; }
    }

    public void AddRefusalEliminationEventListener(UnityAction listener)
    {
        refusalEliminationEvent.AddListener(listener);
    }
    public void AddSimpleRefusalEventListener(UnityAction<string> listener)
    {
        simpleRefusalEvent.AddListener(listener);
    }


    void DealWithObstacleRefusal(int id)
    {
        if (Enumerable.Sum(obstaclesFailed.Values) >= 2 && !StaticTrekData.Stopped)
        {
            StaticTrekData.Stopped = true;
            refusalEliminationEvent.Invoke();
        }
        else if (obstaclesFailed.ContainsKey(id))
        {
            score += 40f;
            obstaclesFailed[id]++;
        }
        else
        {
            score += 20f;
            obstaclesFailed.Add(id, 1);
        }
    }

    public void AddRefusalPenalty(int id)
    {
        if (id > 0)
        {
            DealWithObstacleRefusal(id);
            if (!StaticTrekData.Stopped)
            {
                simpleRefusalEvent.Invoke("Refusal");
            }
        }        
    }

    public void AddRunOutPenalty(int id)
    {
        if (id > 0)
        {
            DealWithObstacleRefusal(id);
            if (!StaticTrekData.Stopped)
            {
                simpleRefusalEvent.Invoke("Run Out");
            }
        }
    }

    public void AddDangerousRidingPenalty()
    {
        score += 25f;
    }

    // Start is called before the first frame update
    void Start()
    {
        trekTiming = ((GameObject.FindGameObjectsWithTag("Trek"))[0]).GetComponent<TrekTiming>();
        EventManager.AddRefusalEliminationInvoker(this);
        EventManager.AddSimpleRefusalInvoker(this);
        EventManager.AddDangerousRidingListener(AddDangerousRidingPenalty);
        EventManager.AddRefusalListener(AddRefusalPenalty);
        EventManager.AddRunOutListener(AddRunOutPenalty);
    }

    // Update is called once per frame
    void Update()
    {
        penalty.text = beginning + score.ToString("0.0");
        if (trekTiming.IsSecondLap)
        {
            score += (trekTiming.ElapsedTime - lastElapsedSeconds) * 0.4f;
            lastElapsedSeconds = trekTiming.ElapsedTime;            
        }
        StaticTrekData.PenaltyScore = score;
    }
}
