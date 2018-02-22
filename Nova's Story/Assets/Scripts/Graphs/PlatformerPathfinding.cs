using Pathfinding;
using Pathfinding.Util;
using Pathfinding.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[JsonOptIn]
public class PlatformerPathfinding : NavGraph
{
    [JsonMember] public LayerMask ground;
    [JsonMember] public int width = 10;
    [JsonMember] public int height = 10;
    [JsonMember] public float resolution = 0.25f;
    [JsonMember] public Vector3 startPos = new Vector2(-5, -5);
    [JsonMember] public float maxSlope = 45;

    private PointNode[] nodes;
    protected override IEnumerable<Progress> ScanInternal()
    {
        if (resolution < 0.1f)
        {
            Debug.Log("Resolution too low!");
            yield break;
        }

        // Initialize memory of nodes
        List<PointNode> nodeList = new List<PointNode>();
        List<PointNode> pastNodes = new List<PointNode>();

        // Creates nodes
        // Storm Raycasting pattern: cast down from sky to hit surfaces
        for (float i = 0; i <= width; i += resolution)
        {
            List<PointNode> curNodes = new List<PointNode>();

            float startY = startPos.y + height;
            Vector3 stormPos = new Vector3(startPos.x + i, startY);
            RaycastHit2D[] hits = Physics2D.RaycastAll(stormPos, Vector2.down, height, this.ground);
            foreach (RaycastHit2D hit in hits)
            {
                Vector3 nodePos = hit.point + new Vector2(0f, 0.01f);
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                PointNode node = null;
                RaycastHit2D inGround = Physics2D.Raycast(nodePos, Vector2.down, 0.0001f, this.ground);
                if (inGround.collider == null && slopeAngle <= maxSlope)
                {
                    node = this.CreateNode(nodePos, true);
                    nodeList.Add(node);
                    curNodes.Add(node);
                }

                // Make connections with past nodes
                if (node != null)
                {
                    node.connections = this.CreateConnections(pastNodes, node);
                }
            }

            pastNodes = curNodes;
        }

        this.nodes = nodeList.ToArray();
        yield break;
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

    // Create a single node at the specified position
    private PointNode CreateNode(Vector3 position, bool walkable = true)
    {
        return new PointNode(active)
        {
            // Node positions are stored as Int3. We can convert a Vector3 to an Int3 like this
            position = (Int3)position,
            Walkable = walkable
        };
    }

    private Connection[] CreateConnections(List<PointNode> pastNodes, PointNode node)
    {
        Vector3 nodePos = (Vector3) node.position;
        List<Connection> connections = new List<Connection>();
        foreach (PointNode pNode in pastNodes)
        {
            Vector3 pastPos = (Vector3)pNode.position;
            float d = Vector2.Distance(pastPos, nodePos);            
            if (d < this.resolution * 1.8f)
            {
                // Check if nodes are blocked by anything
                RaycastHit2D block = Physics2D.Raycast(pastPos, nodePos - pastPos, d, ground);
                if (block.collider == null)
                {
                    // The cost from moving from one node to the next
                    uint cost = (uint)(node.position - pNode.position).costMagnitude;

                    // Add connection to the past nodes
                    List<Connection> pastConnections;
                    if (pNode.connections != null)
                    {
                        pastConnections = new List<Connection>(pNode.connections);
                    }
                    else
                    {
                        pastConnections = new List<Connection>();
                    }
                    pastConnections.Add(new Connection()
                    {
                        node = node,
                        cost = cost
                    });
                    pNode.connections = pastConnections.ToArray();

                    // Add connection to current node
                    connections.Add(new Connection
                    {
                        node = pNode,
                        cost = cost
                    });
                }
            }
        }

        return connections.ToArray();
    }
}