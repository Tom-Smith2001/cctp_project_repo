using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Code written by Tom Smith - Thomas19.Smith@live.uwe.ac.uk

public class TryHelpNode : Node
{
    private AgentStats my_stats;
    private GameObject target;


    //constructor where variables are passed
    public TryHelpNode(AgentStats my_stats)
    {
        this.my_stats = my_stats;
        this.target = my_stats.helpTarget;
    }

    //evaluation function
    public override state Eval()
    {
        this.target = my_stats.helpTarget;
        if (target == null || !target.GetComponent<AgentStats>().injured)
        {
            if (!target.GetComponent<AgentStats>().injured) 
            {
                my_stats.StopHeal(target);            
            }
            my_stats.helping = false;
            return state.failed;
        }
        else 
        {
            //If the agent has a temprerament of 8 or more, and the injured agent is their friend, they will always help them regardless of the danger it poses.
            if (!my_stats.friends.Contains(target) || my_stats.temperament <= 8) 
            {
                //If the injured agent is too close to an active event then give up helping them.
                foreach (GameObject e in my_stats.known_events)
                {
                    if (Vector3.Distance(target.transform.position, e.transform.position) < (20 - my_stats.bravery))
                    {
                        my_stats.helping = false;
                        return state.failed;
                    }
                }
            }
            //return in process if they still havent reached their help target
            if (Vector3.Distance(my_stats.gameObject.transform.position, target.transform.position) > 3)
            {
                my_stats.GetComponent<NavMeshAgent>().destination = target.transform.position;
                return state.in_process;
            }
            else 
            {
                //help them when they reach them if they still need it
                if (target.GetComponent<AgentStats>().injured && !target.GetComponent<AgentStats>().helped) 
                {
                    my_stats.Heal(target);             
                }                
                if (!target.GetComponent<AgentStats>().injured)
                {
                    my_stats.StopHeal(target);
                    return state.passed;
                }
                else 
                {
                    return state.in_process;                
                }
            }
        }

    }




}