using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Breed;
using static GaitName;
using System;
using static HorseConfigUtils;
using static ExtDebug;

public class Horse : MonoBehaviour
{
    string name = "Horse";
    //float age;
    //Player owner;
    Breed breed;
    int bestScore;    
    GaitManager gaitManager;
    Animator animator;
    Obstacle? closestObstacleAhead = null;
    GameObject parent;
    float horseHeight;

    /*PARAMETERS REGARDING THE NEXT STEP*/
    Timer gaitStep;
    GaitName? lastCommand = null;
    int stepsToMoveLeft = 0;
    int toSlowDown = 0;
    int toSpeedUp = 0;
    bool toTurn = false;
    int turningNow = 0;
    int turnDirection = 0;

    /*DATA FOR A JUMP*/
    float jumpVerticalSpeed = 10f;
    float targetHeight = 0;
    float lastSpeed = 0;
    Timer refusalTimer;
    float waitInRefusalSec = 5f;

    /*HORSE DATA*/
    float speedQuant;
    float rotateDegreesPerSecond;
    int lookAhead;
    float jumpHorizontalSpeed;

    /*Skills*/
    float currentSkittishness;
    float currentTrust;
    float currentObedience;
    float currentEndurance = 1f;

    /*Events*/
    DangerousRidingEvent dangerousRidingEvent = new DangerousRidingEvent();
    ObstacleFailureEvent obstacleFailureEvent = new ObstacleFailureEvent();
    OverjumpedObstacleEvent overjumpedObstacleEvent = new OverjumpedObstacleEvent();
    RefusalEvent refusalEvent = new RefusalEvent();
    RunOutEvent runOutEvent = new RunOutEvent();
    HorseExaustedEvent horseExaustedEvent = new HorseExaustedEvent();
    ClosestObstacleFoundEvent closestObstacleFoundEvent = new ClosestObstacleFoundEvent();
    NoObstacleDetectedEvent noObstacleDetectedEvent = new NoObstacleDetectedEvent();


    /*public string Owner
    {
        get { return owner.Name; }
    }*/

    public float Endurance
    {
        get { return currentEndurance; }
    }

    public float Speed
    {
        get { return gaitManager.CurrentSpeed; }
    }

    public float Trust
    {
        get { return currentTrust; }
    }

    public float Obedience
    {
        get { return currentObedience; }
    }

    public float Skittishness
    {
        get { return currentSkittishness; }
    }

    void AddEventsToEventManager()
    {
        EventManager.AddDangerousRidingInvoker(this);
        EventManager.AddObstacleFailureInvoker(this);
        EventManager.AddOverjumpedObstacleInvoker(this);
        EventManager.AddRunOutInvoker(this);
        EventManager.AddRefusalInvoker(this);
        EventManager.AddHorseExaustedInvoker(this);
        EventManager.AddClosestObstacleFoundInvoker(this);
        EventManager.AddNoObstacleDetectedInvoker(this);
    }

    public void AddDangerousRidingEventListener(UnityAction listener)
    {
        dangerousRidingEvent.AddListener(listener);
    }
    public void AddObstacleFailureEventListener(UnityAction<int> listener)
    {
        obstacleFailureEvent.AddListener(listener);
    }
    public void AddOverjumpedObstacleEventListener(UnityAction<int> listener)
    {
        overjumpedObstacleEvent.AddListener(listener);
    }
    public void AddRefusalEventListener(UnityAction<int> listener)
    {
        refusalEvent.AddListener(listener);
    }
    public void AddRunOutEventListener(UnityAction<int> listener)
    {
        runOutEvent.AddListener(listener);
    }
    public void AddHorseExaustedEventListener(UnityAction listener)
    {
        horseExaustedEvent.AddListener(listener);
    }
    public void AddClosestObstacleFoundEventListener(UnityAction<int> listener)
    {
        closestObstacleFoundEvent.AddListener(listener);
    }
    public void AddNoObstacleDetectedEventListener(UnityAction listener)
    {
        noObstacleDetectedEvent.AddListener(listener);
    }

