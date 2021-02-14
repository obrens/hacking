using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkDrawer : MonoBehaviour
{
    public GameObject ConnectionPrefab;
    //private Dictionary<Node, NodeController> nodeDictionary;

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
