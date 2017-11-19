using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    [SerializeField] private Player player;
    private Camera cam;
    [SerializeField] private float maxDistance;
	// Use this for initialization
	void Start () {
        cam = this.GetComponent<Camera>();
	}
	
	// Use FixedUpdate to synchronize with player movement
	void FixedUpdate () {
        Vector3 targetPosition = (cam.ScreenToWorldPoint(Input.mousePosition) + (player.transform.position) * 1.5f) / 2.5f;
        targetPosition.z = -10;
        this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, 0.2f);
    }
}
