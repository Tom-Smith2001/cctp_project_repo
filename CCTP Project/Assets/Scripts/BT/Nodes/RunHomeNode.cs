using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Code written by Tom Smith - Thomas19.Smith@live.uwe.ac.uk

public class RunHomeNode : Node
{
    private AgentStats my_stats;
    private GameObject agent;
    private bool friend_house = false;
    private bool work_hide = false;
    private GameObject target;

    //constructor with variables passed
    public RunHomeNode(AgentStats my_stats, GameObject agent)
    {
        this.my_stats = my_stats;
        this.agent = agent;
    }


    //evaluation function
    public override state Eval()
    {
        //if they dont know their house is on fire, they will run home.
        if (!my_stats.known_events.Contains(my_stats.home))
        {
            if (Vector3.Distance(my_stats.home.GetComponent<HouseScript>().entrance.transform.position, agent.transform.position) < 2.5)
            {
                my_stats.home.GetComponent<HouseScript>().occupants.Add(agent);
                my_stats.at_home = true;
                return state.passed;
            }
            else
            {
                agent.GetComponent<NavMeshAgent>().destination = my_stats.home.GetComponent<HouseScript>().entrance.transform.position;
                return state.in_process;
            }

        }
        else //if their house is on fire, they will first try to run to a friends house, but they must both consider eachother friends
        {
            if (!friend_house && !work_hide)
            {
                if (my_stats.friends.Count > 0)
                {
                    foreach (GameObject friend in my_stats.friends)
                    {
                        //can instead run to a friends house if they also consider the agent a friend.
                        if (friend.GetComponent<AgentStats>().friends.Contains(agent))
                        {
                            target = friend.GetComponent<AgentStats>().home.GetComponent<HouseScript>().entrance;
                            agent.GetComponent<NavMeshAgent>().destination = target.transform.position;
                            friend_house = true;
                            break;
                        }

                    }
                }
            }
            //if that is not an option they will choose to run to their workplace instead.
            if (!friend_house && !work_hide)
            {
                work_hide = true;
                target = my_stats.work_place.GetComponent<WorkScript>().entrance;
                agent.GetComponent<NavMeshAgent>().destination = target.transform.position;
            }

            //return passed once they reach their target location
            if (friend_house && Vector3.Distance(agent.GetComponent<NavMeshAgent>().destination, agent.transform.position) < 2.5)
            {
                target.transform.parent.GetComponent<HouseScript>().occupants.Add(agent);
                my_stats.at_home = true;
                return state.passed;
            }
            else if (work_hide && Vector3.Distance(agent.GetComponent<NavMeshAgent>().destination, agent.transform.position) < 2.5)
            {
                target.transform.parent.GetComponent<WorkScript>().occupants.Add(agent);
                my_stats.at_work = true;
                return state.passed;
            }
            else 
            {
                return state.in_process;            
            }


        }
        
        
    }
}
