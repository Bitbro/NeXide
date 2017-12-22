using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Gun gun;
    private BasicMovement movementHandler;

    private void Start()
    {
        movementHandler = this.GetComponent<BasicMovement>();
    }

    private void Update()
    {
        AimGun();
    }

    // Using FixedUpdate for integrated physics movement.
    private void FixedUpdate()
    {
        // Get Player Input
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Process user inputted movement
        movementHandler.HorizontalMovement(moveHorizontal);
        movementHandler.VerticalMovement(moveVertical);  
    }

    //
    // AIM HELPERS
    //
    private void AimGun()
    {
        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition) - gun.transform.position;
        target.z = 0;

        if (target.x <= 0)
        {
            this.transform.localScale = new Vector3(-1, 1, 1);
            gun.transform.rotation = Quaternion.Euler(0, 0, -(Mathf.Atan2(target.y, -target.x) * Mathf.Rad2Deg));
        } 
        else
        {
            this.transform.localScale = new Vector3(1, 1, 1);
            gun.transform.rotation = Quaternion.Euler(0, 0, (Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg));
        }
    }    
}
