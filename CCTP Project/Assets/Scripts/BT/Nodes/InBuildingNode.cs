using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Code written by Tom Smith - Thomas19.Smith@live.uwe.ac.uk

public class InBuildingNode : Node
{
    private AgentStats my_stats;


    //constructor
    public InBuildingNode(AgentStats my_stats)
    {
        this.my_stats = my_stats;
    }


    //evaluation function
    public override state Eval()
    {
        GameObject agent = my_stats.gameObject;
        NavMeshAgent nav = my_stats.gameObject.GetComponent<NavMeshAgent>();

        //if the agent is at home but is completely calm and shouldnt be sleeping, they will leave
        if (my_stats.at_home && my_stats.currentPanic == 0 && !my_stats.due_home) 
        {
            my_stats.at_home = false;
            my_stats.home.GetComponent<HouseScript>().occupants.Remove(agent);
        }
        //the same check but for working
        if (my_stats.at_work && my_stats.currentPanic == 0 && !my_stats.due_work)
        {
            my_stats.at_work = false;
            my_stats.work_place.GetComponent<WorkScript>().occupants.Remove(agent);
        }
        //if at work, and a fire starts, do a fortitude check to determine if the agent gets injured while they evacuate, either way leave and become panicked or injured.
        if (my_stats.at_work && my_stats.work_place.GetComponent<WorkScript>().fire) 
        {
            if (Random.Range(0, 20) > my_stats.fortitude)
            {
                my_stats.injured = true;
                my_stats.at_work = false;
                my_stats.work_place.GetComponent<WorkScript>().occupants.Remove(agent);
            }
            else 
            {
                my_stats.currentPanic = Random.Range(my_stats.composure, 20);
                my_stats.at_work = false;
                my_stats.work_place.GetComponent<WorkScript>().occupants.Remove(agent);
            }
        }
        //same as above but for at home
        if (my_stats.at_home && my_stats.home.GetComponent<HouseScript>().fire)
        {
            if (Random.Range(0, 20) > my_stats.fortitude)
            {
                my_stats.injured = true;
                my_stats.at_home = false;
                my_stats.home.GetComponent<HouseScript>().occupants.Remove(agent);
            }
            else
            {
                my_stats.currentPanic = Random.Range(my_stats.composure, 20);
                my_stats.at_home = false;
                my_stats.home.GetComponent<HouseScript>().occupants.Remove(agent);
            }
        }

        //if they are at home or work and should be, return passed, else return failed
        if (my_stats.at_work || my_stats.at_home)
        {
            nav.enabled = false;
            agent.GetComponent<MeshRenderer>().enabled = false;
            agent.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            agent.transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
            return state.passed;
        }
        else 
        {
            nav.enabled = true;
            agent.GetComponent<MeshRenderer>().enabled = true;
            agent.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
            agent.transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = true;
            return state.failed;        
        }
    }

    
}
