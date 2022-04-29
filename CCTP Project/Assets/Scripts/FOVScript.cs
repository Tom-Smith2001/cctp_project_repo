using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVScript : MonoBehaviour
{
    public float viewRadius;
    [Range(0,360)]
    public float viewAngle;

    public LayerMask agentMask;
    public LayerMask obstacleMask;
    public LayerMask eventMask;

    public bool seeAgent = false;
    public bool seeEvent = false;

    public List<GameObject> visibleAgents;


    public GameObject gameManager;
    TimeDateScript td;

    private void Start()
    {

        gameManager = GameObject.FindWithTag("GameManager");
        td = gameManager.GetComponent<TimeDateScript>();
        viewRadius = 5 + (this.GetComponent<AgentStats>().observation * 0.75f);
        viewAngle = 45 + (this.GetComponent<AgentStats>().observation * 3);
        float attentionTimer = 0.15f + ((20 - this.GetComponent<AgentStats>().observation) / 50);
        StartCoroutine(VisionCheck(attentionTimer));
    }

    

    void FindVisibleAgents() 
    {
        visibleAgents.Clear();
        Collider[] targetsInRange = Physics.OverlapSphere(transform.position, viewRadius, agentMask);
        Collider[] eventsInRange = Physics.OverlapSphere(transform.position, viewRadius, eventMask);
        for (int i = 0; i < targetsInRange.Length; i++) 
        {
            
            Transform target = targetsInRange[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2) 
            {
                float distToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask) && target != this.transform && !target.GetComponent<AgentStats>().at_home && !target.GetComponent<AgentStats>().at_work) 
                {
                    
                    visibleAgents.Add(target.gameObject);
                    /*if (!this.gameObject.GetComponent<AgentStats>().friends.Contains(target.gameObject)) 
                    {
                        this.gameObject.GetComponent<AgentStats>().friends.Add(target.gameObject);
                    }*/
                    
                }
            }
        }
        for (int i = 0; i < eventsInRange.Length; i++)
        {

            Transform target = eventsInRange[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 1.5)
            {
                float distToTarget = Vector3.Distance(transform.position, target.position);
                if (Physics.Raycast(transform.position, dirToTarget, distToTarget, eventMask))
                {
                    Debug.Log(gameObject.name + " has seen the event!");
                    this.gameObject.GetComponent<ReactScript>().SeeEvent(target.gameObject);

                }
            }
        }
    }


    public Vector3 FindDirection(float angleDeg, bool global) 
    {
        if (!global) 
        {
            angleDeg += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Sin(angleDeg * Mathf.Deg2Rad), 0, Mathf.Cos(angleDeg * Mathf.Deg2Rad)); 
    }

    IEnumerator VisionCheck(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay / td.timescale);
            //Debug.Log(this.GetComponent<AgentStats>().name + " is checking vision");
            FindVisibleAgents();
            if (visibleAgents.Count > 0 && !this.gameObject.GetComponent<AgentStats>().at_home && !this.gameObject.GetComponent<AgentStats>().at_work)
            {

                this.gameObject.GetComponent<ReactScript>().ObserveAgents(visibleAgents);
            }
            
        }
    }
}
