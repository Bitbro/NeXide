using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {
	[SerializeField] private Transform handle1;
	[SerializeField] private Transform handle2;
	[SerializeField] private GameObject platform; 
	[SerializeField] private float speed;
	private float time;

	private bool directionSwitch;
	private Vector2 pos;
	// Use this for initialization
	void Start () {
		directionSwitch = true;
	}
	
	// Update is called once per frame

	void FixedUpdate () {
		
		time += Time.deltaTime;

		if (time > 1) {
			time = 0;
			directionSwitch = !directionSwitch;
		}
		//speed = time / ;
		if (directionSwitch) {
			pos = Vector2.Lerp(handle1.position, handle2.position, time/2f);
		} else {
			pos = Vector2.Lerp(handle2.position, handle1.position, time/2f);
		}
		platform.transform.position = pos;
	}
}