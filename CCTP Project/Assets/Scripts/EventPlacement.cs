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
        if (Input.GetMouseButtonUp(0)) 
        {
            if (currentEvent) 
            {
                Destroy(GameObject.FindWithTag("Event"));
                currentEvent = false;
            }
            Vector3 mousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.transform.position.y));
            GameObject threat = GameObject.Instantiate(eventPrefab, mousePos, Quaternion.identity);
            currentEvent = true;
        
        }
    }
}
