using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] float jumpCut;
    [SerializeField] float jumpForce;
    [SerializeField] float acceleration;
    
    [SerializeField] float aircontrol;
    [SerializeField] float passiveAirDecceleration;
    [SerializeField] float decceleration;
    [SerializeField] KeyCode right;
    
    [SerializeField] KeyCode left;
    [SerializeField] KeyCode down;
    [SerializeField] Vector2 OverlapBoxSize;

    [SerializeField] LayerMask Ground;
    [SerializeField] KeyCode jump;
    
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool _jumpCutAvailable;

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
            _jumpCutAvailable = true;
        }


        if (Input.GetKeyUp(jump) && rb.velocity.y>0f && _jumpCutAvailable)
        {
            rb.velocity = new Vector2(rb.velocity.x,rb.velocity.y*jumpCut);
            _jumpCutAvailable = false;
        }
        if (isGrounded) _jumpCutAvailable = false;
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
        float calculatedVelocity = currentspeed;
        float airAccel = (isGrounded) ? 1f:aircontrol;
        if (targetSpeed == 0) { //deccelerate if you are not trying to move AND you are grounded
            if (isGrounded) {calculatedVelocity = Mathf.Lerp(currentspeed,targetSpeed,decceleration);}
            else {calculatedVelocity = Mathf.Lerp(currentspeed,targetSpeed,passiveAirDecceleration);}
        } else if (Mathf.Sign(targetSpeed) != Mathf.Sign(currentspeed) && currentspeed != 0) { //if you are trying to move in the direction opposite to which you are moving
            calculatedVelocity = Mathf.Lerp(currentspeed,targetSpeed,decceleration*airAccel); //deccelerate quickly
        } else if (Mathf.Abs(targetSpeed) > Mathf.Abs(currentspeed)) { // going in desired direction but havent reached max speed yet
            calculatedVelocity = Mathf.Lerp(currentspeed,targetSpeed,acceleration*airAccel); //accelerate
        } else if (isGrounded) { //if grounded and over max speed (can't accelerate more)
            calculatedVelocity = Mathf.Lerp(currentspeed,targetSpeed,acceleration); //deccelerate slowly (at the pace of acceleration)
        }
        rb.velocity = new Vector2(calculatedVelocity,rb.velocity.y);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<jumpPad>(out jumpPad o))
        {
            _jumpCutAvailable = false;
        }
    } 
}
