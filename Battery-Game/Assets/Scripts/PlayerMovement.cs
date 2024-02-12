using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float runSpeed = 4f;
    float maxSpeed = 6f;
    float turnAroundSpeed = 8f;
    float acceleration;
    float friction = 8f;
    float tolerence = 0.5f;

    bool grounded = true;
    bool onGround;
    float jumpSpeed = 400f;
    float groundDistance = 0.5f;
    int groundLayer;
    float coyoteTime;
    static float coyoteTimeConst = 0.1f;
    float jumpBuffer;
    static float jumpBufferConst = 0.5f;
    bool spaceDown;
    bool jumping = false;

    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        groundLayer = LayerMask.GetMask("Ground");
        coyoteTime = coyoteTimeConst;
        jumpBuffer = jumpBufferConst;
        acceleration = runSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
    }

    void Move()
    {
        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A)) {
            return;
        }
        if (Input.GetKey(KeyCode.D) && rb.velocity.x < maxSpeed)
        {
            if(rb.velocity.x < 0) { acceleration = turnAroundSpeed; } 
            rb.AddForce(Vector2.right * acceleration);
            return;
        }
        if (Input.GetKey(KeyCode.A) && rb.velocity.x > -maxSpeed)
        {
            if(rb.velocity.x > 0) { acceleration = turnAroundSpeed; }
            rb.AddForce(Vector2.left * acceleration);
            return;
        }

        //friction
        if(rb.velocity.x < tolerence && rb.velocity.x > -tolerence)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
            return;
        }
        else if(rb.velocity.x > friction * Time.deltaTime)
        {
            rb.velocity = new Vector2(rb.velocity.x - friction, rb.velocity.y);
            return;
        }
        else if(rb.velocity.x < friction * Time.deltaTime)
        {
            rb.velocity = new Vector2(rb.velocity.x + friction, rb.velocity.y);
            return;
        }
        acceleration = runSpeed;
    }
    void Jump()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, groundLayer);
        float distance = Mathf.Abs(hit.point.y - transform.position.y);
        Debug.Log(distance);

        if(distance < groundDistance) { onGround = true; }
        else { onGround = false; }

        if (onGround)
        {
            grounded = true;
            coyoteTime = coyoteTimeConst;
        }

        else if (!onGround)
        {
            coyoteTime -= Time.deltaTime;
            if (coyoteTime <= 0f)
            {
                grounded = false;
            }
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            spaceDown = true;
            jumpBuffer = jumpBufferConst;
        }

        if (spaceDown)
        {
            jumpBuffer -= Time.deltaTime;
            if (jumpBuffer <= 0f)
            {
                spaceDown = false;
            }
        }

        if (spaceDown && grounded && !jumping)
        {
            coyoteTime = 0f;
            jumpBuffer = 0f;
            jumping = true;
            rb.AddForce(Vector2.up * jumpSpeed);
            rb.gravityScale = 1.5f;
        }

        if (jumping && rb.velocity.y <= 0f || !Input.GetKey(KeyCode.Space))
        {
            jumping = false;
            rb.gravityScale = 2.5f;
        }

        if(!jumping && grounded)
        {
            rb.gravityScale = 2f;
        }
    }
}
