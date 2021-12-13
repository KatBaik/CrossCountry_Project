using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GaitName;

public class Trot : Gait
{
    Animation animation;

    public Trot(float max, float min, double endur, Action method, float length, string trigger) : base(max, min, endur, method, length, trigger) 
    {
        this.Name = GaitName.Trot;
        this.currentSpeed = -1;
    }

    public override void Move()
    {

    }

    public override void StartMoving()
    {
        if (this.currentSpeed < 0)
        {
            CalculateCurrentSpeed();
        }        
    }

    protected override void CalculateCurrentSpeed()
    {
        this.currentSpeed = (this.maximum + this.minimum) / 2;
    }

    public override float GetEnduranceDrop(float endurance)
    {
        return Mathf.Max(0, 0.9985f * endurance-1);
    }

    public override float GetNewSpeedFromEndurance(float endurancePercent)
    {
        return (this.maximum + this.minimum) / 2;
        //return (float) Math.Max((double)this.minimum, (double)(Math.Sqrt(endurancePercent) * (this.maximum + this.minimum) / 2));
    }
}
