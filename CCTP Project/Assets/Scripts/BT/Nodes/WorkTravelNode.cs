using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorkTravelNode : Node
{
    private AgentStats my_stats;

    public WorkTravelNode(AgentStats my_stats)
    {
        this.my_stats = my_stats;
    }
    public override state Eval()
    {
        if (!my_stats.travelling || !my_stats.due_work)
        {
            return state.failed;
        }
        else
        {
            if (my_stats.work_place.GetComponent<WorkScript>().fire)
            {
                my_stats.due_work = false;
                return state.failed;
            }
            my_stats.gameObject.GetComponent<NavMeshAgent>().destination = my_stats.work_place.GetComponent<WorkScript>().entrance.transform.position;
            if (Vector3.Distance(my_stats.work_place.GetComponent<WorkScript>().entrance.transform.position, my_stats.gameObject.transform.position) < 1)
            {
                my_stats.work_place.GetComponent<WorkScript>().occupants.Add(my_stats.gameObject);
                my_stats.at_work = true;
                return state.passed;
            }
            else
            {
                return state.in_process;
            }
        }

    }
}