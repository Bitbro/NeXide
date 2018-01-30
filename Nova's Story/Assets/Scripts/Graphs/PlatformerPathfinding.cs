using System;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class PlatformerPathfinding : NavGraph
{
    public LayerMask ground;
    public int width;
    public int height;

    public override void GetNodes(Action<GraphNode> action)
    {
        throw new NotImplementedException();
    }

    public override IEnumerable<Progress> ScanInternal()
    {
        throw new NotImplementedException();
    }
}