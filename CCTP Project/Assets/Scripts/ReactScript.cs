using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Code written by Tom Smith - Thomas19.Smith@live.uwe.ac.uk

public class ReactScript : MonoBehaviour
{
    FOVScript vision;
    AgentStats myStats;
    public GameObject gameManager;
    public List<GameObject> last_seen;

    private void Start()
    {
        //set relevant variables
        myStats = gameObject.GetComponent<AgentStats>();
        vision = gameObject.GetComponent<FOVScript>();
        StartCoroutine(CalmDown(myStats.composure)); //starts a continuous calming coroutine to lose panic as time progresses
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
    }


    // Update is called once per frame
    void Update()
    {
        ClampStats(); // clamp stats every fram

        if (myStats.currentPanic > 20) 
        {
            myStats.currentPanic = 20; //panic cannot exceed 20       
        }
        if (!myStats.panicked && myStats.currentPanic > myStats.composure) //panicked state occurs when an agents panic exceeds their composure
        {
            myStats.panicked = true;
            myStats.running = true;
            gameObject.GetComponent<NavScript>().updateSpeed(); //agents run when panicking
        }
        if (myStats.panicked && myStats.currentPanic < myStats.composure / 2) //stop panicking once panic is low enough
        {
            myStats.panicked = false;
            myStats.running = false;
            gameObject.GetComponent<NavScript>().updateSpeed();
        }

        if (myStats.helping && myStats.helpTarget.GetComponent<AgentStats>().injured == false) //stop trying to help someone if they're not injured
        {
            myStats.helping = false;
        }

        if (myStats.due_home && !myStats.at_home) //start travelling home when due
        {
            myStats.travelling = true;
        }
        else if (myStats.due_work && !myStats.at_work) //start travelling to work when due
        {
            myStats.travelling = true;
        }
        else 
        {
            myStats.travelling = false;        
        }


    }

