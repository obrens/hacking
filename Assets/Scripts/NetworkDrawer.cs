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
            ConnectionController controller = DrawConnection(connection);
            //connection.Node1.GetComponent<NodeController>().Connections.Add(controller);
            //connection.Node2.GetComponent<NodeController>().Connections.Add(controller);
        }
    }

    private ConnectionController DrawConnection(Connection connection) 
    {
        //LineRenderer renderer = connectionController.GetComponent<LineRenderer>();
        //renderer.Positions.Clear();
        return new ConnectionController();
    }
}
