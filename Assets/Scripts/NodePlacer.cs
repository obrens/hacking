using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// This class determes the position of nodes in the game.
public static class NodePlacer
{
    private const float minX = -5.5f;
    //private const float maxX = 5.5f;
    private const float width = 11f;
    private const float minY = -3.3f;
    //private const float maxY = 2.9f;
    private const float height = 6.2f;

    public static List<Node> DetermineNodeCoordinates(Node[][] nodeGrid, int outerRowCellCount, int middleRowCellCount) 
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        List<Node> nodes = new List<Node>();

        void SingleNodeCoordinates(int nodeRow, int nodeColumn)
        {
            Node node = nodeGrid[nodeRow][nodeColumn];
            if (NodeType.EMPTY == node.Type) return;
            (float min, float max) xBracket = XBracketLimits(nodeRow, middleRowCellCount, outerRowCellCount, nodeColumn);
            (float min, float max) yBracket = YBracketLimits(nodeRow);
            node.X = Random.Range(xBracket.min, xBracket.max);
            node.Y = Random.Range(yBracket.min, yBracket.max);
            nodes.Add(node);
        }

        int row = 0;
        for (int column = 0; column < outerRowCellCount; column++)
        {
            SingleNodeCoordinates(row, column);
        }
        row = 1;
        for (int column = 0; column < middleRowCellCount; column++)
        {
            SingleNodeCoordinates(row, column);
        }
        row = 2;
        for (int column = 0; column < outerRowCellCount; column++)
        {
            SingleNodeCoordinates(row, column);
        }
        return nodes;
    }

    private static (float, float) XBracketLimits(int nodeRow, int middleRowCellCount, int outerRowCellCount, int bracketNumber)
    {
        if (1 == nodeRow)
        {
            return StaggeredBracketLimits(width, minX, middleRowCellCount, bracketNumber, middleRowCellCount < outerRowCellCount);
        }
        return StaggeredBracketLimits(width, minX, outerRowCellCount, bracketNumber, middleRowCellCount > outerRowCellCount);
    }
    
    private static (float, float) YBracketLimits(int bracketNumber) =>
        StaggeredBracketLimits(height, minY, 3, bracketNumber, false);

    private static (float, float) StaggeredBracketLimits(float range, float minimum, int numberOfBrackets, int bracketNumber, bool startsWithPause)
    {
        float bracketSize;
        float bracketMinimum;
        if (startsWithPause)
        {
            bracketSize = range / (2 * numberOfBrackets + 1);
            bracketMinimum = minimum + (2 * bracketNumber + 1) * bracketSize;
        }
        else
        {
            bracketSize = range / (2 * numberOfBrackets - 1);
            bracketMinimum = minimum + 2 * bracketNumber * bracketSize;
        }
        return (bracketMinimum, bracketMinimum + bracketSize);
    }
}
