using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Code written by Tom Smith - Thomas19.Smith@live.uwe.ac.uk

public class DecideAttackNode : Node
{
    private AgentStats my_stats;

    //constructor
    public DecideAttackNode(AgentStats my_stats)
    {
        this.my_stats = my_stats;
    }

    //evaluate function
    public override state Eval()
    {
        //cannot attack injured agents
        if (my_stats.attack_target != null && my_stats.attack_target.GetComponent<AgentStats>().injured)
        {
            my_stats.attacking = false;
        }
        //if not attacking or noone to attack return failed else pass
        if (!my_stats.attacking || my_stats.attack_target == null)
        {
            return state.failed;
        }
        else
        {
            return state.passed;
        }

    }


}