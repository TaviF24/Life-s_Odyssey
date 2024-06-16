using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class DayTimeController : MonoBehaviour
{
    const float secondsInDay = 86400f; // seconds in a day(24h*60m*60s)
    const float phaseLength = 900f; // 15 minute chunks

    [SerializeField] Color nightLightColor;
    [SerializeField] AnimationCurve nightTimeCurve;
    [SerializeField] Color dayLightColor = Color.white;
     
    float time;

    [SerializeField] float startAtTime = 28800f; // start time in seconds (8:00 AM)
    [SerializeField] Text text;
    [SerializeField] float timeScale = 60f; // 1 in game second = 1 real time minute
    [SerializeField] Light2D globalLight;
    private int days;

    List<TimeAgent> agents;

    private void Awake()
    {
        agents = new List<TimeAgent>();
    }

    private void Start()
    {
        time = startAtTime;
    }

    public void Subscribe(TimeAgent timeAgent)
    {
        agents.Add(timeAgent);
    }

    public void Unsubscribe(TimeAgent timeAgent) 
    { 
        agents.Remove(timeAgent); 
    }

    float Hours
    {
        get
        {
            return time / 3600f; // get current time in hours
        }
    }

    float Minutes
    {
        get
        {
            return time % 3600f / 60f; // get current time in minutes
        }
    }

    private void Update()
    {
        time += Time.deltaTime * timeScale;
        TimeValueCalculation();
        DayLight(); // update the light based on the current time

        if (time > secondsInDay)
        {
            NextDay(); // check for a new day
        }

        TimeAgents();
    }

    
    private void TimeValueCalculation()
    {
        // display text on screen with current time
        int hh = (int)Hours;
        int mm = (int)Minutes;
        text.text = "Days: " + days.ToString() + " | " + hh.ToString("00") + ":" + mm.ToString("00") + ":" + days.ToString();
    }

    private void DayLight()
    {
        // calculate the dim value
        float v = nightTimeCurve.Evaluate(Hours);
        Color c = Color.Lerp(dayLightColor, nightLightColor, v);
        globalLight.color = c;
    }

    int oldPhase = 0;
    private void TimeAgents()
    {
        int currentPhase = (int)(time / phaseLength);

        if (oldPhase != currentPhase)
        {
            oldPhase = currentPhase;
            for (int i = 0; i < agents.Count; i++)
            {
                agents[i].Invoke();
            }
        }
    }


    private void NextDay()
    {
        time = 0;
        days += 1;
    }
}
