using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private float speed;
    

    // Use this for initialization
    void OnEnable()
    {


        StartCoroutine(BulletDespawn());


    }

    // Update is called once per frame
    void FixedUpdate()
    {       
        transform.position += speed * (transform.rotation * Vector3.right) * Time.deltaTime;

    }

    private IEnumerator BulletDespawn()
    {

        yield return new WaitForSeconds(4f);

        this.gameObject.SetActive(false);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger Entered");
        this.gameObject.SetActive(false);

        Health health = other.GetComponent<Health>();
        if (health != null)
        {

            health.TakeDamage(34);

        }
        

    }

}