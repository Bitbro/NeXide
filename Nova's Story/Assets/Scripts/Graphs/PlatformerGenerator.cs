using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Pathfinding.Serialization;
using Pathfinding.Util;
// Inherit our new graph from the base graph type
[JsonOptIn]
public class PlatformerGenerator : NavGraph
{
    public LayerMask ground;
    public Vector3 startPos;
    public int width;
    public int height;
    public float maxAngle;
    public int jumpHeight = 5;
    public int airMobi = 2;
    public float scale = 1;

    // Here we will store all nodes in the graph
    PointNode[] nodes;
    GraphTransform transform;
    public override IEnumerable<Progress> ScanInternal()
    {
        PointNode[][] graph = new PointNode[width][];
        for (int i = 0; i < width; i++)
        {
            graph[width] = new PointNode[height];
            for (int j = 0; j < height; j++)
            {
                Vector3 nodePos = CalculateNodePosition(i, j);
                RaycastHit2D hit = Physics2D.Raycast(nodePos, Vector2.down, scale);
                if (hit.collider != null)
                {
                    Vector2 groundNormal = hit.normal;
                    float slopeAngle = Vector2.Angle(groundNormal, Vector2.up);

                    if (slopeAngle <= maxAngle)
                    {
                        CreateNode(nodePos);
                    }
                }

            }
        }
        yield break;
    }

    public override void GetNodes(System.Action<GraphNode> action)
    {
        // This method should call the delegate with all nodes in the graph
        if (nodes == null) return;
        for (int i = 0; i < nodes.Length; i++)
        {
            // Call the delegate
            action(nodes[i]);
        }
    }

    private Vector3 CalculateNodePosition(int x, int y)
    {
        return startPos + new Vector3(x, y) * scale;
    }

    // Create a single node at the specified position
    private PointNode CreateNode(Vector3 position)
    {
        var node = new PointNode(active)
        {
            // Node positions are stored as Int3. We can convert a Vector3 to an Int3 like this
            position = (Int3)position
        };
        return node;
    }

}