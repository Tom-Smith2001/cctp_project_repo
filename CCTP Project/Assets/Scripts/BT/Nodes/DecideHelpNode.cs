using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Code written by Tom Smith - Thomas19.Smith@live.uwe.ac.uk

public class DecideHelpNode : Node
{
    private AgentStats my_stats;


    //constructor
    public DecideHelpNode(AgentStats my_stats)
    {
        this.my_stats = my_stats;
    }

    //evaluate function
    public override state Eval()
    {
        //if the agent isnt already helping someone and the target is injured
        if (my_stats.helpTarget != null && my_stats.helpTarget.GetComponent<AgentStats>().injured) 
        {
            //agents with a temperament below 8 will not help an enemy
            if (my_stats.temperament >= 8 || !my_stats.enemies.Contains(my_stats.helpTarget))
            {
                my_stats.helping = true;                            
            }                    
        }
        // return failed if they're not going to help, else pass
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
