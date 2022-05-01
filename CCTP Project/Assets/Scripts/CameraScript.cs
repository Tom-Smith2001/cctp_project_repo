using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//Code written by Tom Smith - Thomas19.Smith@live.uwe.ac.uk

public class CameraScript : MonoBehaviour
{
    private Camera cam;
    public float cam_speed = 20;
    // Start is called before the first frame update
    void Start()
    {
        cam = this.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("GameManager").GetComponent<UIScript>().paused) 
        {
            return;        
        }
        float horiz = Input.GetAxis("Horizontal") * cam_speed;
        float vert = Input.GetAxis("Vertical") * cam_speed;
        float zoom = Input.mouseScrollDelta.y * cam_speed * 50;

        cam.transform.position += transform.up * vert * Time.deltaTime;
        cam.transform.position += transform.right * horiz * Time.deltaTime;

        cam.transform.position += transform.forward * zoom * Time.deltaTime;



    }
}