    //all stats must remain between 1 and 20
    public void ClampStats() 
    {
        if (myStats.composure > 20)
        {
            myStats.composure = 20;
        }
        else if (myStats.composure < 1) 
        {
            myStats.composure = 1;
        }
        if (myStats.fortitude > 20)
        {
            myStats.fortitude = 20;
        }
        else if (myStats.fortitude < 1)
        {
            myStats.fortitude = 1;
        }
        if (myStats.observation > 20)
        {
            myStats.observation = 20;
        }
        else if (myStats.observation < 1)
        {
            myStats.observation = 1;
        }
        if (myStats.bravery > 20)
        {
            myStats.bravery = 20;
        }
        else if (myStats.bravery < 1)
        {
            myStats.bravery = 1;
        }
        if (myStats.temperament > 20)
        {
            myStats.temperament = 20;
        }
        else if (myStats.temperament < 1)
        {
            myStats.temperament = 1;
        }
        if (myStats.speed > 2)
        {
            myStats.speed = 2;
        }
        else if (myStats.speed < 0)
        {
            myStats.speed = 0;
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
            myStats.currentPanic += (Random.Range(1, (20 - myStats.composure)) / myStats.composure);
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
            //Agents that see their friends will either be calmed or panicked based on their mood, and will also be more or less happy depending on their happiness.
            if (myStats.friends.Contains(agent) && !last_seen.Contains(agent))
            {
                myStats.currentPanic = (myStats.currentPanic + otherStats.currentPanic) / 2;
                myStats.happiness = (myStats.happiness + otherStats.happiness) / 2;
            }

            //When an agent sees someone it considers an enemy, if the agent has <5 temperament then there is a small chance they will attack them, otherwise if they have a temerament of <10 they can become stressed if the enemy appears happier than them.
            if (myStats.enemies.Contains(agent) && !last_seen.Contains(agent))
            {
                if (!myStats.attacking)
                {
                    bool attack = false;
                    if (TimeDateScript.chaos)
                    {
                        attack = myStats.temperament < 9 && Random.Range(0, 12) < myStats.temperament;
                    }
                    else 
                    {
                        attack = myStats.temperament < 5 && Random.Range(0, 20) < myStats.temperament;
                    }
                    if (attack)
                    {
                        myStats.attacking = true;
                        myStats.attack_target = agent;
                    }
                    else if(myStats.temperament < 10)
                    {
                        if (otherStats.happiness > myStats.happiness) 
                        {
                            myStats.stress += otherStats.happiness - myStats.happiness;
                        }                    
                    }
                }
            }

            if (otherStats.injured && !otherStats.helped)
            {
                // agents below 8 temperament will not care if an enemy of theirs is injured.
                if (!myStats.enemies.Contains(agent) || myStats.temperament <= 8)
                {
                    Debug.Log(myStats.name + " noticed that " + otherStats.name + " was injured!");
                    if (!myStats.panicked && myStats.composure > otherStats.currentPanic - otherStats.composure)
                    {
                        Debug.Log(myStats.name + " not panicked, should help here.");
                        HelpOther(agent.transform);
                    }
                    else
                    {
                        Debug.Log(myStats.name + " is panicked, now we're here.");
                        if (myStats.composure < otherStats.currentPanic - otherStats.composure)
                        {
                            Debug.Log(myStats.name + " is panicked, warning friends instead.");
                            myStats.current_warning = agent;
                        }
                        else if (myStats.bravery > myStats.currentPanic)
                        {
                            Debug.Log(myStats.name + " is panicked, but is brave and will help.");
                            myStats.currentPanic = myStats.composure;
                            myStats.panicked = false;
                            HelpOther(agent.transform);
                        }
                    }
                }
            }

            //They have successfully noticed the other agents current state, and are not already helping someone
            if (Random.Range(gameObject.GetComponent<AgentStats>().observation, gameObject.GetComponent<AgentStats>().observation + 10) > otherStats.composure && !myStats.helping) 
            {
                //If this agent see another with an excessively higher temperament than them, and this agent doesn't have the maximum enemy count, make them an enemy.
                if (otherStats.temperament - myStats.temperament > 12 && myStats.enemies.Count < myStats.max_enemies) 
                {
                    myStats.enemies.Add(agent);                                    
                }
                if (otherStats.panicked)
                { 
                    if (otherStats.composure >= myStats.composure) 
                    {
                        if (!last_seen.Contains(agent))
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
                        else 
                        {
                            if ((myStats.currentPanic + ((otherStats.currentPanic * ((20 - myStats.composure) / 20)) + 1) / 4) > 20)
                            {
                                myStats.currentPanic = 20;
                            }
                            else
                            {
                                myStats.currentPanic += ((otherStats.currentPanic * ((20 - myStats.composure) / 20)) + 1) / 4;
                            }
                        }
                        
                    }
                    
                }
                
            }
        }
        last_seen.Clear();
        last_seen = agents;
    }

    private void HelpOther(Transform agent)
    {
        //agent.gameObject.GetComponent<AgentStats>().helped = true;
        Debug.Log(myStats.name + " has decided to help " + agent.name);
        myStats.wandering = false;
        myStats.helping = true;
        myStats.helpTarget = agent.gameObject;
        //gameObject.GetComponent<NavScript>().moveTargetTransform = agent.position;
        /*while (Vector3.Distance(transform.position, agent.position) > 5) 
        {
            myStats.helping = true;
        }*/

    }

    IEnumerator CalmDown(float comp) 
    {
        //Agents will continuously try to calm themselves, calming quicker based on their composure stat and based on how many current events they know about.
        while (true) 
        {
            yield return new WaitForSeconds((20 / comp) * 2);

            if (Random.Range(0, 20) < comp && myStats.currentPanic > 0) 
            {
                if (myStats.known_events.Count > 0)
                {
                    myStats.currentPanic -= 1;
                }
                else 
                {
                    myStats.currentPanic -= 2;                
                }
            }
        
        }
        
    
    }

    

}
