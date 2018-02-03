using Pathfinding;
using Pathfinding.Util;
using Pathfinding.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine;

[JsonOptIn]
public class PlatformerPathfinding : NavGraph
{
    public LayerMask ground;
    public int width = 10;
    public int height = 10;
    public Vector2 startPos;
    public float scale = 1;

    private PointNode[] nodes;
    private GraphTransform transform;

    protected override IEnumerable<Progress> ScanInternal()
    {
        // Initialize memory of nodes
        PointNode[,] nodeGraph = new PointNode[width, height];
        transform = new GraphTransform(Matrix4x4.TRS(startPos, Quaternion.identity,
            Vector3.one * scale));

        // Creates nodes
        for (int i = 0; i < nodeGraph.GetLength(0); i++)
        {
            for (int j = 0; j < nodeGraph.GetLength(1); j++)
            {
                Vector3 pos = PlatformerPathfinding.CalculateNodePosition(i, j, transform);
                if (Physics2D.RaycastNonAlloc(pos, Vector2.down, new RaycastHit2D[1], this.scale * 0.0001f, this.ground) == 0)
                {
                    RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.down, this.scale, this.ground);
                    if (hit.collider != null)
                    {
                        nodeGraph[i, j] = this.CreateNode(pos);
                    }
                }
            }
        }

        this.FinalizeNodes(nodeGraph);
        yield break;
    }

    private void FinalizeNodes(PointNode[,] nodeGraph)
    {
        PointNode[] nodesTemp = new PointNode[nodeGraph.GetLength(0) * nodeGraph.GetLength(1)];
        int nodeIndex = 0;

        for (int i = 0; i < nodeGraph.GetLength(0); i++)
        {
            for (int j = 0; j < nodeGraph.GetLength(1); j++)
            {
                if (nodeGraph[i, j] != null)
                {
                    nodesTemp[nodeIndex] = nodeGraph[i, j];
                    nodeIndex++;
                }
            }
        }
        this.nodes = new PointNode[nodeIndex];
        for (int i = 0; i < nodeIndex; i++)
        {
            this.nodes[i] = nodesTemp[i];
        }
    }

    public override void GetNodes(Action<GraphNode> action)
    {
        if (nodes == null) return;

        for (int i = 0; i < nodes.Length; i++)
        {
            // Call the delegate
            action(nodes[i]);
        }
    }

    private static Vector3 CalculateNodePosition(int x, int y, GraphTransform transform)
    {
        // Get the direction towards the node from the center
        var pos = new Vector3(x, y, 0);

        // Multiply it with the matrix to get the node position in world space
        pos = transform.Transform(pos);
        return pos;
    }

    // Create a single node at the specified position
    private PointNode CreateNode(Vector3 position)
    {
        return new PointNode(active)
        {
            // Node positions are stored as Int3. We can convert a Vector3 to an Int3 like this
            position = (Int3)position
        };
    }
}