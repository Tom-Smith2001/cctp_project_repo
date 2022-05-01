using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Code written by Tom Smith - Thomas19.Smith@live.uwe.ac.uk

public class CheckPanicNode : Node
{
    private AgentStats my_stats;


    //constructor
    public CheckPanicNode(AgentStats my_stats)
    {
        this.my_stats = my_stats;
    }


    //evaluation function
    public override state Eval()
    {
        //start panicking if panic is high enough
        if (my_stats.currentPanic < my_stats.composure) 
        {
            my_stats.panicked = false;        
        }
        //pass if panicked fail if not
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
