using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GaitName;

//public delegate void MoveMethod();

public class Gait
{
    protected GaitName name;
    protected float maximum;
    protected float minimum;
    protected Gait? faster;
    protected Gait? slower;
    protected double enduranceDrop;
    protected float animLength;
    protected string animTrigger;
    protected Action moveMethod;
    protected float currentSpeed;

    protected Gait(float max, float min, double endur, Action method, float length, string trigger)
    {
        maximum = max;
        minimum = min;
        enduranceDrop = endur;
        animLength = length;
        animTrigger = trigger;
        moveMethod = method;
    }

    public Action GaitFunction
    {
        get { return moveMethod; }
    }

    public float AnimLength
    {
        get { return animLength; }
    }

    public string AnimTrigger
    {
        get { return animTrigger; }
    }

    public GaitName Name
    {
        get { return name; }
        protected set { name = value; }
    }

    public Gait? Faster
    {
        get { return faster; }
        set { faster = value; }
    }

    public Gait? Slower
    {
        get { return slower; }
        set { slower = value; }
    }

    public float Maximum
    {
        get { return maximum; }
    }

    public float Minimum
    {
        get { return minimum; }
    }

    public float CurrentSpeed
    {
        get { return currentSpeed; }
        set
        {
            if (value < minimum)
            {
                throw new TooSlowException();
            }
            else if (value > maximum)
            {
                throw new TooFastException();
            }
            else
            {
                currentSpeed = value;
            }
        }
    }

    public virtual void Move() { }

    public virtual void StartMoving() 
    {
        CalculateCurrentSpeed();
    }

    protected virtual void CalculateCurrentSpeed() 
    {
        currentSpeed = (this.maximum + this.minimum) / 2;
    }

    /*public float CalculateSpeedCoefFromEndurance(float curEndurance, float maxEndurance)
    {
        return (float)Math.Pow((double)(curEndurance / maxEndurance), enduranceDrop);
    }*/

    public virtual void Abort()
    {
        currentSpeed = -1;
    }

    public virtual float GetEnduranceDrop(float endurance)
    {
        return endurance;
    }

    public virtual float GetNewSpeedFromEndurance(float endurancePercent) 
    {
        return (this.maximum + this.minimum) / 2;
    }
}
