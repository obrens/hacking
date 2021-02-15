using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// This class contains some methods to help with network/grid manipulation.
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
}
