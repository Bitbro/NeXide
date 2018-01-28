using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BulletTimeBullet : MonoBehaviour
{

    [SerializeField] private float range;
    private float distanceToBarrier;
    private float minDistToHealth;
    [SerializeField] private int damage;
    [SerializeField] private LayerMask hitLayers;
    [SerializeField] private LayerMask stoplayers;
    private RaycastHit[] collisions;
    private Ray ray;

    // Use this for initialization
    void Start()
    {

        ray = new Ray(transform.position, transform.forward);
        collisions = Physics.RaycastAll(ray, range, hitLayers).OrderBy(collision => collision.distance).ToArray();

        foreach (RaycastHit collision in collisions)
        {

            

        }

    }

}