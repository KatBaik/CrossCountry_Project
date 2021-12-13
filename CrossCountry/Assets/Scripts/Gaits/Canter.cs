using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GaitName;

public class Canter : Gait
{
    public Canter(float max, float min, double endur, Action method, float length, string trigger) : base (max, min, endur, method,length, trigger)
    {
        this.Faster = null;
        this.Name = GaitName.Canter;
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
        return Mathf.Max(0, 0.997f * endurance-3);
    }

    public override float GetNewSpeedFromEndurance(float endurancePercent) 
    {
        return (this.maximum + this.minimum) / 2;
        //return (float)Math.Max((double)this.minimum, (double)(endurancePercent *(this.maximum + this.minimum) / 2));
    }
}
