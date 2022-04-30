using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPanicNode : Node
{
    private AgentStats my_stats;

    public CheckPanicNode(AgentStats my_stats)
    {
        this.my_stats = my_stats;
    }

    public override state Eval()
    {
        if (my_stats.currentPanic < my_stats.composure) 
        {
            my_stats.panicked = false;        
        }
        if (my_stats.panicked)
        {
            return state.passed;
        }
        else 
        {
            return state.failed;        
        }
    }
}
