using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WarnNode : Node 
{
    private AgentStats my_stats;
    private GameObject agent;
    private GameObject warning;

    public WarnNode(AgentStats my_stats, GameObject agent)
    {
        this.my_stats = my_stats;
        this.agent = agent;
        this.warning = this.my_stats.current_warning;
    }

    public override state Eval()
    {
        warning = my_stats.current_warning;
        if (warning == null && my_stats.known_events.Count > 0)
        {
            warning = my_stats.known_events[my_stats.known_events.Count - 1];
        }
        if (warning == null)
        {
            Debug.Log(my_stats.gameObject.name + " had nothing to warn their friends about, they are running home instead.");
            return state.failed;
        }
        else if (warning.GetComponent<AgentStats>() == null)
        {

            return EventWarning();

        }
        else
        {
            return InjuredWarning();
        }
    }

    private state EventWarning()
    {
        if ((my_stats.bravery + my_stats.temperament) / 2 < my_stats.currentPanic || my_stats.friends.Count == 0)
        {
            Debug.Log(my_stats.gameObject.name + " decided not to warn any of their friends, they are running home instead.");
            
            return state.failed;
        }
        else
        {
            foreach (GameObject friend in my_stats.friends)
            {
                while (!friend.GetComponent<AgentStats>().known_events.Contains(warning) && !friend.GetComponent<AgentStats>().at_work && !friend.GetComponent<AgentStats>().at_home)
                {
                    Debug.Log(my_stats.gameObject.name + " is trying to warn " + friend.name + " about " + warning.name);
                    if (Vector3.Distance(agent.transform.position, friend.transform.position) > 3)
                    {
                        agent.GetComponent<NavMeshAgent>().destination = friend.transform.position;
                        return state.in_process;
                    }
                    else
                    {
                        friend.GetComponent<AgentStats>().known_events.Add(warning);
                        Debug.Log(my_stats.gameObject.name + " successfully warned " + friend.name + " about " + warning.name);
                        return state.in_process;
                    }
                }
            }
            return state.passed;
        }
    }

    private state InjuredWarning()
    {
        if ( my_stats.friends.Count == 0)
        {
            Debug.Log(my_stats.gameObject.name + " had no friends to warn about " + warning.name + "'s injury, they are running home instead.");
            if (warning.GetComponent<AgentStats>().temperament < 10)
            {
                Debug.Log(warning.name + " is angered by " + my_stats.gameObject.name + " refusing to help them, and now sees them as an enemy");
                warning.GetComponent<AgentStats>().enemies.Add(my_stats.gameObject);
            }            
            return state.failed;
        }
        else
        {
            foreach (GameObject friend in my_stats.friends)
            {
                while (friend.GetComponent<AgentStats>().helpTarget != warning && !friend.GetComponent<AgentStats>().at_work && !friend.GetComponent<AgentStats>().at_home && !friend.GetComponent<AgentStats>().helping)
                {
                    Debug.Log(my_stats.gameObject.name + " is trying to warn " + friend.name + " about " + warning.name);
                    if (Vector3.Distance(agent.transform.position, friend.transform.position) > 3)
                    {
                        agent.GetComponent<NavMeshAgent>().destination = friend.transform.position;
                        return state.in_process;
                    }
                    else
                    {
                        friend.GetComponent<AgentStats>().helpTarget = warning;
                        Debug.Log(my_stats.gameObject.name + " successfully informed " + friend.name + " about " + warning.name);
                        return state.passed;
                    }
                }
            }
            return state.passed;
        }
    }
}
