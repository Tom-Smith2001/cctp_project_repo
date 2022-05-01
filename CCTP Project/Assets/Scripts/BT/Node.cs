using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Node
{
    protected state node_state;

    public state ns { get { return node_state; } }

    //the function called each time a node is checked
    public abstract state Eval();
}

//the states to be returned by node evaluations
public enum state 
{
    in_process,
    passed,
    failed
}