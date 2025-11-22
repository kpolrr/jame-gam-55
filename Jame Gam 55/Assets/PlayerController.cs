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
    [SerializeField] float coyoteTime;
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
    private bool _jumpCutAvailable = false;
    private float _coyoteTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapBox(transform.position-transform.lossyScale.y*0.5f*Vector3.up,OverlapBoxSize,0f,Ground);
        if (isGrounded) _coyoteTimer = coyoteTime;
        if (Input.GetKeyDown(jump) && _coyoteTimer > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x,jumpForce);
            _coyoteTimer = 0;
            _jumpCutAvailable = true;
        }


        if (Input.GetKeyUp(jump))
        {
            if (rb.velocity.y>0f && _jumpCutAvailable)
            {
                rb.velocity = new Vector2(rb.velocity.x,rb.velocity.y*jumpCut);
            }
            _jumpCutAvailable = false;
        }

        if (rb.velocity.y < 0.01f) _jumpCutAvailable = false;
        //if (isGrounded) _jumpCutAvailable = false;
        _coyoteTimer -= Time.deltaTime;
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
            if (isGrounded) {calculatedVelocity = Mathf.Lerp(currentspeed,targetSpeed,decceleration*(60*Time.fixedDeltaTime));}
            else {calculatedVelocity = Mathf.Lerp(currentspeed,targetSpeed,passiveAirDecceleration*(60*Time.fixedDeltaTime));}
        } else if (Mathf.Sign(targetSpeed) != Mathf.Sign(currentspeed) && currentspeed != 0) { //if you are trying to move in the direction opposite to which you are moving
            calculatedVelocity = Mathf.Lerp(currentspeed,targetSpeed,decceleration*airAccel*(60*Time.fixedDeltaTime)); //deccelerate quickly
        } else if (Mathf.Abs(targetSpeed) > Mathf.Abs(currentspeed)) { // going in desired direction but havent reached max speed yet
            calculatedVelocity = Mathf.Lerp(currentspeed,targetSpeed,acceleration*airAccel*(60*Time.fixedDeltaTime)); //accelerate
        } else if (isGrounded) { //if grounded and over max speed (can't accelerate more)
            calculatedVelocity = Mathf.Lerp(currentspeed,targetSpeed,acceleration*(60*Time.fixedDeltaTime)); //deccelerate slowly (at the pace of acceleration)
        }
        rb.velocity = new Vector2(calculatedVelocity,rb.velocity.y);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<jumpPad>(out jumpPad o))
        {
            _coyoteTimer = 0f;
            _jumpCutAvailable = false;
        }
    } 
}
