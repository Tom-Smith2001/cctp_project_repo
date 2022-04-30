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
    [Range(0,20)]
    public float timescale = 1;
    public Slider slider;
    public GameObject eventPrefab;
    public static bool chaos;


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
        slider.value = timescale;
        UpdateText();
        Debug.Log(chaos);

    }

    // Update is called once per frame
    void Update()
    {
        day = (WeekDay)((int)day % 7);
        elapsed_time += Time.deltaTime;
        if (elapsed_time > 5 / timescale) 
        {
            elapsed_time = 0;
            minute += 5;
            PossibleEvent();
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

    public void PossibleEvent() 
    {
        int x = Random.Range(0, 5000);
        if (TimeDateScript.chaos) 
        {
            x /= 2;                    
        }
        
        if (hour > 21 || hour < 4)
        {
            if (x < 100)
            {
                Debug.Log("Random Event Triggered.");
                if (x < 25)
                {
                    GameObject.FindGameObjectsWithTag("House")[Random.Range(0, GameObject.FindGameObjectsWithTag("House").Length)].GetComponent<HouseScript>().fire = true;
                }
                else if (x < 50)
                {
                    GameObject.FindGameObjectsWithTag("Work")[Random.Range(0, GameObject.FindGameObjectsWithTag("Work").Length)].GetComponent<WorkScript>().fire = true;
                }
                else
                {
                    Vector3 location = new Vector3();
                    location = GameObject.FindGameObjectsWithTag("Spawn")[Random.Range(0, GameObject.FindGameObjectsWithTag("Spawn").Length)].transform.position;
                    location -= new Vector3(0, 1, 0);
                    GameObject threat = GameObject.Instantiate(eventPrefab, location, Quaternion.identity);

                }
            }
            else 
            {
                Debug.Log("Random Event Not Triggered.");
            }
        }
        else 
        {
            if (x < 20)
            {
                Debug.Log("Random Event Triggered.");
                if (x < 5)
                {
                    GameObject.FindGameObjectsWithTag("House")[Random.Range(0, GameObject.FindGameObjectsWithTag("House").Length)].GetComponent<HouseScript>().fire = true;
                }
                else if (x < 10)
                {
                    GameObject.FindGameObjectsWithTag("Work")[Random.Range(0, GameObject.FindGameObjectsWithTag("Work").Length)].GetComponent<WorkScript>().fire = true;
                }
                else
                {
                    Vector3 location = new Vector3();
                    location = GameObject.FindGameObjectsWithTag("Spawn")[Random.Range(0, GameObject.FindGameObjectsWithTag("Spawn").Length)].transform.position;
                    location -= new Vector3(0, 1, 0);
                    GameObject threat = GameObject.Instantiate(eventPrefab, location, Quaternion.identity);

                }
            }
            else
            {
                Debug.Log("Random Event Not Triggered.");
            }
        }
    }

    public void UpdateTimescale() 
    {
        timescale = slider.value;
    }
}
