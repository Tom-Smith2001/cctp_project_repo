using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (FOVScript))]
public class NewBehaviourScript : Editor
{
    void OnSceneGUI() 
    {
        FOVScript fow = (FOVScript)target;
        Handles.color = Color.cyan;
        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.viewRadius);
        Vector3 viewAngleA = fow.FindDirection(-fow.viewAngle / 2, false);
        Vector3 viewAngleB = fow.FindDirection(fow.viewAngle / 2, false);

        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.viewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius);

        Handles.color = Color.red;
        foreach (GameObject visibleAgent in fow.visibleAgents) 
        {
            Handles.DrawLine(fow.transform.position, visibleAgent.transform.position);
        }
        if (fow.seeEvent) 
        {
            Handles.DrawLine(fow.transform.position, GameObject.FindGameObjectWithTag("Event").transform.position);

        }
    }
}
