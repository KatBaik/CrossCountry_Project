using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : Gait
{
    public Jump(float max, float min, double endur, Action method, float length, string trigger) : base(max, min, endur, method, length, trigger)
    {
        this.Name = GaitName.Jump;
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
        this.currentSpeed = this.maximum;
    }

    public override float GetEnduranceDrop(float endurance)
    {
        return Mathf.Max(0, 0.995f*endurance-5);
    }

    public override float GetNewSpeedFromEndurance(float endurancePercent)
    {
        return (this.maximum+this.minimum)/2;
    }
}
