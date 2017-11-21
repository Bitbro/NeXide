using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private Gun gun;
    [SerializeField] private LayerMask groundLayer;
    [Header("Movement")]
    [SerializeField]
    private float maxSpeed;
    [SerializeField] private float speedResistance;
    [SerializeField] private float jumpSpeedResistance;
    [SerializeField] private float jumpForce;

    [Header("Debug")]
    [SerializeField]
    private bool onGround;

    [SerializeField] private bool doubleJump;
    [SerializeField] private float speed;

    [HideInInspector]
    public Rigidbody2D rb;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        AimGun();
    }

    // Using FixedUpdate for integrated physics movement.
    private void FixedUpdate()
    {
        // DEBUG CONSTANT
        this.speed = rb.velocity.magnitude;

        // Get Player Input
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Check if player is on ground
        GroundCheck();
        
        // Process user inputted movement
        HorizontalMovement(moveHorizontal);
        VerticalMovement(moveVertical);
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

    //
    // MOVEMENT HELPERS
    //
    private void HorizontalMovement(float moveHorizontal)
    {
        Vector2 movement = new Vector2(moveHorizontal, 0);
        float desiredHorizontalSpeed = moveHorizontal * maxSpeed;

        if (onGround)
        {
            rb.velocity += new Vector2(((desiredHorizontalSpeed - rb.velocity.x) / speedResistance), 0);
        }
        else
        {
            rb.velocity += new Vector2(((desiredHorizontalSpeed - rb.velocity.x) / jumpSpeedResistance), 0);
        }
    }

    // Jumping with axis input
    private bool jumpInputRecharge;
    private bool jump;
    private void VerticalMovement(float moveVertical)
    {
        // Force player to press up a second time for manual timing of second jump
        if (moveVertical < 0.5f)
        {
            jumpInputRecharge = true;
        }

        // Query Jump input
        if (moveVertical > 0.5f)
        {
            jump = true;
        }
        else
        {
            jump = false;
        }

        // First jump
        if (onGround && jump)
        {
            onGround = false;
            jumpInputRecharge = false;
            jump = false;
            Jump();
        }
        // Second jump
        else if (doubleJump && jump && (jumpInputRecharge || rb.velocity.y + Physics2D.gravity.y * Time.deltaTime <= 0))
        {
            doubleJump = false;
            Jump();
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void GroundCheck()
    {
        // Position of ray start
        Vector2 position = transform.position;

        // Direction of ray
        Vector2 direction = Vector2.down;

        // Max distance ray should extend
        float distance = 1.05f;

        // Draw a ray down until it hits the ground or reaches max distance
        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);

        // If ray hits ground
        if (hit.collider != null)
        {
            onGround = true;
            doubleJump = true;
        }

        // If ray hits nothing
        else
        {
            onGround = false;
        }

        // Draw ray for debug (in Gizmos on editor)
        Debug.DrawRay(position, direction * distance, Color.green);
    }
}
