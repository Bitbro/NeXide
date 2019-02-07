using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Seeker), typeof(BasicMovement))]
public abstract class AIController : MonoBehaviour {
    private Seeker seeker;
    private BasicMovement movement;
    private Vector3 target;

	private void Awake() {
        seeker = this.GetComponent<Seeker>();
        movement = this.GetComponent<BasicMovement>();
    }

    // Everytime this AI respawns
    private void OnEnable()
    {
        StartCoroutine(AILoop());
    }

    private Path p;
    private IEnumerator AILoop()
    {
        while (true)
        {
            AIUpdate();
            // Wait for 0.25 seconds to update this AI loop. Random value added to prevent
            // all AI units from updating on the same frame
            yield return new WaitForSeconds(0.4f + Random.value / 4f);

            // Start a path and wait for it
            p = seeker.StartPath(this.transform.position, target);
            yield return StartCoroutine(p.WaitForPath());            
            
        }
    }

    private void Update()
    {        
        if (p != null && p.vectorPath != null && p.vectorPath.Count > 1)
        {
            float horizontal = (p.vectorPath[1].x - this.transform.position.x) > 0 ? 1 : -1;
            movement.HorizontalMovement(horizontal);

            for (int i = 1; i < p.vectorPath.Count; i++)
            {
                Debug.DrawLine(p.vectorPath[i - 1], p.vectorPath[i]);
            }
        }
    }

    protected abstract void AIUpdate();

    protected void SetTarget(Vector3 target)
    {
        this.target = target;
    }
}
