using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseScript : MonoBehaviour
{
    public List<GameObject> tennants;
    public List<GameObject> occupants;
    public GameObject entrance;
    public bool fire;
    public float fire_timer = 60;
    public GameObject gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager");
        if (TimeDateScript.chaos)
        {
            fire_timer *= 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (fire)
        {
            if (fire_timer < 0)
            {
                fire = false;
                fire_timer = 60;
            }
            else 
            {
                fire_timer -= Time.deltaTime * gm.GetComponent<TimeDateScript>().timescale;         
            }

            this.gameObject.layer = LayerMask.NameToLayer("Event");
            this.gameObject.tag = "Event";
            this.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
            foreach (GameObject o in occupants) 
            {
                if (!o.GetComponent<AgentStats>().injured) 
                {
                    if (Random.Range(0, 20) > o.GetComponent<AgentStats>().fortitude)
                    {
                        o.GetComponent<AgentStats>().injured = true;
                        o.GetComponent<AgentStats>().at_home = false;
                    }
                    else
                    {
                        o.GetComponent<AgentStats>().currentPanic = 20;
                        o.GetComponent<AgentStats>().known_events.Add(o.GetComponent<AgentStats>().home);
                        o.GetComponent<AgentStats>().at_home = false;
                    }
                }         
            }
        }
        else 
        {
            this.gameObject.layer = LayerMask.NameToLayer("Wall");
            this.gameObject.tag = "House";
            this.gameObject.GetComponent<MeshRenderer>().material.color = Color.grey;
        }
    }
}
