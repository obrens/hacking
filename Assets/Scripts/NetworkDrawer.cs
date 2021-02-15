using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkDrawer : MonoBehaviour
{

    [Serializable]
    public struct NodeTypePrefabPair 
    {
        public NodeType Type;
        public GameObject Prefab;
    }
    public List<NodeTypePrefabPair> NodePrefabs;
    public GameObject ConnectionPrefab;

    private Dictionary<NodeType, GameObject> nodeTypePrefabDictionary;
    private Dictionary<Node, NodeController> nodeDictionary = new Dictionary<Node, NodeController>();
    private Network network;

    private void Awake()
    {
        nodeTypePrefabDictionary = NodePrefabs.ToDictionary(pair => pair.Type, pair => pair.Prefab);
    }

    public void DrawNetwork(Network network) 
    {
        Debug.Log("NetworkDrawer.DrawNetwork> started");
        this.network = network;
        DrawNodes();
        DrawConnections();
    }

    private void DrawNodes()
    {
        foreach (Node node in network.Nodes)
        {
            GameObject nodeObject = Instantiate(nodeTypePrefabDictionary[node.Type], new Vector3(node.X, node.Y, 0f), Quaternion.identity) as GameObject;
            nodeDictionary[node] = nodeObject.GetComponent<NodeController>();
        }
    }

    private void DrawConnections() 
    {
        foreach (Connection connection in network.Connections)
        {
            ConnectionController connectionController = CreateConnectionObject();
            connectionController.StaticModel = connection;
            SetControllerCrossReferences(connectionController);
        }
    }

    private ConnectionController CreateConnectionObject()
    {
        GameObject connectionObject = Instantiate(ConnectionPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        return connectionObject.GetComponent<ConnectionController>();
    }

    private void SetControllerCrossReferences(ConnectionController connectionController) 
    {
        if (connectionController == null) Debug.Log("connectionController");
        if (connectionController?.StaticModel == null) Debug.Log("StaticModel");
        if (connectionController?.StaticModel?.Node1 == null) Debug.Log("Node1");
        NodeController nodeController1 = nodeDictionary[connectionController.StaticModel.Node1].GetComponent<NodeController>();
        NodeController nodeController2 = nodeDictionary[connectionController.StaticModel.Node2].GetComponent<NodeController>();
        nodeController1.Connections.Add(connectionController);
        nodeController2.Connections.Add(connectionController);
        connectionController.SetNodeControllers(nodeController1, nodeController2);
    }
}
