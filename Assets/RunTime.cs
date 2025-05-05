using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunTime : MonoBehaviour
{
    private float startTime;
    private float elapsedTime;
    private TimeSpan ts;

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        elapsedTime = Time.time - startTime;
        
        ts = TimeSpan.FromSeconds(elapsedTime);
        Debug.Log(string.Format("{0:00}:{1:00}", ts.TotalMinutes, ts.Seconds));
    }

    public string GetElapsedTime()
    {
        return string.Format("{0:00}:{1:00}", ts.TotalMinutes, ts.Seconds);
    }
}
