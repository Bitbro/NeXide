using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(requiredComponent: typeof(AudioSource))]

public class Gun : MonoBehaviour {
    [SerializeField] private Transform muzzle;
    private AudioSource audioSource;

	// Use this for initialization
	void Start () {

        audioSource = GetComponent<AudioSource>();

    }
	
	// Update is called once per frame
	void Update () {
		
        if (Input.GetMouseButtonDown(0)) {
            SpicySpriteManager.AddBullet(muzzle.position, muzzle.rotation);
            

            audioSource.Play();


        }


    }

}
