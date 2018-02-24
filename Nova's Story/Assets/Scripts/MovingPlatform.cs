using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {
	[SerializeField] private Transform handle1;
	[SerializeField] private Transform handle2;
	[SerializeField] private GameObject platform; 
	[SerializeField] private float speed = .1f;
	private float time;

	private bool directionSwitch;
	private Vector2 pos;
	// Use this for initialization
	void Start () {
		directionSwitch = true;
	}
	
	// Update is called once per frame

	void FixedUpdate () {
		
		time += Time.deltaTime*speed;

		if (time > 1) {
			time = 0;
			directionSwitch = !directionSwitch;
		}
		if (directionSwitch) {
			pos = Vector2.Lerp(handle1.position, handle2.position, time);
		} else {
			pos = Vector2.Lerp(handle2.position, handle1.position, time);
		}
		platform.transform.position = pos;
	}
//	public float rightLimit = 2.5f;
//	public float leftLimit = 1.0f;
//	public float speed = 1.0f;
//	private int direction = 1;
//
//	void Update () {
//		if (transform.position.x > rightLimit) {
//			direction = -1;
//		}
//		else if (transform.position.x < leftLimit) {
//			direction = 1;
//		}
//		movement = Vector3.right * direction * speed * Time.deltaTime; 
//		transform.Translate(movement); 
//	}
}