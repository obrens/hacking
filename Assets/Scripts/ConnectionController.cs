using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionController : MonoBehaviour
{
    #region Logical state
    public Connection StaticModel;
    #endregion

    public enum Direction {FORWARD, BACKWARD};

    public NodeController Node1Controller;
    public NodeController Node2Controller;
    private LineRenderer _lineRenderer;
    // Lazy initialization
    private LineRenderer lineRenderer => (null == _lineRenderer) ? (_lineRenderer = GetComponent<LineRenderer>()) : _lineRenderer;

    public void SetNodeControllers(NodeController nodeController1, NodeController nodeController2) 
    {
        if (null != Node1Controller || null != Node2Controller) throw new Exception("Connection already has nodes set!");
        Node1Controller = nodeController1;
        Node2Controller = nodeController2;
        lineRenderer.SetPositions(new Vector3 [] 
            {
                Node1Controller.transform.position, 
                Node2Controller.transform.position
            });
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