    void SetupGaits()
    {
        gaitStep = gameObject.AddComponent<Timer>();
        gaitManager = new GaitManager("HorseConfig.csv", animator, Canter, Trot, Walk, Stop, Jump);
    }

    void SetupHorseData()
    {
        speedQuant = HorseConfigUtils.SpeedQuant;
        rotateDegreesPerSecond = HorseConfigUtils.RotateDegreesPerSecond;
        lookAhead = HorseConfigUtils.LookAhead;
        jumpHorizontalSpeed = HorseConfigUtils.JumpHorizontalSpeed;
        currentEndurance = HorseConfigUtils.Endurance;
        currentSkittishness = HorseConfigUtils.Skittishness;
        currentTrust = HorseConfigUtils.Trust;
        currentObedience = HorseConfigUtils.Obedience;
    }

    void GetRandomTurn()
    {
        while (Math.Abs(turnDirection) < 9)
        {
            turnDirection = Math.Sign(UnityEngine.Random.Range(-10, 10))* UnityEngine.Random.Range(9,12);
        }        
        toTurn = true;
    }
    
    public void CommitRefusal()
    {
        Stop();
        int id = 0;
        if (closestObstacleAhead != null)
        {
            id = closestObstacleAhead.Id;
        }
        if (!refusalTimer.Running) //To avoid a sequence of refusals from 1 action
        {
            refusalTimer.Duration = waitInRefusalSec;
            refusalTimer.Run();
            refusalEvent.Invoke(id);
        }        
    }

    public void CommitRunOut()
    {
        GetRandomTurn();
        CheckTurn();
        MoveAtLeast(3);
        int id = 0;
        if (closestObstacleAhead != null)
        {
            id = closestObstacleAhead.Id;
        }
        if (!refusalTimer.Running) //To avoid a sequence of refusals from 1 action
        {
            refusalTimer.Duration = waitInRefusalSec;
            refusalTimer.Run();
            runOutEvent.Invoke(id);
        }        
    }

    public bool IsSafe(Obstacle? obstacle)
    {
        if (obstacle == null)
        {
            return true;
        }
        float distance = Vector3.Distance(transform.position, obstacle.transform.position);
        if (jumpHorizontalSpeed * gaitManager.JumpDuration < distance)
        {
            return true;
        }
        /*Debug.Log(obstacle.IsQualified(gaitManager.CurrentSpeed,
            HorseConfigUtils.JumpingHeight, HorseConfigUtils.JumpingWidth, currentEndurance));*/
        if (distance*1.5 < jumpHorizontalSpeed * gaitManager.JumpDuration 
            && obstacle.IsQualified(lastSpeed, 
            HorseConfigUtils.JumpingHeight, HorseConfigUtils.JumpingWidth, currentEndurance))
        {
            return true;
        }
        currentTrust *= 0.9f;
        return false;
    }

    public bool IsScared(Obstacle? obstacle)
    {
        if (obstacle == null || obstacle.IsAgreeable(currentSkittishness, currentObedience, currentTrust))
        {
            return true;
        }
        currentSkittishness = (float) Math.Max(1, (double)currentSkittishness*1.1f);
        return false;
    }

    public bool WillObey(Obstacle? obstacle)
    {
        if (IsSafe(obstacle))
        {
            if (!IsScared(obstacle) || 
                currentTrust*currentObedience > currentSkittishness*currentSkittishness)
            {
                int randomFactor = UnityEngine.Random.Range(0, 80);
                if (randomFactor <= currentTrust * currentObedience * 100)
                {
                    currentTrust = (float)Math.Min((double)1.05f* currentTrust, (double)HorseConfigUtils.Trust);
                    currentSkittishness *= 0.95f;
                    return true;
                }                
            }
        }
        return false;
    }

    public void Turn()
    {
        SlowDownOnStep(speedQuant / 200);
        float rotationAmount = rotateDegreesPerSecond * Time.deltaTime;
        parent.transform.Rotate(Vector3.up, turningNow * rotationAmount);
    }

