using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(requiredComponent: typeof(AudioSource))]

public class Gun : MonoBehaviour {
    [SerializeField] private Transform muzzle;
    [SerializeField] private GameObject bullet;
    private AudioSource audioSource;

	// Use this for initialization
	void Start () {

        audioSource = GetComponent<AudioSource>();

    }
	
	// Update is called once per frame
	void Update () {
		
        if (Input.GetMouseButtonDown(0)) {
            
            Bullet b = Instantiate(bullet, muzzle.position, muzzle.rotation).GetComponent<Bullet>(); //instantiates the bullet with muzzle location and direction
            b.transform.Rotate(new Vector3(0,-90,0));

            audioSource.Play();


        }


    }

}
