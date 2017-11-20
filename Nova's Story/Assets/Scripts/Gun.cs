using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {
    [SerializeField] private Transform muzzle;
    [SerializeField] private GameObject bullet;

	// Use this for initialization
	void Start () {
		

	}
	
	// Update is called once per frame
	void Update () {
		
        if (Input.GetMouseButtonDown(0)) {
            
            Bullet b = Instantiate(bullet, muzzle.position, muzzle.rotation).GetComponent<Bullet>(); //instantiates the bullet with muzzle location and direction
            b.transform.Rotate(new Vector3(0,-90,0));
        }
    
    
	}

}