    public void TurnRight()
    {
        turnDirection += 1;
        toTurn = true;
    }

    public void TurnLeft()
    {
        turnDirection -= 1;
        toTurn = true;
    }

    void SlowDownOnStep(float coeff)
    {
        if (parent.GetComponent<Rigidbody>().velocity.magnitude > 0)
        {
            Vector3 vel = parent.GetComponent<Rigidbody>().velocity;
            if (vel.magnitude == 0)
            {
                vel = parent.transform.forward;
            }
            gaitManager.CurrentSpeed = vel.magnitude * (1 - coeff * speedQuant);
            parent.GetComponent<Rigidbody>().velocity = gaitManager.CurrentSpeed * parent.transform.forward;
        }
    }

    void SpeedUpOnStep()
    {
        Vector3 vel = parent.GetComponent<Rigidbody>().velocity;
        if (vel.magnitude == 0)
        {
            vel = parent.transform.forward;
        }
        try
        {
            gaitManager.CurrentSpeed = vel.magnitude * (1 + toSpeedUp * speedQuant);
            parent.GetComponent<Rigidbody>().velocity = gaitManager.CurrentSpeed * parent.transform.forward;
        }
        catch (TooFastException e)
        {
            dangerousRidingEvent.Invoke();
        }
    }

    public void SlowDown()
    {
        toSlowDown++;     
    }

    public void SpeedUp()
    {
        toSpeedUp++;
    }
    
    void Move()
    {
        lastSpeed = gaitManager.CurrentSpeed;
        gaitManager.StartMoving();
        animator.SetTrigger(gaitManager.AnimTrigger);
        gaitStep.Duration = gaitManager.AnimLength;
        gaitStep.Run();
        Vector3 forward = parent.transform.forward;
        parent.GetComponent<Rigidbody>().velocity = new Vector3(gaitManager.CurrentSpeed * forward.x, gaitManager.CurrentSpeed * forward.y, gaitManager.CurrentSpeed * forward.z);
        try
        {
            gaitManager.CurrentSpeed = parent.GetComponent<Rigidbody>().velocity.magnitude;
        }
        catch (TooFastException e)
        {
            dangerousRidingEvent.Invoke();
        }
        gaitManager.Move();
    }

    public void Stop()
    {
        if (lastCommand == null)
        {
            lastCommand = GaitName.Standing;
        }
        if (gaitStep.Finished && stepsToMoveLeft == 0)
        {
            gaitManager.CurrentGait = GaitName.Standing;
            lastCommand = null;
            Move();
            return;
        }
                
    }

    public void Walk()
    {
        if (gaitStep.Finished && stepsToMoveLeft == 0)
        {
            gaitManager.CurrentGait = GaitName.Walk;
            if (!(lastCommand == GaitName.Jump && closestObstacleAhead != null))
            {
                lastCommand = null;
            }            
            Move();
            return;
        }
        if (lastCommand == null)
        {
            lastCommand = GaitName.Walk;
        }                
    }

    public void Trot()
    {
        if (gaitStep.Finished && stepsToMoveLeft == 0)
        {
            gaitManager.CurrentGait = GaitName.Trot;
            if (!(lastCommand == GaitName.Jump && closestObstacleAhead != null))
            {
                lastCommand = null;
            }
            Move();
            return;
        }
        if (lastCommand == null)
            lastCommand = GaitName.Trot;
    }
    
    public void Canter()
    {
        if (gaitStep.Finished && stepsToMoveLeft == 0)
        {
            gaitManager.CurrentGait = GaitName.Canter;
            if (!(lastCommand == GaitName.Jump && closestObstacleAhead != null))
            {
                lastCommand = null;
            }
            Move();
            return;
        }
        if (lastCommand == null)
            lastCommand = GaitName.Canter;
    }
    
