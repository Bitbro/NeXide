using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Gun gun;
    private BasicMovement movementHandler;

    private void Start()
    {
        movementHandler = this.GetComponent<BasicMovement>();
    }

    private void Update()
    {
        gun.AimGun(Camera.main.ScreenToWorldPoint(Input.mousePosition), this.transform);
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
 
}
