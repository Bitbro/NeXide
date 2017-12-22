using UnityEngine;
using System.Collections;

public class HumanoidAIController : AIController
{
    protected override void AIUpdate()
    {
        Debug.Log(AIManager.GetPlayerTransform().position);
        this.SetTarget(AIManager.GetPlayerTransform().position);
    }

}
