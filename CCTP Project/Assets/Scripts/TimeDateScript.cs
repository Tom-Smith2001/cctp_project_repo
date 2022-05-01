using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeDateScript : MonoBehaviour
{
    public int minute = 0;          //
    public int hour = 0;            //
    public WeekDay day = 0;         // Timer values and text to display
    public float elapsed_time = 0;  //
    public Text date_time;          //
    [Range(0,20)]
    public float timescale = 1; //timescale and slider to adjust it 
    public Slider slider;
    public GameObject eventPrefab; //events to spawn
    public static bool chaos; //mode chosen in main menu


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

    }

    // Update is called once per frame
    void Update()
    {
        day = (WeekDay)((int)day % 7); //weekdays need to loop from sunday back to monday
        elapsed_time += Time.deltaTime;
        if (elapsed_time > 5 / timescale) //every 5 seconds at 1x speed, the time increases by 5 mins, and a random event could spawn
        {
            elapsed_time = 0;
            minute += 5;
            PossibleEvent();
            UpdateText();
        }
        if(minute >= 60) //looping hours
        {
            minute = 0;
            hour += 1;
            UpdateText();
        }
        if (hour >= 24) //looping days
        {
            hour = 0;
            day += 1;
            UpdateText();
        }
        
        
    }

    void UpdateText() //sets the timer text on UI
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

    public void PossibleEvent() // chance to spawn a random event or start a fire based on the gamemode and time of day
    {
        int x = Random.Range(0, 5000);
        if (TimeDateScript.chaos) //twice as likely in chaos mode
        {
            x /= 2;                    
        }
        
        if (hour > 21 || hour < 4) // at night, five times more likely
        {
            if (x < 100)
            {
                Debug.Log("Random Event Triggered.");
                if (x < 25)
                {
                    GameObject.FindGameObjectsWithTag("House")[Random.Range(0, GameObject.FindGameObjectsWithTag("House").Length)].GetComponent<HouseScript>().fire = true; //housefire
                }
                else if (x < 50)
                {
                    GameObject.FindGameObjectsWithTag("Work")[Random.Range(0, GameObject.FindGameObjectsWithTag("Work").Length)].GetComponent<WorkScript>().fire = true; //fire in a workplace
                }
                else
                {
                    Vector3 location = new Vector3();
                    location = GameObject.FindGameObjectsWithTag("Spawn")[Random.Range(0, GameObject.FindGameObjectsWithTag("Spawn").Length)].transform.position; //picks a random spawn point in the map
                    location -= new Vector3(0, 1, 0);
                    GameObject threat = GameObject.Instantiate(eventPrefab, location, Quaternion.identity); //spawns an event

                }
            }
            else 
            {
                Debug.Log("Random Event Not Triggered.");
            }
        }
        else //same as above but less likely
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
            else //no event was triggered
            {
                Debug.Log("Random Event Not Triggered.");
            }
        }
    }

    public void UpdateTimescale() 
    {
        timescale = slider.value; //slider dictates the timescale
    }
}
