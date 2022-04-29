using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentGen : MonoBehaviour
{
    public int numOfAgents;
    public GameObject[] spawnPoints;
    public GameObject spPrefab;
    public GameObject agentPrefab;
    public GameObject agentCont;
    public List<string> namesTaken;

    


    // Start is called before the first frame update
    void Start()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("Spawn");
        //PlaceSpawnPoints();
        PlaceAgents();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlaceSpawnPoints()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            
            GameObject sp = GameObject.Instantiate(agentPrefab, spawnPoints[i].transform.position, Quaternion.identity);
            sp.transform.parent = spawnPoints[i].transform;

        }
    }

    void PlaceAgents() 
    {
        namesTaken = new List<string>();
        for(int i = 0; i < numOfAgents; i++) 
        {
            int sp = Random.Range(0, spawnPoints.Length);
            GameObject agent = GameObject.Instantiate(agentPrefab, spawnPoints[sp].transform.position, Quaternion.identity);
            agent.transform.parent = agentCont.transform;
            AgentStats.FirstName firstName = (AgentStats.FirstName)Random.Range(0, 103);
            AgentStats.Surname surname = (AgentStats.Surname)Random.Range(0, 25);
            agent.GetComponent<AgentStats>().name = "" + firstName + " " + surname;
            if (!namesTaken.Contains(agent.GetComponent<AgentStats>().name))
            {
                namesTaken.Add(agent.GetComponent<AgentStats>().name);
                //Debug.Log(namesTaken);
            }
            else 
            {
                if (namesTaken.Count < numOfAgents)
                {
                    while (namesTaken.Contains(agent.GetComponent<AgentStats>().name))
                    {
                        firstName = (AgentStats.FirstName)Random.Range(0, 103);
                        surname = (AgentStats.Surname)Random.Range(0, 25);
                        agent.GetComponent<AgentStats>().name = "" + firstName + " " + surname;
                    }
                    namesTaken.Add(agent.GetComponent<AgentStats>().name);
                }                
            }
            
        }
    }
}
