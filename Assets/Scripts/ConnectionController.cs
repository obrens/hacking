using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionController : MonoBehaviour
{
    public enum Direction {FORWARD, BACKWARD};

    public NodeController Node1Controller;
    public NodeController Node2Controller;
    private LineRenderer renderer;

    private void Start() 
    {
        renderer = GetComponent<LineRenderer>();
        renderer.SetPositions(new Vector3 [] {});
    }

    public NodeController OtherNode(NodeController thisNode) 
    {
        if (thisNode == Node1Controller) return Node2Controller;
        if (thisNode == Node2Controller) return Node1Controller;
        throw new Exception("This is not a connection of the given node!");
    }

    public void FillUp(int fillTime, Direction direction) 
    {
        //TODO implement
    }
}
