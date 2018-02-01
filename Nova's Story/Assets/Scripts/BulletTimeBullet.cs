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
    [SerializeField] private int decrementor;
    [SerializeField] private LayerMask hitLayers;
    [SerializeField] private LayerMask stoplayers;
    private Health health;
    private RaycastHit2D[] collisions;
    private Ray ray;
    
    // Use this for initialization
    void Start()
    {

        ray = new Ray(transform.position, transform.forward);
        collisions = Physics2D.RaycastAll((Vector2) transform.position, (Vector2) transform.rotation.eulerAngles, range, hitLayers).OrderBy(collision => collision.distance).ToArray();

        foreach (RaycastHit2D collision in collisions)
        {

            health = collision.collider.gameObject.GetComponent<Health>();

            if (health == null)
            {

                this.damage = 0;
                Destroy(this.gameObject);

            }
            else
            {

                health.TakeDamage(damage);
                damage -= decrementor;

                if (damage <= 0)
                {

                    damage = 0;
                    Destroy(this.gameObject);

                }

            }

        }

    }

}