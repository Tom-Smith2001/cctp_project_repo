using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Code written by Tom Smith - Thomas19.Smith@live.uwe.ac.uk

public class HomeTravelNode : Node
{
    private AgentStats my_stats;


    //constructor
    public HomeTravelNode(AgentStats my_stats)
    {
        this.my_stats = my_stats;
    }

    //evaluate function
    public override state Eval()
    {
        //if not travelling or not supposed to be headed home, return failed.
        if (!my_stats.travelling || !my_stats.due_home)
        {
            return state.failed;
        }
        else 
        {
            //dont go home if its on fire
            if (my_stats.home.GetComponent<HouseScript>().fire) 
            {
                my_stats.due_home = false;
                return state.failed;
            }
            //else set navmesh destination to home and return passed once they arrive
            my_stats.gameObject.GetComponent<NavMeshAgent>().destination = my_stats.home.GetComponent<HouseScript>().entrance.transform.position;
            if (Vector3.Distance(my_stats.home.GetComponent<HouseScript>().entrance.transform.position, my_stats.gameObject.transform.position) < 1)
            {
                my_stats.home.GetComponent<HouseScript>().occupants.Add(my_stats.gameObject);
                my_stats.at_home = true;
                return state.passed;
            }
            else 
            {
                return state.in_process;            
            }
        }

    }
}
