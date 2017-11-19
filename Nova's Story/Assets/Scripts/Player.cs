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
    private Rigidbody2D rb;

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
        this.speed = rb.velocity.magnitude;
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        GroundCheck();
        HorizontalMovement(moveHorizontal);
        if (moveVertical > 0.3f)
        {
            VerticalMovement();
        }
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
    private void VerticalMovement()
    {
        if (onGround)
        {
            onGround = false;
            Jump();
            StartCoroutine(EnableDoubleJump());
        }
        else if (doubleJump)
        {
            doubleJump = false;
            Jump();
        }
    }
    private IEnumerator EnableDoubleJump()
    {
        yield return new WaitForSecondsRealtime(0.25f);
        doubleJump = true;
    }
    private void Jump()
    {
        rb.velocity += new Vector2(0, jumpForce);
    }
    private void GroundCheck()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        float distance = 1.21f;
        Debug.DrawRay(position, direction * distance, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);
        if (hit.collider != null)
        {
            onGround = true;
            doubleJump = false;
        }
        else if (onGround)
        {
            onGround = false;
            doubleJump = true;
        }
    }


}
