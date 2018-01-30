using UnityEngine;
using System.Collections;

public class HumanoidAIController : AIController
{
    protected override void AIUpdate()
    {
        if (AIManager.GetPlayerTransform() != null)
        {
            this.SetTarget(AIManager.GetPlayerTransform().position);
        }
    }

}
