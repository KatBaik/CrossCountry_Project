using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{
    static TrekTiming timeRanOutInvoker;
    static List<UnityAction> timeRanOutListeners = new List<UnityAction>();

    public static void AddTimeRanOutInvoker(TrekTiming invoker)
    {
        timeRanOutInvoker = invoker;
        if (timeRanOutListeners.Count > 0)
        {
            foreach (UnityAction timeRanOutListener in timeRanOutListeners)
            {
                invoker.AddTimeRanOutEventListener(timeRanOutListener);
            }            
        }
    }

    public static void AddTimeRanOutListener(UnityAction listener)
    {
        timeRanOutListeners.Add(listener);
        if (timeRanOutInvoker != null)
        {
            timeRanOutInvoker.AddTimeRanOutEventListener(listener);
        }
    }

    static List<Horse> dangerousRidingInvokers = new List<Horse>();
    static List<UnityAction> dangerousRidingListeners = new List<UnityAction>();

    public static void AddDangerousRidingInvoker(Horse invoker)
    {
        dangerousRidingInvokers.Add(invoker);
        if (dangerousRidingListeners.Count > 0)
        {
            foreach (UnityAction listener in dangerousRidingListeners)
            {
                invoker.AddDangerousRidingEventListener(listener);
            }
        }
    }

    public static void AddDangerousRidingListener(UnityAction listener)
    {
        dangerousRidingListeners.Add(listener);
        if (dangerousRidingInvokers.Count > 0)
        {
            foreach (Horse invoker in dangerousRidingInvokers)
            {
                invoker.AddDangerousRidingEventListener(listener);
            }
        }
    }

    static List<Horse> obstacleFailureInvokers = new List<Horse>(); 
    static List<UnityAction<int>> obstacleFailureListeners = new List<UnityAction<int>>();

    public static void AddObstacleFailureInvoker(Horse invoker)
    {
        obstacleFailureInvokers.Add(invoker);
        if (obstacleFailureListeners.Count > 0)
        {
            foreach (UnityAction<int> listener in obstacleFailureListeners)
            {
                invoker.AddObstacleFailureEventListener(listener);
            }
        }
    }

    public static void AddObstacleFailureListener(UnityAction<int> listener)
    {
        obstacleFailureListeners.Add(listener);
        if (obstacleFailureInvokers.Count > 0)
        {
            foreach (Horse invoker in obstacleFailureInvokers)
            {
                invoker.AddObstacleFailureEventListener(listener);
            }
        }
    }

    static Trek obstacleSuccessInvoker;
    static List<UnityAction<int>> obstacleSuccessListeners = new List<UnityAction<int>>();

    public static void AddObstacleSuccessInvoker(Trek invoker)
    {
        obstacleSuccessInvoker = invoker;
        if (obstacleSuccessListeners.Count > 0)
        {
            foreach (UnityAction<int> listener in obstacleSuccessListeners)
            {
                invoker.AddObstacleSuccessEventListener(listener);
            }
        }
    }

    public static void AddObstacleSuccessListener(UnityAction<int> listener)
    {
        obstacleSuccessListeners.Add(listener);
        if (obstacleSuccessInvoker != null)
        {
            obstacleSuccessInvoker.AddObstacleSuccessEventListener(listener);
        }
    }

    static Trek finishInvoker;
    static List<UnityAction> finishListeners = new List<UnityAction>();

    public static void AddFinishInvoker(Trek invoker)
    {
        finishInvoker = invoker;
        if (finishListeners.Count > 0)
        {
            foreach (UnityAction listener in finishListeners)
            {
                invoker.AddFinishEventListener(listener);
            }
        }
    }

    public static void AddFinishListener(UnityAction listener)
    {
        finishListeners.Add(listener);
        if (finishInvoker != null)
        {
            finishInvoker.AddFinishEventListener(listener);
        }
    }

    static List<Horse> overjumpedObstacleInvokers = new List<Horse>();
    static List<UnityAction<int>> overjumpedObstacleListeners = new List<UnityAction<int>>();

    public static void AddOverjumpedObstacleInvoker(Horse invoker)
    {
        overjumpedObstacleInvokers.Add(invoker);
        if (overjumpedObstacleListeners.Count > 0)
        {
            foreach (UnityAction<int> listener in overjumpedObstacleListeners)
            {
                invoker.AddOverjumpedObstacleEventListener(listener);
            }
        }
    }

    public static void AddOverjumpedObstacleListener(UnityAction<int> listener)
    {
        overjumpedObstacleListeners.Add(listener);
        if (overjumpedObstacleInvokers.Count > 0)
        {
            foreach (Horse invoker in overjumpedObstacleInvokers)
            {
                invoker.AddOverjumpedObstacleEventListener(listener);
            }
        }
    }

    static List<Horse> refusalInvokers = new List<Horse>();
    static List<UnityAction<int>> refusalListeners = new List<UnityAction<int>>();

    public static void AddRefusalInvoker(Horse invoker)
    {
        refusalInvokers.Add(invoker);
        if (refusalListeners.Count > 0)
        {
            foreach (UnityAction<int> listener in refusalListeners)
            {
                invoker.AddRefusalEventListener(listener);
            }
        }
    }

    public static void AddRefusalListener(UnityAction<int> listener)
    {
        refusalListeners.Add(listener);
        if (refusalInvokers.Count > 0)
        {
            foreach (Horse invoker in refusalInvokers)
            {
                invoker.AddRefusalEventListener(listener);
            }
        }
    }

    static List<Horse> runOutInvokers = new List<Horse>();
    static List<UnityAction<int>> runOutListeners = new List<UnityAction<int>>();

    public static void AddRunOutInvoker(Horse invoker)
    {
        runOutInvokers.Add(invoker);
        if (runOutListeners.Count > 0)
        {
            foreach (UnityAction<int> listener in runOutListeners)
            {
                invoker.AddRunOutEventListener(listener);
            }
        }
    }

    public static void AddRunOutListener(UnityAction<int> listener)
    {
        runOutListeners.Add(listener);
        if (runOutInvokers.Count > 0)
        {
            foreach (Horse invoker in runOutInvokers)
            {
                invoker.AddRunOutEventListener(listener);
            }
        }
    }

    static List<Horse> horseExaustedInvokers = new List<Horse>();
    static List<UnityAction> horseExaustedListeners = new List<UnityAction>();

    public static void AddHorseExaustedInvoker(Horse invoker)
    {
        horseExaustedInvokers.Add(invoker);
        if (horseExaustedListeners.Count > 0)
        {
            foreach (UnityAction listener in horseExaustedListeners)
            {
                invoker.AddHorseExaustedEventListener(listener);
            }
        }
    }

    public static void AddHorseExaustedListener(UnityAction listener)
    {
        horseExaustedListeners.Add(listener);
        if (horseExaustedInvokers.Count > 0)
        {
            foreach (Horse invoker in horseExaustedInvokers)
            {
                invoker.AddHorseExaustedEventListener(listener);
            }
        }
    }

    static PenaltyScore refusalEliminationInvokers;
    static List<UnityAction> refusalEliminationListeners = new List<UnityAction>();

    public static void AddRefusalEliminationInvoker(PenaltyScore invoker)
    {
        refusalEliminationInvokers = invoker;
        if (refusalEliminationListeners.Count > 0)
        {
            foreach (UnityAction listener in refusalEliminationListeners)
            {
                invoker.AddRefusalEliminationEventListener(listener);
            }
        }
    }

    public static void AddRefusalEliminationListener(UnityAction listener)
    {
        refusalEliminationListeners.Add(listener);
        if (refusalEliminationInvokers != null)
        {
            refusalEliminationInvokers.AddRefusalEliminationEventListener(listener);
        }
    }

    static PenaltyScore simpleRefusalInvokers;
    static List<UnityAction<string>> simpleRefusalListeners = new List<UnityAction<string>>();

    public static void AddSimpleRefusalInvoker(PenaltyScore invoker)
    {
        simpleRefusalInvokers = invoker;
        if (simpleRefusalListeners.Count > 0)
        {
            foreach (UnityAction<string> listener in simpleRefusalListeners)
            {
                invoker.AddSimpleRefusalEventListener(listener);
            }
        }
    }

    public static void AddSimpleRefusalListener(UnityAction<string> listener)
    {
        simpleRefusalListeners.Add(listener);
        if (simpleRefusalInvokers != null)
        {
            simpleRefusalInvokers.AddSimpleRefusalEventListener(listener);
        }
    }

    static List<Horse> closestObstacleFoundInvokers = new List<Horse>();
    static List<UnityAction<int>> closestObstacleFoundListeners = new List<UnityAction<int>>();

    public static void AddClosestObstacleFoundInvoker(Horse invoker)
    {
        closestObstacleFoundInvokers.Add(invoker);
        if (closestObstacleFoundListeners.Count > 0)
        {
            foreach (UnityAction<int> listener in closestObstacleFoundListeners)
            {
                invoker.AddClosestObstacleFoundEventListener(listener);
            }
        }
    }

    public static void AddClosestObstacleFoundListener(UnityAction<int> listener)
    {
        closestObstacleFoundListeners.Add(listener);
        if (closestObstacleFoundInvokers.Count > 0)
        {
            foreach (Horse invoker in closestObstacleFoundInvokers)
            {
                invoker.AddClosestObstacleFoundEventListener(listener);
            }
        }
    }

    static List<Horse> noObstacleDetectedInvokers = new List<Horse>();
    static List<UnityAction> noObstacleDetectedListeners = new List<UnityAction>();

    public static void AddNoObstacleDetectedInvoker(Horse invoker)
    {
        noObstacleDetectedInvokers.Add(invoker);
        if (noObstacleDetectedListeners.Count > 0)
        {
            foreach (UnityAction listener in noObstacleDetectedListeners)
            {
                invoker.AddNoObstacleDetectedEventListener(listener);
            }
        }
    }

    public static void AddNoObstacleDetectedListener(UnityAction listener)
    {
        noObstacleDetectedListeners.Add(listener);
        if (noObstacleDetectedInvokers.Count > 0)
        {
            foreach (Horse invoker in noObstacleDetectedInvokers)
            {
                invoker.AddNoObstacleDetectedEventListener(listener);
            }
        }
    }

    static EventPanel sceneFinishedInvoker;
    static List<UnityAction> sceneFinishedListeners = new List<UnityAction>();

    public static void AddSceneFinishedInvoker(EventPanel invoker)
    {
        sceneFinishedInvoker = invoker;
        if (sceneFinishedListeners.Count > 0)
        {
            foreach (UnityAction listener in sceneFinishedListeners)
            {
                invoker.AddSceneFinishedEventListener(listener);
            }
        }
    }

    public static void AddSceneFinishedListener(UnityAction listener)
    {
        sceneFinishedListeners.Add(listener);
        if (sceneFinishedInvoker != null)
        {
            sceneFinishedInvoker.AddSceneFinishedEventListener(listener);
        }
    }

    static Trek trekCreatedInvoker;
    static UnityAction trekCreatedListener;

    public static void AddTrekCreatedInvoker(Trek invoker)
    {
        trekCreatedInvoker = invoker;
        if (trekCreatedListener != null)
        {
            invoker.AddTrekCreatedEventListener(trekCreatedListener);
        }
    }

    public static void AddTrekCreatedListener(UnityAction listener)
    {
        trekCreatedListener = listener;
        if (trekCreatedInvoker != null)
        {
            trekCreatedInvoker.AddTrekCreatedEventListener(listener);
        }
    }

    static HorseConnector horseConfigFileIdentifiedInvoker;
    static UnityAction horseConfigFileIdentifiedListener;

    public static void AddHorseConfigFileIdentifiedInvoker(HorseConnector invoker)
    {
        horseConfigFileIdentifiedInvoker = invoker;
        if (horseConfigFileIdentifiedListener != null)
        {
            invoker.AddHorseConfigFileIdentifiedEventListener(horseConfigFileIdentifiedListener);
        }
    }

    public static void AddHorseConfigFileIdentifiedListener(UnityAction listener)
    {
        horseConfigFileIdentifiedListener = listener;
        if (horseConfigFileIdentifiedInvoker != null)
        {
            horseConfigFileIdentifiedInvoker.AddHorseConfigFileIdentifiedEventListener(listener);
        }
    }
}
