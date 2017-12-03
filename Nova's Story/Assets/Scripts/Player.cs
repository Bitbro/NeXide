using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private Gun gun;
    [SerializeField] private LayerMask groundLayer;
    [Header("Movement")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float speedResistance;
    [SerializeField] private float maxClimbAngle;

    [Header("Jump")]
    [SerializeField] private float jumpSpeedResistance;
    [SerializeField] private float jumpForce;
    [SerializeField] private float maxDoubleJumpForce;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float doubleJumpControl = 2f;

    [Header("Debug")]
    [SerializeField]
    private bool onGround;

    [SerializeField] private bool doubleJump;
    [SerializeField] private float speed;

    [HideInInspector] public Rigidbody2D rb;
    private Vector2 groundNormal;
    private bool usePhysics;
    private List<GameObject> collisions;

    private void Start()
    {
        collisions = new List<GameObject>();
        rb = this.GetComponent<Rigidbody2D>();
        usePhysics = false;
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

        // Allow player to correct double jump in midair and add weightiness to jump
        JumpCorrection();
        // Correct player to ground plane
        GroundCorrection(moveHorizontal);
        
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
            rb.velocity += new Vector2((desiredHorizontalSpeed - rb.velocity.magnitude * (rb.velocity.x>0?1:-1)) / 
                (speedResistance), 0);
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
            Jump(jumpForce);
        }
        // Second jump
        else if (doubleJump && jump && (jumpInputRecharge || rb.velocity.y + Physics2D.gravity.y * Time.deltaTime <= 0))
        {
            doubleJump = false;
            Jump(maxDoubleJumpForce);
        }
    }

    private void Jump(float force)
    {
        rb.velocity = new Vector2(rb.velocity.x, force);
    }

    private void JumpCorrection()
    {
        if (!onGround)
        {
            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector2.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (rb.velocity.y > 0 && !jump && !doubleJump)
            {
                rb.velocity += Vector2.up * Physics.gravity.y * (doubleJumpControl - 1) * Time.deltaTime;
            }
        }
    }
    // Distance boxcast should start from player center
    private readonly Vector2 boxOffset = new Vector2(0, -1.05f);
    // Size of boxcast
    private readonly Vector2 boxSize = new Vector2(0.85f, 0.05f);
    private void GroundCheck()
    {
        // Position of ray start
        Vector2 position = transform.position;
        RaycastHit2D groundHit = Physics2D.BoxCast(position, boxSize, transform.rotation.z, -transform.up, 1.02f, groundLayer);        

        // If ray hits ground
        if (groundHit.collider != null)
        {
            onGround = true;
            doubleJump = true;
            groundNormal = groundHit.normal;
        }
        // If ray hits nothing
        else
        {
            groundNormal = Vector2.zero;
            onGround = false;
        }

        Debug.DrawRay(transform.position, -transform.up);
    }

    private void GroundCorrection(float moveHorizontal)
    {
        if (onGround)
        {

            if (!usePhysics)
            {
                // Zero out gravity on slope (0.17f velocity cancels out gravity)
                rb.velocity = new Vector2(rb.velocity.x, 0.17f);

                float slopeAngle = Vector2.Angle(groundNormal, Vector2.up);
                //rb.velocity += new Vector2(0, -(2.2f * Mathf.Cos(slopeAngle * Mathf.Deg2Rad) -2.2f));
                float velocityX = rb.velocity.x;
                if (slopeAngle <= maxClimbAngle)
                {
                    float moveDistance = Mathf.Abs(velocityX);
                    float climbMultiplier = 0f;
                    if(Mathf.Sign(groundNormal.x) * Mathf.Sign(groundNormal.y) == Mathf.Sign(velocityX)){
                        climbMultiplier = 1.1f; // Higher gravity down slope
                    }
                    else
                    {
                        climbMultiplier = -1f;
                    }

                    float climbVelocityY = -Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance
                        * climbMultiplier;
                    rb.velocity += new Vector2(0, climbVelocityY);
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.rigidbody.bodyType == RigidbodyType2D.Dynamic)
        {
            collisions.Add(collision.gameObject);
            usePhysics = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.rigidbody.bodyType == RigidbodyType2D.Dynamic)
        {
            collisions.Remove(collision.gameObject);
            if (collisions.Count <= 0)
            {
                usePhysics = false;
            }
        }
    }
}
