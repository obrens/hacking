using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NetworkBuilder : MonoBehaviour
{
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

        connections = new List<Connection>();
        NecessaryConnections();
        OptionalConnections();
        ConnectDisconnectedNodes();
        network = new Network();
        network.Connections = connections;
        network.Nodes = NodePlacer.DetermineNodeCoordinates(nodeGrid, outerRowCellCount, middleRowCellCount);
        GetComponent<NetworkDrawer>().DrawNetwork(network);
    }

    private void NecessaryConnections() 
    {
        int row = 0;
        for (int column = 0; column < outerRowCellCount; column++)
        {
            NodeType type = nodeGrid[row][column].Type;
            if (NodeType.TREASURE == type || NodeType.FIREWALL == type) 
            {
                ConnectionsTo(row, column);
            }
        }
        row = 1;
        for (int column = 0; column < middleRowCellCount; column++)
        {
            NodeType type = nodeGrid[row][column].Type;
            if (NodeType.TREASURE == type || NodeType.FIREWALL == type) 
            {
                ConnectionsTo(row, column);
            }
        }
        row = 2;
        for (int column = 0; column < outerRowCellCount; column++)
        {
            NodeType type = nodeGrid[row][column].Type;
            if (NodeType.TREASURE == type || NodeType.FIREWALL == type) 
            {
                ConnectionsTo(row, column);
            }
        }
    }

    private void ConnectionsTo(int row, int column)
    {
        List<Cell> cells = NetworkAlgorithms.FindGridPath(nodeGrid, gridIsFat, startCell.row, startCell.column, row, column);
        Node previousNode = nodeGrid[startCell.row][startCell.column];
        for (int i = 1; i < cells.Count; i++)
        {
            Cell cell = cells[i];
            Node potentialNode = nodeGrid[cell.row][cell.column];
            if (NodeType.EMPTY == potentialNode.Type)
            {
                continue;
            }
            Connection connection = new Connection(previousNode, potentialNode);
            connections.Add(connection);
            previousNode = potentialNode;
        }
    }

    private void OptionalConnections()
    {
        //TODO Just randomly make connections between nodes from neighbouring cells with a percentage chance
    }

    // Goes through all the nodes, and if any one is disconnected from all other nodes, just connects it randomly with a neighbour
    // This method is in O(2) time with regard to the number of nodes.
    private void ConnectDisconnectedNodes()
    {
        int row = 0;
        for (int column = 0; column < outerRowCellCount; column++)
        {
            Node node = nodeGrid[row][column];
            ConnectIfDisconnected(node, row, column);
        }
        row = 1;
        for (int column = 0; column < middleRowCellCount; column++)
        {
            Node node = nodeGrid[row][column];
            ConnectIfDisconnected(node, row, column);
        }
        row = 2;
        for (int column = 0; column < outerRowCellCount; column++)
        {
            Node node = nodeGrid[row][column];
            ConnectIfDisconnected(node, row, column);
        }
    }

    private void ConnectIfDisconnected(Node node, int row, int column)
    {
        bool connected = false;
        foreach (Connection connection in connections)
        {
            if (connection.Node1 == node || connection.Node2 == node) connected = true;
        }

        if(!connected)
        {
            List<Cell> neighbouringCells = NetworkAlgorithms.GetAllNeighbours(nodeGrid, gridIsFat, row, column);
            Cell[] nonEmptyCells = neighbouringCells.Where(cell => NodeType.EMPTY == nodeGrid[cell.row][cell.column].Type).ToArray<Cell>();
            if (nonEmptyCells.Length > 0)
            {
                Cell cell = NetworkAlgorithms.RandomFrom(nonEmptyCells);
                connections.Add(new Connection(node, nodeGrid[cell.row][cell.column]));
            }
            else
            {
                Debug.LogWarning("Cell had non non-empty neighbours!");
            }
        }
    }
}
