using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    [SerializeField] private PlayerController player;
    private Camera cam;
    [SerializeField] private float maxDistance;
	// Use this for initialization
	void Start () {
        cam = this.GetComponent<Camera>();
	}
	
	// Use FixedUpdate to synchronize with player movement
	void FixedUpdate () {
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        // Delta algorithm. Ask Preston if you're curious
        Vector3 targetPositionDelta = (mousePos + 3 * player.transform.position) / 4 - player.transform.position;
        targetPositionDelta.z = -10;
        targetPositionDelta = Vector3.ClampMagnitude(targetPositionDelta, maxDistance);
        this.transform.position = Vector3.Lerp(this.transform.position, player.transform.position + targetPositionDelta, 0.2f);
    }
}
