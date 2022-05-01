using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Code written by Tom Smith - Thomas19.Smith@live.uwe.ac.uk

public class Selector : Node
{

    protected List<Node> child_nodes = new List<Node>();

    //constructor where list of child nodes are passed into the selector
    public Selector(List<Node> child_nodes)
    {
        this.child_nodes = child_nodes;
    }

    //evaluation function
    public override state Eval()
    {
        //loop through each child node
        foreach (var node in child_nodes)
        {
            //call the child nodes eval function
            switch (node.Eval())
            {
                //if failed, move on to the next child in the list
                case state.failed:
                    break;
                //if the child is in process do not continue the selector check until it is finished
                case state.in_process:
                    node_state = state.in_process;
                    return node_state;
                //if a chiled passes the selector passes
                case state.passed:
                    node_state = state.passed;
                    return node_state;
            }
        }
        //if all children are looped through and none passed, the selector fails
        node_state = state.failed;
        

        return node_state;
    }

}
