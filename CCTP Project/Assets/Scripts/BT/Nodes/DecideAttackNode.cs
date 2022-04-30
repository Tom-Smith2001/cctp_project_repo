using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecideAttackNode : Node
{
    private AgentStats my_stats;

    public DecideAttackNode(AgentStats my_stats)
    {
        this.my_stats = my_stats;
    }
    public override state Eval()
    {
        //cannot attack injured agents
        if (my_stats.attack_target != null && my_stats.attack_target.GetComponent<AgentStats>().injured)
        {
            my_stats.attacking = false;
        }
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