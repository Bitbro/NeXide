using UnityEngine;
using System.Collections.Generic;
public class BasicMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    private float maxSpeed;
    [SerializeField] private float speedResistance;
    [SerializeField] private float maxClimbAngle;
    [SerializeField] private LayerMask groundLayer;

    [Header("Jump")]
    [SerializeField]
    private float jumpSpeedResistance;
    [SerializeField] private float jumpForce;
    [SerializeField] private bool canDoubleJump;
    [SerializeField] private float maxDoubleJumpForce;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float doubleJumpControl = 2f;

    [Header("Debug")]
    [SerializeField]
    private bool onGround;

    [SerializeField] private bool doubleJump;
    [SerializeField] private float speed;

    private Rigidbody2D rb;
    private bool usePhysics;
    private List<GameObject> collisions;
    private Vector2 groundNormal;

    private void Start()
    {
        collisions = new List<GameObject>();
        rb = this.GetComponent<Rigidbody2D>();
    }


    public void HorizontalMovement(float moveHorizontal)
    {
        GroundCheck();
        float desiredHorizontalSpeed = moveHorizontal * maxSpeed;

        if (onGround)
        {
            rb.velocity += new Vector2((desiredHorizontalSpeed - rb.velocity.magnitude * (rb.velocity.x > 0 ? 1 : -1)) /
                (speedResistance), 0);
        }
        else
        {
            rb.velocity += new Vector2(((desiredHorizontalSpeed - rb.velocity.x) / jumpSpeedResistance), 0);
        }

        GroundCorrection(moveHorizontal);
    }

    // Jumping with axis input
    private bool jumpInputRecharge;
    private bool jump;
    public void VerticalMovement(float moveVertical)
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
        else if (canDoubleJump && doubleJump && jump && (jumpInputRecharge || rb.velocity.y + Physics2D.gravity.y * Time.deltaTime <= 0))
        {
            doubleJump = false;
            Jump(maxDoubleJumpForce);
        }

        JumpCorrection();
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
    private readonly Vector2 boxSize = new Vector2(1f, 0.05f);
    private void GroundCheck()
    {
        // Position of ray start
        Vector2 position = transform.position;
        RaycastHit2D groundHit = Physics2D.BoxCast(position, boxSize, transform.rotation.z, -transform.up, 1.02f, groundLayer);
        groundNormal = groundHit.normal;
        float slopeAngle = Vector2.Angle(groundNormal, Vector2.up);
        // If ray hits ground
        if (groundHit.collider != null && slopeAngle <= maxClimbAngle)
        {
            onGround = true;
            doubleJump = true;            
        }
        // If ray hits nothing
        else
        {
            onGround = false;
        }

        Debug.DrawRay(transform.position, -transform.up);
    }

    private void GroundCorrection(float moveHorizontal)
    {
        if (onGround && !usePhysics)
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
                if (Mathf.Sign(groundNormal.x) * Mathf.Sign(groundNormal.y) == Mathf.Sign(velocityX))
                {
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
