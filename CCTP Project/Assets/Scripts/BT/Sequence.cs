using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Code written by Tom Smith - Thomas19.Smith@live.uwe.ac.uk

public class Sequence : Node
{

    protected List<Node> child_nodes = new List<Node>();


    //constructor, where list of nodes to be ran in sequence are passed
    public Sequence(List<Node> child_nodes)
    {
        this.child_nodes = child_nodes;
    }


    //evaluation function
    public override state Eval()
    {
        //cycle through each node passed in the list
        foreach (var node in child_nodes) 
        {
            //call the nodes evaluate function
            switch (node.Eval())
            {
                //if the child node fails the sequence is failed
                case state.failed:
                    node_state = state.failed;
                    return node_state;
                // if the child node is in process the sequence is in process
                case state.in_process:
                    node_state = state.in_process;
                    return node_state;
                //if the child node passed, move on to the next child node
                case state.passed:
                    break;
            }
        }
        //if all nodes are checked without failing, return passed
        node_state = state.passed;        
        

        return node_state;
    }

}
