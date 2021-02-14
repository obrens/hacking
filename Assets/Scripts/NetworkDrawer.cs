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

    private void Awake()
    {
        nodeTypePrefabDictionary = NodePrefabs.ToDictionary(pair => pair.Type, pair => pair.Prefab);
    }

    public void DrawNodes(Network network)
    {
        foreach (Node node in network.Nodes)
        {
            GameObject nodeObject = Instantiate(nodeTypePrefabDictionary[node.Type], new Vector3(node.X, node.Y, 0f), Quaternion.identity) as GameObject;
            nodeDictionary[node] = nodeObject.GetComponent<NodeController>();
        }
    }

    public void DrawConnections(Network network) 
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
        NodeController nodeController1 = connectionController.StaticModel.Node1.GetComponent<NodeController>();
        NodeController nodeController2 = connectionController.StaticModel.Node2.GetComponent<NodeController>();
        nodeController1.Connections.Add(connectionController);
        nodeController2.Connections.Add(connectionController);
        connectionController.SetNodeControllers(nodeController1, nodeController2);
    }
}