    public void Jump() //TODO work with obstacle
    {
        stepsToMoveLeft = 0;
        if (!gaitStep.Finished)
        {
            lastCommand = GaitName.Jump;
            return;            
        }
        if (closestObstacleAhead == null)
        {
            lastCommand = GaitName.Canter;
            gaitManager.CurrentGait = GaitName.Canter;
            gaitManager.Move();
            return;
        }
        lastCommand = GaitName.Canter;
        if (WillObey(closestObstacleAhead))
        {
            gaitManager.CurrentGait = GaitName.Jump;
            animator.SetTrigger(gaitManager.JumpTrigger);
            gaitStep.Duration = gaitManager.JumpDuration;
            gaitStep.Run();            
            Vector3 forward = parent.transform.forward;
            targetHeight = closestObstacleAhead.Height + horseHeight;
            jumpVerticalSpeed = targetHeight * 2.1f / gaitManager.JumpDuration;
            parent.GetComponent<Rigidbody>().velocity = new Vector3(jumpHorizontalSpeed * forward.x, jumpVerticalSpeed, jumpHorizontalSpeed * forward.z);
            gaitManager.Move();
        }
        else
        {
            currentObedience *= 0.9f;
            Disobey();
        }
    }

    void CheckFenceAhead()
    {
        int layer_mask = LayerMask.GetMask("Hedge");
        RaycastHit[] hits = Physics.BoxCastAll(new Vector3(parent.transform.position.x, horseHeight / 2, parent.transform.position.z), //center
            new Vector3(1, horseHeight, gaitManager.AnimLength * gaitManager.CurrentSpeed), //TODO halfextends
            parent.transform.forward, //direction
            parent.transform.rotation, //orientation
            gaitManager.AnimLength * gaitManager.CurrentSpeed, //maxDistance
            layer_mask);
        /*long delay = 100000;
        for (long i=0; i < delay; i++){
            ExtDebug.DrawBoxCastOnHit(new Vector3(parent.transform.position.x, horseHeight / 2, parent.transform.position.z),
                new Vector3(horseHeight, horseHeight, gaitManager.AnimLength * gaitManager.CurrentSpeed),
                parent.transform.rotation,
                parent.transform.forward,
                gaitManager.AnimLength * gaitManager.CurrentSpeed,
                Color.red);
        }*/
        if (hits.Length > 0)
        {
            Stop();
        }
    }

    public Obstacle GetObstacle()
    {
        int layer_mask = LayerMask.GetMask("Obstacle");
        float maxSpeed = Mathf.Max(gaitManager.CurrentSpeed, jumpHorizontalSpeed);
        float maxLookAhead = gaitManager.AnimLength * maxSpeed * lookAhead;
        RaycastHit[] hits = Physics.BoxCastAll(new Vector3(parent.transform.position.x + parent.transform.forward.x*maxLookAhead/2, 
            horseHeight / 2, parent.transform.position.z + parent.transform.forward.z * maxLookAhead/ 2), //center
            new Vector3(1f, horseHeight,maxLookAhead), //TODO halfextends
            parent.transform.forward, //direction
            parent.transform.rotation, //orientation
            maxLookAhead, //maxDistance
            layer_mask); //layerMask
        /*long delay = 100000;
        for (long i=0; i < delay; i++){
            ExtDebug.DrawBoxCastOnHit(new Vector3(parent.transform.position.x + parent.transform.forward.x * maxLookAhead / 2,
            horseHeight / 2, parent.transform.position.z + parent.transform.forward.z * maxLookAhead / 2),
                new Vector3(1f, horseHeight, maxLookAhead),
                parent.transform.rotation,
                parent.transform.forward,
                maxLookAhead,
                Color.red);
        }*/
        
        RaycastHit? closestHit = null;
        float minDist = -1;
        foreach(RaycastHit hit in hits)
        {
            float dist = Vector3.Distance(parent.transform.position, hit.transform.position);
            if (minDist<0 || dist < minDist)
            {
                closestHit = hit;
                minDist = dist;
            }
        }
        if (closestHit != null)
        {
            return ((RaycastHit)closestHit).transform.gameObject.GetComponent<Obstacle>();
        }
        return null;
    }

