using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] float jumpCut;
    [SerializeField] float jumpForce;
    [SerializeField] float acceleration;
    
    [SerializeField] float decceleration;
    [SerializeField] KeyCode right;
    
    [SerializeField] KeyCode left;

    [SerializeField] KeyCode down;
    [SerializeField] Vector2 OverlapBoxSize;

    [SerializeField] LayerMask Ground;
    [SerializeField] KeyCode jump;
    
    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapBox(transform.position-transform.lossyScale.y*0.5f*Vector3.up,OverlapBoxSize,0f,Ground);
        if (Input.GetKeyDown(jump) && isGrounded)
        {
            rb.AddForce(jumpForce*Vector2.up,ForceMode2D.Impulse);
        }

        if (Input.GetKeyUp(jump) && rb.velocity.y>0f)
        {
            rb.velocity = new Vector2(rb.velocity.x,rb.velocity.y*jumpCut);
        }
    }
    void FixedUpdate()
    {
        float targetSpeed = 0; 
        if (Input.GetKey(right))
        {
            targetSpeed = movementSpeed;
        }
        if (Input.GetKey(left))
        {
            targetSpeed = -movementSpeed;
        }

        float currentspeed = rb.velocity.x;
        float calculatedVelocity;
        if (targetSpeed == 0) {
            calculatedVelocity = Mathf.Lerp(currentspeed,targetSpeed,decceleration);
        } else if (Mathf.Abs(targetSpeed) > Mathf.Abs(currentspeed)) {
            calculatedVelocity = Mathf.Lerp(currentspeed,targetSpeed,acceleration);
        } else {
            calculatedVelocity = Mathf.Lerp(currentspeed,targetSpeed,acceleration);
        }
        rb.velocity = new Vector2(calculatedVelocity,rb.velocity.y);
    }
}
