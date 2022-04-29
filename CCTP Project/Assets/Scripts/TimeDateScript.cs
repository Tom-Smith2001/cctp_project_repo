using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeDateScript : MonoBehaviour
{
    public int minute = 0;
    public int hour = 0;
    public WeekDay day = 0;
    public float elapsed_time = 0;
    public Text date_time;
    [Range(0,10)]
    public float timescale = 1;


    public enum WeekDay
    {
        MON,
        TUE,
        WED,
        THU,
        FRI,
        SAT,
        SUN    
    }


    // Start is called before the first frame update
    void Start()
    {

        UpdateText();

    }

    // Update is called once per frame
    void Update()
    {
        day = (WeekDay)((int)day % 7);
        elapsed_time += Time.deltaTime;
        if (elapsed_time > 20 / timescale) 
        {
            elapsed_time = 0;
            minute += 10;
            UpdateText();
        }
        if(minute >= 60) 
        {
            minute = 0;
            hour += 1;
            UpdateText();
        }
        if (hour >= 24) 
        {
            hour = 0;
            day += 1;
            UpdateText();
        }
        
        
    }

    void UpdateText() 
    {
        if (hour < 10 && minute < 10)
        {
            date_time.text = day + ", 0" + hour + ":0" + minute;

        }
        else if (hour < 10)
        {
            date_time.text = day + ", 0" + hour + ":" + minute;

        }
        else if (minute < 10)
        {
            date_time.text = day + ", " + hour + ":0" + minute;

        }
        else
        {
            date_time.text = day + ", " + hour + ":" + minute;

        }
    }
}
