using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GaitName;

public class Standing : Gait
{

    public Standing(float max, float min, double endur, Action method, float length, string trigger) : base(max, min, endur, method, length, trigger)
    {
        this.Slower = null;
        this.Name = GaitName.Standing;
        this.currentSpeed = 0;
    }

    public override void Move()
    {

    }

    public override void StartMoving()
    {
        this.currentSpeed = 0;
    }

    public override float GetNewSpeedFromEndurance(float endurancePercent)
    {
        return 0;
    }
}
