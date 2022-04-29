using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Node
{
    protected state node_state;

    public state ns { get { return node_state; } }

    public abstract state Eval();
}

public enum state 
{
    in_process,
    passed,
    failed
}