    public void Spook()
    {
        GetRandomTurn();
        CheckTurn();
        Canter();
        MoveAtLeast(5);
    }

    public void Disobey()
    {
        int decision = UnityEngine.Random.Range(0,3);
        switch (decision)
        {
            case 0:
                Stop();
                break;
            case 1:
                CommitRunOut();
                break;
            case 2:
                Spook();
                break;
        }
    }

    public void MoveAtLeast(int stepsNumber)
    {
        stepsToMoveLeft = stepsNumber;
    }
    
    public void Train()
    {

    }

    void AvoidObstacles()
    {
        float distance = Vector3.Distance(
            new Vector3(parent.transform.position.x, 0f, parent.transform.position.z),
            new Vector3(closestObstacleAhead.gameObject.transform.position.x, 0f, closestObstacleAhead.gameObject.transform.position.z));
        int decidesToAvoid = UnityEngine.Random.Range(0,
            (int)(gaitManager.AnimLength * gaitManager.CurrentSpeed * lookAhead / (1 - currentObedience)));
        if (distance < jumpHorizontalSpeed * gaitManager.JumpDuration / 10 
            || (lastCommand == null && distance<gaitManager.AnimLength* gaitManager.CurrentSpeed)
            || (lastCommand != null && distance<gaitManager.NextStepLength((GaitName)lastCommand)
                *gaitManager.StepExpectedSpeed((GaitName)lastCommand, currentEndurance))
            || (currentEndurance < HorseConfigUtils.EnduranceMinForJump))
        {
            CommitRefusal();
        }
        else if (distance < jumpHorizontalSpeed * gaitManager.JumpDuration 
            && currentEndurance > HorseConfigUtils.EnduranceMinForRunOut 
            && decidesToAvoid == 0)
        {
            CommitRunOut();
        }
    }
    
    void CheckObstacleAhead()
    {
        closestObstacleAhead = GetObstacle();
        if (closestObstacleAhead!=null)
        {
            closestObstacleFoundEvent.Invoke(closestObstacleAhead.Id);
        }
        else
        {
            noObstacleDetectedEvent.Invoke();
        }
        if (gaitManager.CurrentSpeed > 0 && closestObstacleAhead != null && lastCommand != GaitName.Jump && gaitManager.CurrentGait != GaitName.Jump)
        {
            AvoidObstacles();
        }
    }

    void CheckTurn()
    {
        if (toTurn)
        {
            toTurn = false;
            turningNow = turnDirection;
        }
        else
        {
            turningNow = 0;
        }
        turnDirection = 0;
    }

    void CheckSlowDownSpeedUp()
    {
        if (toSlowDown*toSpeedUp != 0)
        {
            currentObedience *= 0.95f;
            Disobey();
        }
        else
        {
            if (toSlowDown > 0)
            {
                SlowDownOnStep(toSlowDown);
            }
            else if (toSpeedUp > 0)
            {
                SpeedUpOnStep();
            }
        }
        toSpeedUp = 0;
        toSlowDown = 0;
    }
    
