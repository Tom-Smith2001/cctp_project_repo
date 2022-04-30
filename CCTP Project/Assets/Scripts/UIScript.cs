using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour
{
    public GameObject highlighted_object;
    public GameObject stats_panel;
    public GameObject routine_panel;
    public GameObject HUD;
    public GameObject PauseScreen;
    public bool paused;
    public Text stats_name;
    public Text stats_name2;
    public Text stats_info;
    public Text stats_info2;
    public Text routine_info;
    public Text routine_info2;
    public Text time_date;

    // Start is called before the first frame update
    void Start()
    {
        paused = false;
        stats_panel.SetActive(false);
        routine_panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused && PauseScreen.activeInHierarchy) 
        {
            Unpause();        
        }
        if (paused && !PauseScreen.activeInHierarchy) 
        {
            Pause();        
        }
        if (Input.GetKeyUp(KeyCode.Escape)) 
        {
            if (!paused)
            {
                Pause();
            }
            else 
            {
                Unpause();            
            }
        }
        double ts = System.Math.Round(this.GetComponent<TimeDateScript>().timescale, 2);
        time_date.text = "Timescale = " + ts + "X";
        if (highlighted_object == null) 
        {
            stats_panel.SetActive(false);
            routine_panel.SetActive(false);
            return;
        }
        if (highlighted_object.tag == "Agent")
        {
            AgentStats ags = highlighted_object.GetComponent<AgentStats>();
            stats_name.text = highlighted_object.name;
            stats_name2.text = "Relationships: ";
            stats_info.text = "Stats: \nCOMPOSURE: " + ags.composure + " \nOBSERVATION: " + ags.observation + " \nFORTITUDE: " + ags.fortitude + " \nBRAVERY: " + ags.bravery + " \nTEMPERAMENT: " + ags.temperament + " \nSPEED: " + ags.speed + " \nCURRENT PANIC: " + ags.currentPanic + " \nSTRESS: " + ags.stress + " \nHAPPINESS: " + ags.happiness;
            stats_info2.text = "Friends: \n";
            if (ags.friends.Count > 5 || ags.enemies.Count > 5) 
            {
                stats_info2.fontSize = 14;            
            }
            foreach (GameObject friend in ags.friends)
            {
                stats_info2.text = stats_info2.text + friend.name + ", ";
            }
            stats_info2.text = stats_info2.text + ". \nEnemies: \n ";
            foreach (GameObject enemy in ags.enemies)
            {
                stats_info2.text = stats_info2.text + enemy.name + ", ";
            }


            routine_info2.text = "This agent sleeps between ";

            if (ags.night_shift)
            {
                routine_info.text = "Works night shifts, 16:00 - 00:00 \nWork Days: \n";
                routine_info2.text = routine_info2.text + "03:00 and 11:00.";
            }
            else
            {
                routine_info.text = "Works day shifts, 09:00 - 17:00 \nWork Days: \n";

                routine_info2.text = routine_info2.text + "23:00 and 07:00.";
            }

            foreach (TimeDateScript.WeekDay day in ags.work_days)
            {
                routine_info.text = routine_info.text + day + ", ";
            }





            stats_panel.SetActive(true);
            routine_panel.SetActive(true);
        }
        else if (highlighted_object.tag == "Work" || highlighted_object.tag == "House" || highlighted_object.tag == "Event")
        {
            stats_name.text = highlighted_object.name;
            if (highlighted_object.GetComponent<WorkScript>()!= null && highlighted_object.GetComponent<WorkScript>().fire) 
            {
                stats_name.text = stats_name.text + " (ON FIRE!)";
            }
            else if (highlighted_object.GetComponent<HouseScript>() != null && highlighted_object.GetComponent<HouseScript>().fire)
            {
                stats_name.text = stats_name.text + " (ON FIRE!)";
            }
            stats_info.text = "Current Occupants: \n";
            stats_info2.text = "";
            if (highlighted_object.GetComponent<WorkScript>() != null)
            {
                stats_name2.text = "Employees: \n";
                foreach (GameObject o in highlighted_object.GetComponent<WorkScript>().occupants)
                {
                    stats_info.text = stats_info.text + o.name + ", ";
                }
                foreach (GameObject e in highlighted_object.GetComponent<WorkScript>().employees)
                {
                    stats_info2.text = stats_info2.text + e.name + ", ";
                }
            }
            else
            {
                stats_name2.text = "Tennants: \n";
                foreach (GameObject o in highlighted_object.GetComponent<HouseScript>().occupants)
                {
                    stats_info.text = stats_info.text + o.name + ", ";
                }
                foreach (GameObject t in highlighted_object.GetComponent<HouseScript>().tennants)
                {
                    stats_info2.text = stats_info2.text + t.name + ", ";
                }
            }
            stats_panel.SetActive(true);
            routine_panel.SetActive(false);
        }
        else 
        {
            stats_panel.SetActive(false);
            routine_panel.SetActive(false);
        }
    }
    public void Pause()
    {
        paused = true;
        HUD.SetActive(false);
        PauseScreen.SetActive(true);
    }
    public void Unpause() 
    {
        paused = false;
        HUD.SetActive(true);
        PauseScreen.SetActive(false);
    }

    public void MainMenu() 
    {

        SceneManager.LoadScene(0);
        paused = false;
        HUD.SetActive(true);
        PauseScreen.SetActive(false);
    }
}
