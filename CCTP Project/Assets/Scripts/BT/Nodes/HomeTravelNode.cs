using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HomeTravelNode : Node
{
    private AgentStats my_stats;

    public HomeTravelNode(AgentStats my_stats)
    {
        this.my_stats = my_stats;
    }
    public override state Eval()
    {
        if (!my_stats.travelling || !my_stats.due_home)
        {
            return state.failed;
        }
        else
        {
            if (my_stats.home.GetComponent<HouseScript>().fire) 
            {
                my_stats.due_home = false;
                return state.failed;
            }
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
