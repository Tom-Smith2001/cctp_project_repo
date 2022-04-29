using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactScript : MonoBehaviour
{
    FOVScript vision;
    AgentStats myStats;
    public GameObject gameManager;

    private void Start()
    {
        myStats = gameObject.GetComponent<AgentStats>();
        vision = gameObject.GetComponent<FOVScript>();
        StartCoroutine(CalmDown(myStats.composure));
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
    }


    // Update is called once per frame
    void Update()
    {

        if (myStats.currentPanic > 20) 
        {
            myStats.currentPanic = 20;        
        }
        if (!myStats.panicked && myStats.currentPanic > myStats.composure) 
        {
            myStats.panicked = true;
            myStats.running = true;
            gameObject.GetComponent<NavScript>().updateSpeed();
        }
        if (myStats.panicked && myStats.currentPanic < myStats.composure / 2) 
        {
            myStats.panicked = false;
            myStats.running = false;
            gameObject.GetComponent<NavScript>().updateSpeed();
        }

        if (myStats.helping && myStats.helpTarget.GetComponent<AgentStats>().injured == false)
        {
            myStats.helping = false;
        }


    }

    public void SeeEvent(GameObject event_seen) 
    {
        
        GameObject furthestNode = this.gameObject;
        if (!myStats.known_events.Contains(event_seen))
        {
            myStats.currentPanic += Random.Range(1, (20 - myStats.composure));
            myStats.known_events.Add(event_seen);
            myStats.current_warning = event_seen;
        }
        else 
        {
            myStats.currentPanic += Mathf.CeilToInt(Random.Range(1, (20 - myStats.composure)) / myStats.composure);
        }
        foreach (GameObject sp in gameManager.GetComponent<AgentGen>().spawnPoints) 
        {
            if (Vector3.Distance(sp.transform.position, GameObject.FindGameObjectWithTag("Event").transform.position)
                > Vector3.Distance(furthestNode.transform.position, GameObject.FindGameObjectWithTag("Event").transform.position))
            {
                furthestNode = sp;
            }
        }
        //gameObject.GetComponent<NavScript>().navMeshAgent.destination = furthestNode.transform.position;
        //Debug.Log("Running to " + gameObject.GetComponent<NavScript>().navMeshAgent.destination);
    }

    public void ObserveAgents(List<GameObject> agents) 
    {
        //Debug.Log("observing");
        foreach (GameObject agent in agents) 
        {

            AgentStats otherStats = agent.gameObject.GetComponent<AgentStats>();

            //They have successfully noticed the other agents current state, and are not already helping someone
            if (Random.Range(gameObject.GetComponent<AgentStats>().observation, gameObject.GetComponent<AgentStats>().observation + 10) > otherStats.composure && !myStats.helping) 
            {

                //Agents that see their friends will either be calmed or panicked based on their mood.
                if (myStats.friends.Contains(agent)) 
                {
                    myStats.currentPanic = (myStats.currentPanic + otherStats.currentPanic) / 2;                
                }


                if (otherStats.panicked) 
                {
                    //Debug.Log(myStats.name + " noticed that " + otherStats.name + " was panicked!");
                    if (otherStats.composure >= myStats.composure) 
                    {
                        if ((myStats.currentPanic + (otherStats.currentPanic * ((20 - myStats.composure) / 20)) + 1) > 20)
                        {
                            myStats.currentPanic = 20;

                        }
                        else
                        {
                            myStats.currentPanic += (otherStats.currentPanic * ((20 - myStats.composure) / 20)) + 1;
                        }
                    }
                    
                }
                if (otherStats.injured && !otherStats.helped) 
                {
                    // agents below 8 temperament will not care if an enemy of theirs is injured.
                    if (!myStats.enemies.Contains(agent) && myStats.temperament < 8)
                    {
                        Debug.Log(myStats.name + " noticed that " + otherStats.name + " was injured!");
                        if (!myStats.panicked && myStats.composure > otherStats.currentPanic - otherStats.composure)
                        {
                            HelpOther(agent.transform);
                        }
                        else
                        {
                            if (myStats.composure < otherStats.currentPanic - otherStats.composure)
                            {
                                myStats.current_warning = agent;
                            }
                            else if (myStats.bravery > myStats.currentPanic)
                            {
                                HelpOther(agent.transform);
                            }
                        }
                    }
                }
            }
        }
        vision.seeAgent = false;
    }

    private void HelpOther(Transform agent)
    {
        //agent.gameObject.GetComponent<AgentStats>().helped = true;
        myStats.wandering = false;
        myStats.helping = true;
        myStats.helpTarget = agent.gameObject;
        //gameObject.GetComponent<NavScript>().moveTargetTransform = agent.position;
        /*while (Vector3.Distance(transform.position, agent.position) > 5) 
        {
            myStats.helping = true;
        }*/

    }

    IEnumerator CalmDown(int comp) 
    {
        while (true) 
        {
            yield return new WaitForSeconds(20 - comp);
            if (Random.Range(0, 20) < comp && myStats.currentPanic > 0) 
            {
                myStats.currentPanic -= 1;
            }
        
        }
        
    
    }

    /*IEnumerator HealAgent(GameObject injured_agent)
    {
        Debug.Log(this.name + " is helping " + injured_agent.name);
        yield return new WaitForSeconds(Random.Range((20 - injured_agent.GetComponent<AgentStats>().fortitude), 20));
        injured_agent.GetComponent<AgentStats>().currentPanic /= 10;
        injured_agent.GetComponent<AgentStats>().injured = false;
        injured_agent.GetComponent<AgentStats>().helped = false;
        myStats.wandering = true;
        myStats.helping = false;
        Debug.Log(this.name + " has successfully helped " + injured_agent.name);
    }*/

}
