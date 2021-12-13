using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GaitName;
using static HorseConfigUtils;

public class GaitManager
{
    Gait currentGait;
    Dictionary<GaitName, Gait> gaits;
    float personalStamina = 1f;

    public GaitManager(string configFileName, Animator animator,
        Action canter, Action trot, Action walk,
        Action standing, Action jump)
    {
        HorseConfigUtils.Initialize(StaticTrekData.HorseConfigFile);
        CreateGaitHierarchy(animator, canter, trot, walk, standing, jump);
    }
    
    void CreateGaitHierarchy(Animator animator,
        Action canterFunc, Action trotFunc, Action walkFunc,
        Action standingFunc, Action jumpFunc)
    {
        gaits = new Dictionary<GaitName, Gait>();
        Gait? canter = null;
        Gait? trot = null;
        Gait? walk = null;
        Gait? standing = null;
        Gait? jump = null;
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            switch (clip.name)
            {
                case "Canter":
                    canter = new Canter(HorseConfigUtils.MaxCanter, HorseConfigUtils.MinCanter, 2.0, canterFunc, clip.length/2, "Canter");
                    break;
                case "Trot":
                    trot = new Trot(HorseConfigUtils.MaxTrot, HorseConfigUtils.MinTrot, 1.5, trotFunc, clip.length, "Trot");
                    break;
                case "Walk":
                    walk = new Walk(HorseConfigUtils.MaxWalk, 0, 1.0, walkFunc, clip.length /2, "Walk");
                    break;
                case "Jump":
                    jump = new Jump(10, 10, 3.0, jumpFunc, clip.length /2, "Jump");
                    break;
                case "Stand":
                    standing = new Standing(0, 0, 1.0, standingFunc, clip.length, "Stand");
                    break;
            }
        }
        trot.Faster = canter;
        canter.Slower = trot;
        walk.Faster = trot;
        trot.Slower = walk;
        standing.Faster = walk;
        walk.Slower = standing;
        gaits.Add(GaitName.Canter, canter);
        gaits.Add(GaitName.Trot, trot);
        gaits.Add(GaitName.Walk, walk);
        gaits.Add(GaitName.Standing, standing);
        gaits.Add(GaitName.Jump, jump);
    }

    public GaitName CurrentGait
    {
        get { return currentGait.Name; }
        set { currentGait = gaits[value]; }
    }

    public float CurrentSpeed
    {
        get { return currentGait.CurrentSpeed; }
        set 
        {
            try
            {
                currentGait.CurrentSpeed = value;
            }
            catch (TooSlowException e1)
            {
                bool success = false;
                while(currentGait.Slower != null && !success)
                {
                    currentGait = currentGait.Slower;
                    try
                    {
                        currentGait.CurrentSpeed = value;
                        success = true;
                    }
                    catch (TooSlowException e1_1) { }
                }
                if (!success)
                {
                    currentGait = gaits[GaitName.Standing];
                    currentGait.CurrentSpeed = 0;
                }
            }
            catch (TooFastException e2)
            {
                bool success = false;
                while (currentGait.Faster != null && !success)
                {
                    currentGait = currentGait.Faster;
                    try
                    {
                        currentGait.CurrentSpeed = value;
                        success = true;
                    }
                    catch (TooFastException e2_2) { }
                }
                if (!success)
                {
                    currentGait = gaits[GaitName.Canter];
                    CurrentSpeed = currentGait.Maximum;
                    throw new TooFastException();                    
                }
            }
        }
    }

    public float JumpDuration
    {
        get { return gaits[GaitName.Jump].AnimLength; }
    }

    public string JumpTrigger
    {
        get { return gaits[GaitName.Jump].AnimTrigger; }
    }

    public string AnimTrigger
    {
        get { return currentGait.AnimTrigger; }
    }

    public float AnimLength
    {
        get { return currentGait.AnimLength; }
    }

    public float NextStepLength(GaitName step)
    {
        return gaits[step].AnimLength;
    }
    
    public void Move()
    {
        currentGait.Move();
    }

    void GaitFunctionCall()
    {
        currentGait.GaitFunction();
    }
    
    public void ChangeGait(GaitName? gait)
    {
        if (gait != null)
        {
            currentGait.Abort();
            currentGait = gaits[(GaitName)gait];
            currentGait.StartMoving();
            GaitFunctionCall();
        }   
    }

    public void ContinueGait()
    {
        GaitFunctionCall();
    }

    /*public float DeriveCurrentSpeedFromEndurance(float speed, float curEndurance, float maxEndurance, float personalStamina)
    {
        return (currentGait.CalculateSpeedCoefFromEndurance(curEndurance, maxEndurance) * speed * personalStamina);
    }*/

    public float DeriveCurrentEnduranceFromSpeed(float endurance)
    {
        //return personalStamina*currentGait.GetNewSpeedFromEndurance(endurancePercent);
        return personalStamina * currentGait.GetEnduranceDrop(endurance);
    }

    public float StepExpectedSpeed(GaitName gait, float endurancePercent)
    {
        return gaits[gait].GetNewSpeedFromEndurance(endurancePercent);
    }

    public void StartMoving()
    {
        currentGait.StartMoving();
    }
}
