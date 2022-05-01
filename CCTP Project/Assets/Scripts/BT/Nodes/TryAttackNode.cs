using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


//Code written by Tom Smith - Thomas19.Smith@live.uwe.ac.uk

public class TryAttackNode : Node
{
    private AgentStats my_stats;
    private GameObject target;

    //constructor where variables are passed
    public TryAttackNode(AgentStats my_stats)
    {
        this.my_stats = my_stats;
        this.target = my_stats.attack_target;
    }

    //evaluation function
    public override state Eval()
    {
        //sets the target, but if they're already injured then do not attack.
        this.target = my_stats.attack_target;
        if (target == null || target.GetComponent<AgentStats>().injured)
        {
            
            my_stats.attacking = false;
            return state.failed;
        }
        else
        {
            //if the attack target is too close a known event based on this agents bravery, then they wont attack them.
            foreach (GameObject e in my_stats.known_events)
            {
                if (Vector3.Distance(target.transform.position, e.transform.position) < (20 - my_stats.bravery))
                {
                    my_stats.attacking = false;
                    return state.failed;
                }
            }

            //move to and then attack the target
            if (Vector3.Distance(my_stats.gameObject.transform.position, target.transform.position) > 3)
            {
                my_stats.GetComponent<NavMeshAgent>().destination = target.transform.position;
                return state.in_process;
            }
            else
            {

                Attack(target);
                return state.passed;
            }
        }

    }

    private void Attack(GameObject victim) 
    {
        //attacker will have an advantage, but the fight is decided by the agents' fortitude stat.
        if (my_stats.fortitude * 1.25 > victim.GetComponent<AgentStats>().fortitude)
        {
            victim.GetComponent<AgentStats>().injured = true;
        }
        else 
        {
            my_stats.injured = false;        
        }

        //any agent that could see the victim when they were attacked will immediately consider the attacker an enemy, unless said agent has a low temperament and they were previously friends.
        foreach (Collider agent in Physics.OverlapSphere(victim.transform.position, 20f, LayerMask.NameToLayer("Agents"))) 
        {
            if (agent.gameObject.GetComponent<ReactScript>().last_seen != null && agent.gameObject.GetComponent<ReactScript>().last_seen.Contains(victim)) 
            {
                if (agent.gameObject.GetComponent<AgentStats>().temperament > 6 || !agent.gameObject.GetComponent<AgentStats>().friends.Contains(my_stats.gameObject)) 
                {

                    agent.gameObject.GetComponent<AgentStats>().enemies.Add(my_stats.gameObject);

                }
            }        
        }
    }




}