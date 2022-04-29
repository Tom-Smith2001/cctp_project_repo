using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkScript : MonoBehaviour
{
    public List<GameObject> employees;
    public List<GameObject> occupants;
    public GameObject entrance;
    public bool fire;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (fire)
        {
            this.gameObject.layer = LayerMask.NameToLayer("Event");
            this.gameObject.tag = "Event";
            this.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
        }
        else
        {
            this.gameObject.layer = LayerMask.NameToLayer("Wall");
            this.gameObject.tag = "Work";
            this.gameObject.GetComponent<MeshRenderer>().material.color = Color.grey;
        }
    }
}
