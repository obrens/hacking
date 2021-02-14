using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Network 
{
    public List<Node> Nodes;
    public List<GameObject> HardcodedNodes;
    public List<Connection> Connections;
}

public enum NodeType {START, DATA, TREASURE, FIREWALL, SPAM}

[Serializable]
public class Node 
{
    public NodeType Type;
    public int Risk;
    public float X, Y;
}

[Serializable]
public class Connection 
{
    //public Node Node1;
    //public Node Node2;
    public GameObject Node1;
    public GameObject Node2;
}
