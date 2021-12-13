using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ObstaclesUtils;
using System;

public class Obstacle : MonoBehaviour
{
    public int idCode;
    public float height;
    public float width;
    int score;
    float requiredSpeedMin;
    float requiredSpeedMax;
    float requiredEndurance;
    float maxSkittishness;
    float requiredTrust;
    float requiredObedience;
    Color color;

    public int Id
    {
        get { return idCode; }
        set {
                if (idCode != null && value != null)
                {
                    idCode = value;
                }
            }
    }

    public float Height
    {
        get { return height; }
    }

    public float Width
    {
        get { return width; }
    }

    public int Score
    {
        get { return score; }
    }

    public Color Colour
    {
        get { return color; }
        set { color = value; }
    }

    public float RequiredSpeedMin
    {
        get { return requiredSpeedMin; }
    }

    public float RequiredSpeedMax
    {
        get { return requiredSpeedMax; }
    }

    public float RequiredEndurance
    {
        get { return requiredEndurance; }
    }

    public float MaxSkittishness
    {
        get { return maxSkittishness; }
    }

    public float RequiredTrust
    {
        get { return requiredTrust; }
    }

    public float RequiredObedience
    {
        get { return requiredObedience; }
    }

    public bool IsQualified(float horseSpeed, float horseMaxJumpHeight, float horseMaxJumpWidth, float horseEndurance)
    {
        /*Debug.Log(requiredSpeedMin);
        Debug.Log(requiredSpeedMax);
        Debug.Log(horseSpeed);
        Debug.Log(height);
        Debug.Log(horseMaxJumpHeight);
        Debug.Log(width);
        Debug.Log(horseMaxJumpWidth);
        Debug.Log(requiredEndurance);
        Debug.Log(horseEndurance);*/
        if (requiredSpeedMin<=horseSpeed && horseSpeed<=requiredSpeedMax && 
            horseMaxJumpHeight>=height && horseMaxJumpWidth>=width && horseEndurance >= requiredEndurance)
        {
            return true;
        }
        return false;
    }

    public bool IsAgreeable(float horseSkittishness, float horseObedience, float horseTrust)
    {
        if (requiredTrust<=horseTrust && requiredObedience<=horseObedience && maxSkittishness > horseSkittishness)
        {
            return true;
        }
        return false;
    }

    void SetQualities()
    {
        score = ObstaclesUtils.Score(idCode);
        height = ObstaclesUtils.Height(idCode);
        width = ObstaclesUtils.Width(idCode);
        requiredSpeedMin = ObstaclesUtils.RequiredSpeedMin(idCode);
        requiredSpeedMax = ObstaclesUtils.RequiredSpeedMax(idCode);
        requiredEndurance = ObstaclesUtils.RequiredEndurance(idCode);
        maxSkittishness = ObstaclesUtils.MaxSkittishness(idCode);
        requiredTrust = ObstaclesUtils.RequiredTrust(idCode);
        requiredObedience = ObstaclesUtils.RequiredObedience(idCode);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    public void PostStart()
    {
        ObstaclesUtils.Initialize();
        SetQualities();
        if (height > 0)
        {
            transform.localScale = new Vector3(transform.localScale.x, height, transform.localScale.z);
        }
        else
        {
            height = transform.localScale.y;
        }
        if (width > 0)
        {
            transform.localScale = new Vector3(width, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            width = transform.localScale.x;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
