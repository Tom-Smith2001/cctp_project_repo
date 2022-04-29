using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Node
{

    protected List<Node> child_nodes = new List<Node>();

    public Sequence(List<Node> child_nodes)
    {
        this.child_nodes = child_nodes;
    }

    public override state Eval()
    {
        foreach (var node in child_nodes) 
        {
            switch (node.Eval())
            {
                case state.failed:
                    node_state = state.failed;
                    return node_state;
                case state.in_process:
                    node_state = state.in_process;
                    return node_state;
                case state.passed:
                    break;
            }
        }
        
        node_state = state.passed;        
        

        return node_state;
    }

}
