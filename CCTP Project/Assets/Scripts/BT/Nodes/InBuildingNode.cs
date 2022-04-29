using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InBuildingNode : Node
{
    private AgentStats my_stats;

    public InBuildingNode(AgentStats my_stats)
    {
        this.my_stats = my_stats;
    }

    public override state Eval()
    {
        GameObject agent = my_stats.gameObject;
        NavMeshAgent nav = my_stats.gameObject.GetComponent<NavMeshAgent>();

        if (my_stats.at_home && my_stats.currentPanic == 0 && !my_stats.due_home) 
        {
            my_stats.at_home = false;
        }

        if (my_stats.at_work && my_stats.currentPanic == 0 && !my_stats.due_work)
        {
            my_stats.at_work = false;
        }

        if (my_stats.at_work && my_stats.work_place.GetComponent<WorkScript>().fire) 
        {
            if (Random.Range(0, 20) > my_stats.fortitude)
            {
                my_stats.injured = true;
                my_stats.at_work = false;
            }
            else 
            {
                my_stats.currentPanic = Random.Range(my_stats.composure, 20);
                my_stats.at_work = false;
            }
        }

        if (my_stats.at_home && my_stats.home.GetComponent<HouseScript>().fire)
        {
            if (Random.Range(0, 20) > my_stats.fortitude)
            {
                my_stats.injured = true;
                my_stats.at_home = false;
            }
            else
            {
                my_stats.currentPanic = Random.Range(my_stats.composure, 20);
                my_stats.at_home = false;
            }
        }


        if (my_stats.at_work || my_stats.at_home)
        {
            nav.enabled = false;
            agent.GetComponent<MeshRenderer>().enabled = false;
            agent.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            return state.passed;
        }
        else 
        {
            nav.enabled = true;
            agent.GetComponent<MeshRenderer>().enabled = true;
            agent.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
            return state.failed;        
        }
    }

    
}
