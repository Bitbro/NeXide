using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private float startTime;

    // Use this for initialization
    void Start()
    {

        startTime = Time.time;

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        float timeSinceIntantiation = Time.time - startTime;
        transform.position += speed * (transform.rotation * Vector3.right) * Time.deltaTime;

        if (timeSinceIntantiation > 5)
        {

            Destroy(gameObject);

        }
    }
}