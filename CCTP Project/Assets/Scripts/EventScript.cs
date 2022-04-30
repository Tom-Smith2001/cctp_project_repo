using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventScript : MonoBehaviour
{
    public float timer = 60;
    GameObject gm;
    public LayerMask agents;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager");
        //re-bake navmesh so agents dont walk through events
        if (TimeDateScript.chaos)
        {
            timer *= 2;
        }
        
    }

    public void InjureNearby() 
    {
        foreach (Collider a in Physics.OverlapSphere(this.transform.position, 15, agents))
        {
            Debug.Log(a.name);
            GameObject agent = a.gameObject;
            if (agent.GetComponent<AgentStats>().fortitude < Random.Range(0, 20)) 
            {
                agent.GetComponent<AgentStats>().injured = true;
            }        
        }


    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime * gm.GetComponent<TimeDateScript>().timescale;
        if (timer < 0) 
        {
            foreach (GameObject a in GameObject.FindGameObjectsWithTag("Agent")) 
            {
                if (a.GetComponent<AgentStats>().known_events.Contains(this.gameObject)) 
                {
                    a.GetComponent<AgentStats>().known_events.Remove(this.gameObject);
                    if (a.GetComponent<AgentStats>().current_warning == this.gameObject) 
                    {
                        a.GetComponent<AgentStats>().current_warning = null;
                    }
                }            
            }
            this.GetComponent<BoxCollider>().enabled = false;
            this.GetComponent<MeshRenderer>().enabled = false;
            Destroy(this.GetComponent<MeshFilter>().mesh);
            gm.GetComponent<AgentGen>().Bake();
            Destroy(this.gameObject);
        }
    }
}
