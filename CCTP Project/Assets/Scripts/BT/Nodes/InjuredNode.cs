using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InjuredNode : Node
{
    private AgentStats my_stats;

    public InjuredNode(AgentStats my_stats)
    {
        this.my_stats = my_stats;
    }

    public override state Eval()
    {
        NavMeshAgent nav = my_stats.gameObject.GetComponent<NavMeshAgent>();
        if (my_stats.injured)
        {
            nav.enabled = false;
            my_stats.currentPanic = 20;
            return state.passed;
        }
        else 
        {
            nav.enabled = true;
            return state.failed;        
        }
    }

    
}
