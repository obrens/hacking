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

public enum NodeType {START, DATA, TREASURE, FIREWALL, SPAM, EMPTY}

[Serializable]
public class Node 
{
    public NodeType Type;
    public int Risk;
    public float X, Y;
    
    public Node(NodeType type)
    {
        Type = type;
    }

    public Node(NodeType type, int risk, float x, float y)
    {
        Type = type;
        Risk = risk;
        X = x;
        Y = y;
    }
}

[Serializable]
public class Connection 
{
    public Node Node1;
    public Node Node2;
    //public GameObject Node1;
    //public GameObject Node2;

    public Connection(Node node1, Node node2)
    {
        Node1 = node1;
        Node2 = node2;
    }
}