    void CheckGaits()
    {
        if (stepsToMoveLeft > 0)
        {
            stepsToMoveLeft--;
            gaitManager.ContinueGait();
        }
        else
        {
            /*Changing gait AND without a risk to run into the obstacle*/
            if (lastCommand != null && 
                (closestObstacleAhead == null || //no obstacle
                (lastCommand == GaitName.Jump && Vector3.Distance( //obstacle, but we can overjump it
                    closestObstacleAhead.gameObject.transform.position, transform.position) < jumpHorizontalSpeed * gaitManager.JumpDuration/2) ||
                    (lastCommand != GaitName.Jump && Vector3.Distance( //obstacle, but far enough not to jump
                    closestObstacleAhead.gameObject.transform.position, transform.position) > 
                    gaitManager.StepExpectedSpeed((GaitName)lastCommand, currentEndurance)*gaitManager.NextStepLength((GaitName)lastCommand) + jumpHorizontalSpeed * gaitManager.JumpDuration/5)))
            {
                gaitManager.ChangeGait(lastCommand);
            }
            /*Too close to the obstacle, won't move at all*/
            else if (closestObstacleAhead != null && Vector3.Distance(
                closestObstacleAhead.gameObject.transform.position, transform.position) < jumpHorizontalSpeed * gaitManager.JumpDuration / 10)
            {
                Stop();
            }
            /*Changing gait, but need to adjust speed in order not to run into an obstacle on the next step*/
            else if (closestObstacleAhead != null && lastCommand != GaitName.Jump && lastCommand != null && 
                Vector3.Distance(closestObstacleAhead.gameObject.transform.position, transform.position) <= 
                    gaitManager.StepExpectedSpeed((GaitName)lastCommand, currentEndurance) * 
                    gaitManager.NextStepLength((GaitName)lastCommand) + jumpHorizontalSpeed * gaitManager.JumpDuration / 5)
            {
                gaitManager.ChangeGait(lastCommand);
                gaitManager.CurrentSpeed = (Vector3.Distance(
                    closestObstacleAhead.gameObject.transform.position, transform.position) - jumpHorizontalSpeed * gaitManager.JumpDuration / 5) / gaitManager.AnimLength;
            }
            /*jumping ahead or no gait change, but need to adjust speed in order not to run into an obstacle on the next step */
            else if (closestObstacleAhead != null && Vector3.Distance(closestObstacleAhead.gameObject.transform.position, transform.position) <=
                    gaitManager.CurrentSpeed * gaitManager.AnimLength + jumpHorizontalSpeed * gaitManager.JumpDuration / 5)
            {
                gaitManager.CurrentSpeed = (Vector3.Distance(
                        closestObstacleAhead.gameObject.transform.position, transform.position) - jumpHorizontalSpeed * gaitManager.JumpDuration / 5) / gaitManager.AnimLength;
                gaitManager.ContinueGait();
            }
            /*No command to change gait*/
            else
            {
                gaitManager.ContinueGait();
            }
        }
    }

    void CheckJump()
    {
        Vector3 vel = parent.GetComponent<Rigidbody>().velocity;
        if (vel.y>0 && parent.transform.position.y > targetHeight)
        {
            if (closestObstacleAhead != null)
            {
                overjumpedObstacleEvent.Invoke(closestObstacleAhead.Id);
            }            
            targetHeight = horseHeight;
            parent.GetComponent<Rigidbody>().velocity = new Vector3(vel.x, -vel.y, vel.z);
        }
        else if(vel.y<0 && parent.transform.position.y < targetHeight)
        {
            parent.GetComponent<Rigidbody>().velocity = new Vector3(vel.x, 0, vel.z);
            parent.transform.position = new Vector3(parent.transform.position.x, horseHeight, parent.transform.position.z);
            closestObstacleAhead = null;
        }
    }


    void CheckForNextStep()
    {
        CheckFenceAhead();
        CheckObstacleAhead();
        CheckTurn();
        CheckSlowDownSpeedUp();
        CheckGaits();
    }


    // Start is called before the first frame update
    void Start()
    {
        refusalTimer = gameObject.AddComponent<Timer>();
        parent = transform.parent.gameObject;
        horseHeight = parent.transform.position.y;
        animator = gameObject.GetComponent<Animator>();
        SetupGaits();
        SetupHorseData();
        AddEventsToEventManager();
        gaitManager.CurrentGait = GaitName.Standing;
        lastCommand = null;
        Move();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentEndurance <= 0 && !StaticTrekData.Stopped)
        {
            Stop();
            StaticTrekData.Stopped = true;
            horseExaustedEvent.Invoke();
        }
        else if (gaitStep.Finished)
        {
            currentEndurance = gaitManager.DeriveCurrentEnduranceFromSpeed(currentEndurance);
            CheckForNextStep();            
        }
        else if (gaitManager.CurrentGait == GaitName.Jump)
        {
            CheckJump();
        }
        else if(turningNow != 0)
        {
            Turn();
        }
    }
}
