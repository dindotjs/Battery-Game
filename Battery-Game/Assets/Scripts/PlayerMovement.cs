using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float runSpeed = 2f;
    float maxSpeed = 8f;
    float friction = 2f;
    float tolerence = 0.5f;

    bool grounded = true;
    float jumpSpeed = 100f;

    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
    }

    void Move()
    {
        if(Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A)) {
            return;
        }
        if (Input.GetKey(KeyCode.D) && rb.velocity.x < maxSpeed)
        {
            rb.AddForce(Vector2.right * runSpeed);
            return;
        }
        if (Input.GetKey(KeyCode.A) && rb.velocity.x > -maxSpeed)
        {
            rb.AddForce(Vector2.left * runSpeed);
            return;
        }
        if(rb.velocity.x > tolerence)
        {
            rb.AddForce(Vector2.left * friction);
            return;
        }
        if(rb.velocity.x < -tolerence)
        {
            rb.AddForce(Vector2.right * friction);
            return;
        }
    }
    void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.AddForce(Vector2.up * jumpSpeed);
        }
    }
}
