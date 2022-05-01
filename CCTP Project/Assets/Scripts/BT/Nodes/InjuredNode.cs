using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Code written by Tom Smith - Thomas19.Smith@live.uwe.ac.uk

public class InjuredNode : Node
{
    private AgentStats my_stats;


    //constructor where variables are passed
    public InjuredNode(AgentStats my_stats)
    {
        this.my_stats = my_stats;
    }


    //evaluation function
    public override state Eval()
    {
        //if the agent is injured, disable the navmesh and panic them, returning passed.
        NavMeshAgent nav = my_stats.gameObject.GetComponent<NavMeshAgent>();
        if (my_stats.injured)
        {
            nav.enabled = false;
            my_stats.currentPanic = 20;
            return state.passed;
        }
        else //they are not injured, sp return failed
        {
            nav.enabled = true;
            return state.failed;        
        }
    }

    
}
