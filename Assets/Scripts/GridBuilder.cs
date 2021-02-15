using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// This class generated the nodes and places then in a logical hex grid.
public class GridBuilder : MonoBehaviour
{
    private LevelSettings levelSettings;
    private int dataNodeCount => 
        levelSettings.NodeCount 
        - levelSettings.TreasureNodeCount 
        - levelSettings.FirewallNodeCount 
        - levelSettings.SpamNodeCount;
    private int outerRowCellCount => 
        levelSettings.NodeCount > 11 
            ? 6 
            : levelSettings.NodeCount > 5 ? 4 : 2;
    private int middleRowCellCount => 
        levelSettings.NodeCount > 10 ? 5 : 3;
    
    // The matrix is actually a logical representation of a hex grid. 
    // It has three rows, and the number of cells in each of the rows is given by outerRowCellCount and middleRowCellCount.
    private Node[][] nodeGrid;
    // The grid is fat if the middle row is longer than the top and bottom rows. 
    private bool gridIsFat;
    private Cell startCell;
    private Cell endCell;

    // A certain number of cells will be empty once the grid is populated.
    // emptyCells represents the number of cells that still have to be marked as empty.
    int emptyLeft;
    private int numberOfDataLeft;
    private int numberOfTreasureLeft;
    private int numberOfFirewallLeft;
    private int numberOfSpamLeft;

    private void Start()
    {
        Debug.Log("GridBuilder.Start> started");
        Random.InitState((int)System.DateTime.Now.Ticks);
        levelSettings = GameManager.Instance.LevelSettings;
        InitializeNodeGrid();
        PopulateGrid();
        GetComponent<NetworkBuilder>().BuildNetwork(nodeGrid, gridIsFat, startCell, outerRowCellCount, middleRowCellCount);
    }

    private void InitializeNodeGrid()
    {
        nodeGrid = new Node[][] {
            new Node[outerRowCellCount],
            new Node[middleRowCellCount],
            new Node[outerRowCellCount],
        };
        gridIsFat = (middleRowCellCount > outerRowCellCount);
        numberOfDataLeft = dataNodeCount;
        numberOfTreasureLeft = levelSettings.TreasureNodeCount;
        numberOfFirewallLeft = levelSettings.FirewallNodeCount;
        numberOfSpamLeft = levelSettings.SpamNodeCount;
        emptyLeft = 2 * outerRowCellCount + middleRowCellCount - 1 - levelSettings.NodeCount;

        // We place the start node and one of the treasure nodes on opposite edges of the grid in order to hopefully make all the nodes and connections useful.
        Cell[] leftCells;
        if (gridIsFat)
        {
            leftCells = new Cell[] {
                new Cell(row: 0, column: 0), 
                new Cell(row: 1, column: 0), 
                new Cell(row: 2, column: 0)
            };
        }
        else
        {
            leftCells = new Cell[] {
                new Cell(row: 0, column: 0),
                new Cell(row: 2, column: 0)
            };
        }
        startCell = NetworkAlgorithms.RandomFrom(leftCells);
        nodeGrid[startCell.row][startCell.column] = new Node(NodeType.START);

        switch(startCell.row) {
            case 0: 
                endCell = new Cell(row: 2, column: outerRowCellCount - 1);
                break;
            case 1: 
                Cell[] rightCells = new Cell[] {
                    new Cell(row: 0, column: outerRowCellCount - 1),
                    new Cell(row: 1, column: outerRowCellCount - 1),
                    new Cell(row: 2, column: outerRowCellCount - 1)
                };
                endCell = NetworkAlgorithms.RandomFrom(rightCells);
                break;
            case 2: 
                endCell = new Cell(row: 0, column: outerRowCellCount - 1);
                break;
            default:
                endCell = new Cell(row: 0, column: 0);
                break;
        }
        nodeGrid[endCell.row][endCell.column] = new Node(NodeType.TREASURE);
        numberOfTreasureLeft--;
    }

    private void PopulateGrid() 
    {
        int row = 0;
        for (int column = 0; column < outerRowCellCount; column++)
        {
            PopulateCell(row, column);
        }
        row = 1;
        for (int column = 0; column < middleRowCellCount; column++)
        {
            PopulateCell(row, column);
        }
        row = 2;
        for (int column = 0; column < outerRowCellCount; column++)
        {
            PopulateCell(row, column);
        }
    }

    private void PopulateCell(int row, int column)
    {
        // Skip the grid cell if it is already accupied by the start or treasure nodes 
        // (which we placed when we initialized the grid)
        if ((row == startCell.row && column == startCell.column) || (row == endCell.row && column == endCell.column))
            return;
        
        // Choose a node type for this grid cell. Try to not put treasure or firewall next to start node if possible.
        int choice;
        if (NetworkAlgorithms.AreNeighbours(gridIsFat, row, column, startCell.row, startCell.column) 
            && (emptyLeft + numberOfDataLeft + numberOfSpamLeft) > 0)
        {
            choice = Random.Range(0, emptyLeft + numberOfDataLeft + numberOfSpamLeft);
        }
        else
        {
            choice = Random.Range(0, emptyLeft + numberOfDataLeft + numberOfSpamLeft + numberOfFirewallLeft + numberOfTreasureLeft);
        }

        if (choice < emptyLeft) 
        {
            nodeGrid[row][column] = new Node(NodeType.EMPTY);
            emptyLeft--;
        }
        else if (choice < emptyLeft + numberOfDataLeft)
        {
            nodeGrid[row][column] = new Node(NodeType.DATA);
            numberOfDataLeft--;
        }
        else if (choice < emptyLeft + numberOfDataLeft + numberOfSpamLeft) 
        {
            nodeGrid[row][column] = new Node(NodeType.SPAM);
            numberOfSpamLeft--;
        }
        else if (choice < emptyLeft + numberOfDataLeft + numberOfSpamLeft + numberOfFirewallLeft)
        {
            nodeGrid[row][column] = new Node(NodeType.FIREWALL);
            numberOfFirewallLeft--;
        }
        else 
        {
            nodeGrid[row][column] = new Node(NodeType.TREASURE);
            numberOfTreasureLeft--;
        }
    }
}

public struct Cell
{
    public int row;
    public int column;

    public Cell(int row, int column)
    {
        this.row = row;
        this.column = column;
    }
}