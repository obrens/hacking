using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// This class takes a hex grid of nodes and creates a network with exact node positions, and connections between nodes
public class NetworkBuilder : MonoBehaviour
{
    private const int chanceToConnectDenomminator = 2;
    private Node[][] nodeGrid;
    private bool gridIsFat;
    private Cell startCell;
    private int outerRowCellCount;
    private int middleRowCellCount;
    private List<Connection> connections;
    private Network network;

    public void BuildNetwork(Node[][] nodeGrid, bool gridIsFat, Cell startCell, int outerRowCellCount, int middleRowCellCount)
    {
        Debug.Log("NetworkBuilder.BuildNetwork> started");
        this.nodeGrid = nodeGrid;
        this.gridIsFat = gridIsFat;
        this.startCell = startCell;
        this.outerRowCellCount = outerRowCellCount;
        this.middleRowCellCount = middleRowCellCount;

        MakeConnections();
        RemoveConnectionsToEmptyNodes();
        network = new Network();
        network.Connections = connections;
        network.Nodes = NodePlacer.DetermineNodeCoordinates(nodeGrid, outerRowCellCount, middleRowCellCount);
        //DebugCheckNodes();
        GetComponent<NetworkDrawer>().DrawNetwork(network);
    }

    private void MakeConnections()
    {
        connections = new List<Connection>();

        int minColumns = gridIsFat ? outerRowCellCount : middleRowCellCount;
        Node previousTop = nodeGrid[0][0];
        Node previousMiddle = nodeGrid[1][0];
        Node previousBottom = nodeGrid[2][0];
        Node top = nodeGrid[0][0];
        Node middle = nodeGrid[1][0];
        Node bottom = nodeGrid[2][0];

        // Connecting all cells in first column
        connections.Add(new Connection(top, middle));
        connections.Add(new Connection(middle, bottom));

        for (int currentColumn = 0; currentColumn < minColumns; currentColumn++)
        {
            top = nodeGrid[0][currentColumn];
            middle = nodeGrid[1][currentColumn];
            bottom = nodeGrid[2][currentColumn];

            List<int> rows = new List<int>() {0, 1, 2};
            List<int> connectingNodes = rows
                .Where(row => NodeType.FIREWALL != nodeGrid[row][currentColumn].Type && NodeType.EMPTY != nodeGrid[row][currentColumn].Type)
                .ToList<int>();

            void ConnectRowWise(int row) 
            {
                //TODO Check if previous nodes were empty before connecting
                Node previous;
                switch (row) 
                {
                    case 0:
                        previous = gridIsFat 
                            ? previousTop
                            : NetworkAlgorithms.RandomFrom(new Node[] {previousTop, previousMiddle});
                        connections.Add(new Connection(previous, top));
                        break;
                    case 1:
                        previous = gridIsFat 
                            ? NetworkAlgorithms.RandomFrom(new Node[] {previousTop, previousMiddle, previousBottom})
                            : previousMiddle;
                        connections.Add(new Connection(previous, middle));
                        break;
                    case 2:
                        previous = gridIsFat 
                            ? previousTop
                            : NetworkAlgorithms.RandomFrom(new Node[] {previousMiddle, previousBottom});
                        connections.Add(new Connection(previous, bottom));
                        break;
                }
            }

            // Connecting previous to current column at least once
            int gauranteedConnectionRow = NetworkAlgorithms.RandomFrom(connectingNodes.ToArray());
            ConnectRowWise(gauranteedConnectionRow);
            foreach (int connectingNode in connectingNodes) 
            {
                if (connectingNode != gauranteedConnectionRow) 
                {
                    if (Random.Range(0, chanceToConnectDenomminator) == 0) ConnectRowWise(connectingNode);
                }
            }

            // Connect column wise
            if (connectingNodes.Contains(0) && connectingNodes.Contains(1))
            {
                if (Random.Range(0, chanceToConnectDenomminator) == 0) 
                {
                    connections.Add(new Connection(top, middle));
                }
            }
            if (connectingNodes.Contains(1) && connectingNodes.Contains(2))
            {
                if (Random.Range(0, chanceToConnectDenomminator) == 0) 
                {
                    connections.Add(new Connection(middle, bottom));
                }
            }

            // Connect firewall if present in column
            List<int> firewallNodes = rows
                .Where(row => NodeType.FIREWALL != nodeGrid[row][currentColumn].Type && NodeType.EMPTY != nodeGrid[row][currentColumn].Type)
                .ToList<int>();
            foreach (int firewallRow in firewallNodes)
            {
                ConnectRowWise(firewallRow);
            }

            previousTop = top;
            previousMiddle = middle;
            previousBottom = bottom;
        }

        // Connect last column
        if (gridIsFat) 
        {
            middle = nodeGrid[1][middleRowCellCount - 1];
            Node previous = NetworkAlgorithms.RandomFrom(new Node[] {previousTop, previousMiddle, previousBottom});
            connections.Add(new Connection(previous, middle));
        }
        else
        {
            top = nodeGrid[0][outerRowCellCount - 1];
            Node previous = NetworkAlgorithms.RandomFrom(new Node[] {previousTop, previousMiddle});
            connections.Add(new Connection(previous, top));

            bottom = nodeGrid[2][outerRowCellCount - 1];
            previous = NetworkAlgorithms.RandomFrom(new Node[] {previousMiddle, previousBottom});
            connections.Add(new Connection(previous, bottom));
        }
    }

    private void RemoveConnectionsToEmptyNodes()
    {
        connections = connections.Where(connection => NodeType.EMPTY != connection.Node1.Type && NodeType.EMPTY != connection.Node2.Type).ToList();
        /*List<Connection> badConnections;
        foreach (Connection connection in connections)
        {
            if (NodeType.EMPTY == connection.Node1.Type)
            {

            }
        }*/
    }

    private void DebugCheckNodes()
    {
        foreach (Connection connection in connections)
        {
            bool node1IsPresent = false;
            foreach (Node node in network.Nodes)
            {
                if (node == connection.Node1) 
                {
                    node1IsPresent = true;
                    break;
                }
            }
            if (node1IsPresent)
            {
                Debug.Log("\n1 is present> " + connection.Node1.Type.ToString() + "\n");
            }
            else
            {
                Debug.Log("\n1 is NOT present> " + connection.Node1.Type.ToString() + "\n");
            }

            bool node2IsPresent = false;
            foreach (Node node in network.Nodes)
            {
                if (node == connection.Node2) 
                {
                    node2IsPresent = true;
                    break;
                }
            }
            if (node2IsPresent)
            {
                Debug.Log("\n2 is present> " + connection.Node2.Type.ToString() + "\n");
            }
            else
            {
                Debug.Log("\n2 is NOT present> " + connection.Node2.Type.ToString() + "\n");
            }
        }
    }
}
