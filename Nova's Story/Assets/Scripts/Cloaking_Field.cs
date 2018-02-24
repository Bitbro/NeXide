using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloaking_Field : MonoBehaviour {

    private SpriteRenderer rend;
    [SerializeField] private float rate;
    private Health health;
    private float damage;

	// Use this for initialization
	void Start () {
        rend = this.gameObject.GetComponent<SpriteRenderer>();
        health = this.gameObject.GetComponent<Health>();
        
        
    }
	
	// Update is called once per frame
	void Update () {

        if (rend.enabled == false)
        {

            damage += rate * Time.deltaTime;
            health.TakeDamage((int)damage);
            damage -= (int)damage;

        }
        if ((this.health.GetShield() > 0 ) && (Input.GetKeyUp(KeyCode.R)))
        {

            rend.enabled = !rend.enabled;
            
            

        }
        if (this.health.GetShield() <= 0)
        {

            rend.enabled = true;

        }

	}
}
