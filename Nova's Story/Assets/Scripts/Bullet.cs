using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private float startTime;
    public AudioSource audioSource;
    // Use this for initialization
    void Start()
    {

        audioSource = GetComponent<AudioSource>();
        startTime = Time.time;
        audioSource.Play();

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