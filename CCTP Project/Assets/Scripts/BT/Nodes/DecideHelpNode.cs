using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecideHelpNode : Node
{
    private AgentStats my_stats;

    public DecideHelpNode(AgentStats my_stats)
    {
        this.my_stats = my_stats;
    }
    public override state Eval()
    {
        if (my_stats.helpTarget != null && my_stats.helpTarget.GetComponent<AgentStats>().injured) 
        {
            if (my_stats.temperament >= 8 || !my_stats.enemies.Contains(my_stats.helpTarget))
            {
                my_stats.helping = true;                            
            }                    
        }
        if (!my_stats.helping)
        {
            my_stats.helping = false;
            return state.failed;
        }
        else 
        {
            return state.passed;
        }

    }

    
}
