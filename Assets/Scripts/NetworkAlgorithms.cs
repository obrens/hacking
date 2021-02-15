using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class NetworkAlgorithms
{
    public static T RandomFrom<T>(T[] objects) => objects[Random.Range(0, objects.Length)];

    public static bool AreNeighbours(bool gridIsFat, int row1, int column1, int row2, int column2) 
    {
        if (row1 == row2 && Mathf.Abs(column1 - column2) == 1) return true;
        if (column1 == column2 && Mathf.Abs(row1 - row2) == 1) return true;
        if (Mathf.Abs(row1 - row2) == 1 && Mathf.Abs(column1 - column2) == 1)
        {
            if (1 == row1)
            {
                if (gridIsFat)
                {
                    return column1 > column2;
                }
                else
                {
                    return column1 < column2;
                }
            }
            else
            {
                if (gridIsFat)
                {
                    return column1 < column2;
                }
                else
                {
                    return column1 > column2;
                }
            }
        }
        return false;
    }

    public static List<Cell> GetAllNeighbours(Node[][] nodeGrid, bool gridIsFat, int row, int column)
    {
        List<Cell> neighbours = new List<Cell>();
        if (1 == row || 2 == row) 
        {
            int otherRow = row - 1;
            int otherColumn = column - 1;
            if (otherColumn > -1)
            {
                if (AreNeighbours(gridIsFat, row, column, otherRow, otherColumn)) neighbours.Add(new Cell(otherRow, otherColumn));
            }
            otherColumn = column;
            if (otherColumn > -1 && otherColumn < nodeGrid[otherRow].Length) 
            {
                neighbours.Add(new Cell(otherRow, otherColumn));
            }
            otherColumn = column + 1;
            if (otherColumn < nodeGrid[otherRow].Length) 
            {
                if (AreNeighbours(gridIsFat, row, column, otherRow, otherColumn)) neighbours.Add(new Cell(otherRow, otherColumn));
            }
        }
        {
            int otherRow = row;
            int otherColumn = column - 1;
            if (otherColumn > -1)
            {
                neighbours.Add(new Cell(otherRow, otherColumn));
            }
            otherColumn = column + 1;
            if (otherColumn < nodeGrid[otherRow].Length) 
            {
                neighbours.Add(new Cell(otherRow, otherColumn));
            }
        }
        if (0 == row || 1 == row) 
        {
            int otherRow = row + 1;
            int otherColumn = column - 1;
            if (otherColumn > -1)
            {
                if (AreNeighbours(gridIsFat, row, column, otherRow, otherColumn)) neighbours.Add(new Cell(otherRow, otherColumn));
            }
            otherColumn = column;
            if (otherColumn > -1 && otherColumn < nodeGrid[otherRow].Length) 
            {
                neighbours.Add(new Cell(otherRow, otherColumn));
            }
            otherColumn = column + 1;
            if (otherColumn < nodeGrid[otherRow].Length) 
            {
                if (AreNeighbours(gridIsFat, row, column, otherRow, otherColumn)) neighbours.Add(new Cell(otherRow, otherColumn));
            }
        }
        return neighbours;
    }

    public static List<Cell> FindGridPath(Node[][] nodeGrid, bool gridIsFat, int row1, int column1, int row2, int column2) 
    {
        Debug.Log("NetworkAlgorithms.FindGridPath> started");
        List<Cell> path = new List<Cell>();
        Cell currentCell = new Cell(row: row1, column: column1);
        path.Add(currentCell);
        while (currentCell.row != row2 || currentCell.column != column2) 
        {
            List<Cell> bestTries = new List<Cell>();
            if (row2 < currentCell.row)
            {
                bestTries.Add(new Cell(currentCell.row - 1, currentCell.column));
            }
            if (row2 > currentCell.row)
            {
                bestTries.Add(new Cell(currentCell.row + 1, currentCell.column));
            }
            if (column2 < currentCell.column)
            {
                bestTries.Add(new Cell(currentCell.row, currentCell.column - 1));
            }
            if (column2 > currentCell.column)
            {
                bestTries.Add(new Cell(currentCell.row, currentCell.column + 1));
            }
            Debug.Log("current> " + currentCell.row + "\t" + currentCell.column);
            Debug.Log("endgoal> " + row2 + "\t" + column2);
            Debug.Log("bestTries.Count> " + bestTries.Count);

            // In case we got to our goal node
            if (bestTries.Count == 1 && bestTries[0].row == row2 && bestTries[0].column == column2)
            {
                path.Add(bestTries[0]);
                break;
            }
            
            // Must go around those firewalls
            bestTries = bestTries.Where<Cell>(cell => NodeType.FIREWALL != nodeGrid[cell.row][cell.column].Type).ToList<Cell>();
            if (bestTries.Count > 0) 
            {
                currentCell = RandomFrom<Cell>(bestTries.ToArray());
                path.Add(currentCell);
                continue;
            }

            List<Cell> otherNeighbours = GetAllNeighbours(nodeGrid, gridIsFat, currentCell.row, currentCell.column);
            Cell[] goodNeighbours = otherNeighbours.Where<Cell>(cell => NodeType.FIREWALL != nodeGrid[cell.row][cell.column].Type).ToArray<Cell>();
            if (goodNeighbours.Length == 0)
            {
                Debug.LogWarning("There were no good neighbours!");
                currentCell = RandomFrom(otherNeighbours.ToArray());
                path.Add(currentCell);
                continue;
            }
            currentCell = RandomFrom<Cell>(goodNeighbours);
            path.Add(currentCell);
        }
        return path;
    }
}
