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

    public void AimGun(Vector3 aimPosition, Transform aimer)
    {
        Vector3 target = aimPosition - transform.position;
        target.z = 0;

        if (target.x <= 0)
        {
            aimer.transform.localScale = new Vector3(-1, 1, 1);
            transform.rotation = Quaternion.Euler(0, 0, -(Mathf.Atan2(target.y, -target.x) * Mathf.Rad2Deg));
        }
        else
        {
            aimer.transform.localScale = new Vector3(1, 1, 1);
            transform.rotation = Quaternion.Euler(0, 0, (Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg));
        }
    }
}
