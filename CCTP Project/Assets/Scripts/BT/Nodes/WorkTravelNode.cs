using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


//Code written by Tom Smith - Thomas19.Smith@live.uwe.ac.uk


public class WorkTravelNode : Node
{
    // the stats of the agent this is effecting
    private AgentStats my_stats;

    //constructor where the stats are passed in
    public WorkTravelNode(AgentStats my_stats)
    {
        this.my_stats = my_stats;
    }

    //evaluate function where the process is executed to determine if this node will attempt to govern agent behaviour, or return failed if it is invalid.
    public override state Eval()
    {
        //if the agent isnt travelling or shouldnt be at work, return fail
        if (!my_stats.travelling || !my_stats.due_work)
        {
            return state.failed;
        }
        else
        {
            // return fail if the work place is on fire
            if (my_stats.work_place.GetComponent<WorkScript>().fire)
            {
                my_stats.due_work = false;
                return state.failed;
            }
            //otherwise, set the navmesh target to the agents work place, and return passed when they arrive
            my_stats.gameObject.GetComponent<NavMeshAgent>().destination = my_stats.work_place.GetComponent<WorkScript>().entrance.transform.position;
            if (Vector3.Distance(my_stats.work_place.GetComponent<WorkScript>().entrance.transform.position, my_stats.gameObject.transform.position) < 1)
            {
                my_stats.work_place.GetComponent<WorkScript>().occupants.Add(my_stats.gameObject);
                my_stats.at_work = true;
                return state.passed;
            }
            else
            {
                //return in process if this is the correct node but the agent is still travelling to work.
                return state.in_process;
            }
        }

    }
}