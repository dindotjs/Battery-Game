using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //movement

    float acceleration;
    float runSpeed = 25f;
    float maxSpeed = 6f;
    float turnAroundSpeed = 50f;
    float friction = 60f;
    bool facingRight = true;
    float resistance;

    bool grounded = true;
    bool onGround;
    float jumpSpeed = 11f;
    float groundDistance = 0.57f;
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
    public List<Sprite> batterySprites = new List<Sprite>();
    public SpriteRenderer batteryBottom;
    public bool canPickUp;
    public bool holdingBattery = false;
    Vector2 throwVector = new Vector2(3, 2);
    float throwVelocity = 8f;
    float batteryPushback = 500f;
    public BoxCollider2D hitbox;

    public SpriteRenderer hands;
    Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        groundLayer = LayerMask.GetMask("Ground", "Platform");
        coyoteTime = coyoteTimeConst;
        jumpBuffer = jumpBufferConst;
        acceleration = runSpeed;
        battery = GameObject.FindGameObjectWithTag("Battery");
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        Move();
        Jump();
        PickUpBattery();
        AnimatePlayer();
    }

    void FixedUpdate()
    {
        if(facingRight) { transform.rotation = Quaternion.Euler(0f, 0f, 0); }
        else { transform.rotation = Quaternion.Euler(0f, 180f, 0); }
    }

    void Move()
    {
        if(holdingBattery) { resistance = 1.5f; }
        else { resistance = 1f; }
        acceleration = runSpeed;

        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A)) {
            return;
        }
        if (Input.GetKey(KeyCode.D) && rb.velocity.x < maxSpeed / resistance)
        {
            if(rb.velocity.x < 0) { acceleration = turnAroundSpeed; }
            rb.velocity = new Vector2(rb.velocity.x + ((acceleration / resistance) * Time.deltaTime), rb.velocity.y);
            //rb.AddForce(Vector2.right * (acceleration / resistance) * Time.deltaTime);
            facingRight = true;
            //transform.rotation = Quaternion.Euler(0f, 0f, 0);
            return;
        }
        if (Input.GetKey(KeyCode.A) && rb.velocity.x > -maxSpeed / resistance)
        {
            if(rb.velocity.x > 0) { acceleration = turnAroundSpeed; }
            rb.velocity = new Vector2(rb.velocity.x - ((acceleration / resistance) * Time.deltaTime), rb.velocity.y);
            //rb.AddForce(Vector2.left * (acceleration / resistance) * Time.deltaTime);
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
    }
    void Jump()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.45f, Vector2.down, Mathf.Infinity, groundLayer);
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

        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
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
            rb.gravityScale = 2f * resistance;
        }

        if (rb.velocity.y <= 0f || (!Input.GetKey(KeyCode.Space) && !Input.GetKey(KeyCode.W)))
        {
            jumping = false;
            rb.gravityScale = 4f * resistance;
        }

        if(!jumping && grounded)
        {
            rb.gravityScale = 2f * resistance;
        }
    }
    void PickUpBattery()
    {
        battery.GetComponent<BoxCollider2D>().enabled = !holdingBattery;

        if (holdingBattery && Input.GetKeyDown(KeyCode.E))
        {
            int direction = transform.rotation == Quaternion.Euler(0f, 0f, 0) ? 1 : -1;
            holdingBattery = false;
            battery.layer = LayerMask.NameToLayer("Battery");
            hitbox.enabled = false;
            battery.GetComponent<Rigidbody2D>().velocity = new Vector2(throwVector.normalized.x * throwVelocity * direction + rb.velocity.x, throwVector.normalized.y * throwVelocity + rb.velocity.y);
            rb.AddForce(new Vector2(-throwVector.normalized.x * direction * batteryPushback, -throwVector.normalized.y * batteryPushback));
        }

        else if (canPickUp && !holdingBattery && Input.GetKeyDown(KeyCode.E))
        {
            holdingBattery = true;
            canPickUp = false;
            battery.layer = LayerMask.NameToLayer("HeldBattery");
            hitbox.enabled = true;
        }

        if (holdingBattery)
        {
            battery.transform.position = batteryHolder.transform.position;
            hands.enabled = true;
        }
        else
        {
            hands.enabled = false;
        }

        if (canPickUp)
        {
            battery.GetComponent<SpriteRenderer>().sprite = batterySprites[1];
            batteryBottom.enabled = true;
        }
        else
        {
            battery.GetComponent<SpriteRenderer>().sprite = batterySprites[0];
            batteryBottom.enabled = false;
        }
    }

    void AnimatePlayer()
    {
        Debug.Log(onGround);
        if(Mathf.Round(rb.velocity.x * Time.deltaTime * 100f) != 0 && onGround)
        {
            anim.Play("RunAlt");
            if (holdingBattery)
            {
                anim.speed = 0.5f;
            }
            else
            {
                anim.speed = 1f;
            }
        }
        else if(!onGround && rb.velocity.y > 0)
        {
            anim.Play("Jump");
        }
        else if(!onGround && rb.velocity.y <= 0)
        {
            anim.Play("Fall");
        }
        else
        {
            anim.Play("Idle");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Battery"))
        {
            canPickUp = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Battery"))
        {
            canPickUp = false;
        }
    }
}
