using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //movement
    float runSpeed = 1000f;
    float maxSpeed = 6f;
    float turnAroundSpeed = 2500f;
    float acceleration;
    float friction = 60f;
    bool facingRight = true;

    bool grounded = true;
    bool onGround;
    float jumpSpeed = 11f;
    float groundDistance = 0.52f;
    int groundLayer;
    float coyoteTime;
    static float coyoteTimeConst = 0.1f;
    float jumpBuffer;
    static float jumpBufferConst = 0.15f;
    bool spaceDown;
    bool jumping = false;

    Rigidbody2D rb;

    //battery
    GameObject battery;
    public GameObject batteryHolder;
    public GameObject batteryPlacer;
    bool canPickUp;
    bool holdingBattery = false;
    Vector2 throwVector = new Vector2(3, 1);
    float throwVelocity = 6f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        groundLayer = LayerMask.GetMask("Ground", "Battery");
        coyoteTime = coyoteTimeConst;
        jumpBuffer = jumpBufferConst;
        acceleration = runSpeed;
        battery = GameObject.FindGameObjectWithTag("Battery");
    }


    void Update()
    {
        Move();
        Jump();
        PickUpBattery();
    }

    void FixedUpdate()
    {
        if(facingRight) { transform.rotation = Quaternion.Euler(0f, 0f, 0); }
        else { transform.rotation = Quaternion.Euler(0f, 180f, 0); }
    }

    void Move()
    {
        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A)) {
            return;
        }
        if (Input.GetKey(KeyCode.D) && rb.velocity.x < maxSpeed)
        {
            if(rb.velocity.x < 0) { acceleration = turnAroundSpeed; } 
            rb.AddForce(Vector2.right * acceleration * Time.deltaTime);
            facingRight = true;
            //transform.rotation = Quaternion.Euler(0f, 0f, 0);
            return;
        }
        if (Input.GetKey(KeyCode.A) && rb.velocity.x > -maxSpeed)
        {
            if(rb.velocity.x > 0) { acceleration = turnAroundSpeed; }
            rb.AddForce(Vector2.left * acceleration * Time.deltaTime);
            facingRight = false;
            //transform.rotation = Quaternion.Euler(0f, 180f, 0);
            return;
        }

        //friction
        if(rb.velocity.x < friction * Time.deltaTime && rb.velocity.x > -friction * Time.deltaTime)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
            return;
        }
        else if(rb.velocity.x >= friction * Time.deltaTime)
        {
            rb.velocity = new Vector2(rb.velocity.x - friction * Time.deltaTime, rb.velocity.y);
            return;
        }
        else if(rb.velocity.x <= -friction * Time.deltaTime)
        {
            rb.velocity = new Vector2(rb.velocity.x + friction * Time.deltaTime, rb.velocity.y);
            return;
        }
        acceleration = runSpeed;
    }
    void Jump()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.5f, Vector2.down, Mathf.Infinity, groundLayer);
        float distance = Mathf.Abs(hit.point.y - transform.position.y);

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
            spaceDown = false;
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            //rb.AddForce(Vector2.up * jumpSpeed);
            rb.gravityScale = 2f;
        }

        if (rb.velocity.y <= 0f || !Input.GetKey(KeyCode.Space))
        {
            jumping = false;
            rb.gravityScale = 4f;
        }

        if(!jumping && grounded)
        {
            rb.gravityScale = 2f;
        }
    }
    void PickUpBattery()
    {
        if(canPickUp && Input.GetKeyDown(KeyCode.E))
        {
            holdingBattery = true;
            battery.layer = LayerMask.NameToLayer("HeldBattery");
            GetComponent<BoxCollider2D>().enabled = true;
        }

        else if(holdingBattery && Input.GetKeyDown(KeyCode.E)) {
            int direction = transform.rotation == Quaternion.Euler(0f, 0f, 0) ? 1 : -1;
            holdingBattery = false;
            battery.layer = LayerMask.NameToLayer("Battery");
            GetComponent<BoxCollider2D>().enabled = false;
            battery.GetComponent<Rigidbody2D>().velocity = new Vector2(throwVector.normalized.x * throwVelocity * direction + rb.velocity.x, throwVector.normalized.y * throwVelocity + rb.velocity.y);
            //battery.transform.position = batteryPlacer.transform.position;
        }

        if (holdingBattery)
        { 
            battery.transform.position = batteryHolder.transform.position; 
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Battery")
        {
            canPickUp = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Battery")
        {
            canPickUp = false;
        }
    }
}
