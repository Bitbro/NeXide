using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {



    private int health;
    public int damage;

	// Use this for initialization
	void Awake () {

        health = 100;
		
	}
	
	// Update is called once per frame
	void Update () {

        
		
	}

    public void TakeDamage(int damage)
    {

        if (health > 0) {

            health -= damage;

            if (health <= 0)
            {

                Death();

            }

        }


    }

    void Death()
    {

        this.gameObject.SetActive(false);

    }

}
