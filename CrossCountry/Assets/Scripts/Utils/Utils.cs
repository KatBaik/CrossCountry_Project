using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Utils
{
    public static string GetFormattedTime(double seconds)
    {
        string answer = "";
        TimeSpan timeFormatted = TimeSpan.FromSeconds(seconds);
        answer += timeFormatted.ToString("hh':'mm':'ss");
        answer += ":";
        TimeSpan tertias = TimeSpan.FromSeconds((seconds - Math.Floor(seconds)) * 60);
        answer += tertias.ToString("ss");
        return answer;
    }
}
