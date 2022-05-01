using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Code written by Tom Smith - Thomas19.Smith@live.uwe.ac.uk

public class WorkScript : MonoBehaviour
{
    public List<GameObject> employees; // all agents that work here
    public List<GameObject> occupants; // agents currently in the building
    public GameObject entrance; // the empty object used to mark the entrance in world space
    public bool fire; //if the buildings on fire
    public float fire_timer = 60; // how long a fire will last
    public GameObject gm; // the game manager used in timescale checking

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager");
        if (TimeDateScript.chaos)
        {
            fire_timer *= 2; //in chaos mode all fires last twice as long
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if on fire
        if (fire)
        {
            //fire goes out once timer hits 0, and the timer is reset
            if (fire_timer < 0)
            {
                fire = false;
                fire_timer = 60;
            }
            else
            {
                //timer decreases based on timescale
                fire_timer -= Time.deltaTime * gm.GetComponent<TimeDateScript>().timescale;
            }
            this.gameObject.layer = LayerMask.NameToLayer("Event"); //building becomes an event when on fire
            this.gameObject.tag = "Event";                          //building becomes an event when on fire
            this.gameObject.GetComponent<MeshRenderer>().material.color = Color.red; //while on fire buildings are red
            foreach (GameObject o in occupants)
            {
                //kick out each occupant, injuring them if they lose a fortitude check
                if (!o.GetComponent<AgentStats>().injured)
                {
                    if (Random.Range(0, 20) > o.GetComponent<AgentStats>().fortitude)
                    {
                        o.GetComponent<AgentStats>().injured = true;
                        o.GetComponent<AgentStats>().at_work = false;
                    }
                    else
                    {
                        o.GetComponent<AgentStats>().currentPanic = 20;
                        o.GetComponent<AgentStats>().known_events.Add(o.GetComponent<AgentStats>().home);
                        o.GetComponent<AgentStats>().at_work = false;
                    }
                }
            }
        }
        else
        {
            //reset building to usual tag layer and colour
            this.gameObject.layer = LayerMask.NameToLayer("Wall");
            this.gameObject.tag = "Work";
            this.gameObject.GetComponent<MeshRenderer>().material.color = Color.grey;
        }
    }
}
