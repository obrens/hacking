using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
        Debug.Log("NodePlacer.DetermineNodeCoordinates> started");
        Random.InitState((int)System.DateTime.Now.Ticks);
        List<Node> nodes = new List<Node>();

        void SingleNodeCoordinates(int nodeRow, int nodeColumn)
        {
            Node node = nodeGrid[nodeRow][nodeColumn];
            if (NodeType.EMPTY == node.Type) return;
            (float min, float max) xBracket = XBracketLimits(nodeRow == 1 ? middleRowCellCount : outerRowCellCount, nodeColumn);
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

    private static (float, float) XBracketLimits(int numberOfBrackets, int bracketNumber) => 
        StaggeredBracketLimits(width, minX, numberOfBrackets, bracketNumber);
    
    private static (float, float) YBracketLimits(int bracketNumber) =>
        StaggeredBracketLimits(height, minY, 3, bracketNumber);

    private static (float, float) StaggeredBracketLimits(float range, float minimum, int numberOfBrackets, int bracketNumber)
    {
        float bracketSize = range / (2 * numberOfBrackets - 1);
        float bracketMinimum = minimum + 2 * bracketNumber * bracketSize;
        return (bracketMinimum, bracketMinimum + bracketSize);
    }
}
