using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventPlacement : MonoBehaviour
{
    public Camera cam;
    public GameObject eventPrefab;
    public bool currentEvent;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.GetComponent<UIScript>().paused) 
        {
            return;        
        }
        if (Input.GetMouseButtonUp(1)) 
        {
            
            Vector3 mousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.transform.position.y));
            RaycastHit hit;
            Vector3 dir = (mousePos - cam.transform.position).normalized;
            if (Physics.Raycast(cam.transform.position, dir, out hit, 1000f))
            {
                Debug.Log("hit");
                if (hit.collider.tag == "Work" || hit.collider.tag == "House")
                {
                    this.gameObject.GetComponent<UIScript>().highlighted_object = hit.collider.gameObject;
                    if (hit.collider.tag == "Work")
                    {
                        hit.collider.gameObject.GetComponent<WorkScript>().fire = true;
                    }
                    if (hit.collider.tag == "House")
                    {
                        hit.collider.gameObject.GetComponent<HouseScript>().fire = true;
                    }
                }
                else if (hit.collider.tag == "Agent")
                {
                    hit.collider.gameObject.GetComponent<AgentStats>().injured = true;
                }
                else if(hit.collider.tag != "Event")
                {
                    GameObject threat = GameObject.Instantiate(eventPrefab, mousePos, Quaternion.identity);
                    Debug.Log(threat.name);
                    this.GetComponent<AgentGen>().Bake();
                    threat.GetComponent<EventScript>().InjureNearby();
                    currentEvent = true;
                }
            }
            else
            {
                Debug.Log("no hit");
            }
        
        }
        if (Input.GetMouseButtonUp(0))
        {
            
            Vector3 mousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.transform.position.y));
            RaycastHit hit;
            Vector3 dir = (mousePos - cam.transform.position).normalized;
            if (Physics.Raycast(cam.transform.position, dir, out hit, 1000f))
            {
                Debug.Log("hit");
                if (hit.collider.tag == "Agent" || hit.collider.tag == "Work" || hit.collider.tag == "House")
                {
                    this.gameObject.GetComponent<UIScript>().highlighted_object = hit.collider.gameObject;
                }
                if (hit.collider.tag == "Event" && hit.collider.gameObject.GetComponent<EventScript>() == null) 
                {
                    this.gameObject.GetComponent<UIScript>().highlighted_object = hit.collider.gameObject;
                }
            }
            else 
            {
                Debug.Log("no hit");            
            }

        }
    }
}
