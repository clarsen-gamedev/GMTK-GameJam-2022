// Name: PlayerController.cs
// Author: Connor Larsen
// Date: 07/15/2022
// Description: Controls how the player moves

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Public & Serialized Variables
    [SerializeField] float moveSpeed = 1f;              // How fast the player moved
    [SerializeField] float jumpForce = 1f;              // How high the player jumps
    [SerializeField] float floatForce = 1f;             // How much force is applied with the flap action
    [SerializeField] float groundCheckRadius = 0.5f;    //
    [SerializeField] float slopeCheckDistance = 0.5f;   // 

    [SerializeField] Transform groundCheck; //
    [SerializeField] LayerMask groundLayer; // 

    public enum ControlScheme {BASIC, FLAP, GRAVITY, NONE}; // Enum types for all possible control schemes the player has
    public ControlScheme controlScheme;                     // Reference to the currently used control scheme
    #endregion

    #region Private Variables
    private Rigidbody2D rb;         // Reference to the player's Rigidbody2D
    private CapsuleCollider2D cc;   // Reference to the player's CapsuleCollider2D

    private Vector2 colliderSize;       // Reference to the size of the player's CapsuleCollider2D
    private Vector2 slopeNormalPerp;    // 
    private Vector2 newVelocity;        // 

    private bool isOnSlope;         // If the player is on a slope or not
    private bool isJumping;         // If the player is jumping or not
    private bool canJump;           // If the player is able to jump or not
    private bool isGrounded;        // If the player is grounded or not
    private bool canWalkOnSlope;    // If the player can walk on a slope or not

    private float xInput;               // Stores the horizontal input of the player
    private float maxSlopeAngle;        // Maximum angle the player can jump from
    private float slopeDownAngle;       // 
    private float slopeDownAngleOld;    // 
    #endregion

    #region Functions
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();       // Grab the Rigidbody2D attached to the player
        cc = GetComponent<CapsuleCollider2D>(); // Grab the CapsuleCollider2D attached to the player
        colliderSize = cc.size;                 // Grab the size of the CapsuleCollider2D

        controlScheme = ControlScheme.BASIC;    // Default control scheme to BASIC
    }

    // Update is called once per frame
    private void Update()
    {
        CheckInput();   // Check the player input
    }

    // Fixed Update
    void FixedUpdate()
    {
        CheckGround();
        SlopeCheck();
        ApplyMovement();
    }

    // Check the input of the player
    private void CheckInput()
    {
        // Action
        if (Input.GetKey(KeyCode.Space))    // When the ACTION key is pressed...
        {
            if (controlScheme == ControlScheme.BASIC)   // If the control scheme is set to BASIC...
            {
                if (isGrounded && !isOnSlope)   // Flat jump
                {
                    
                }

                else if (isGrounded && isOnSlope)   // Slope jump
                {

                }

                else if (!isGrounded)   // In air
                {

                }

                if (Mathf.Abs(rb.velocity.y) < 0.001f)   // Check to see if the player is on solid ground
                {
                    rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse); // Add an upwards force to the player's Rigidbody
                }
            }

            else if (controlScheme == ControlScheme.FLAP)   // If the control scheme is set to FLAP...
            {
                rb.velocity = new Vector2(0f, 0f);                               // Reset velocity
                rb.AddForce(new Vector2(0, floatForce), ForceMode2D.Impulse);    // Add an upwards force to the player's Rigidbody
            }

            else if (controlScheme == ControlScheme.GRAVITY)    // If the control scheme is set to GRAVITY...
            {
                if (rb.gravityScale == 1)
                {
                    rb.gravityScale = -1;
                }
                else
                {
                    rb.gravityScale = 1;
                }
            }
        }

        // DEBUG Switch State
        if (Input.GetKey(KeyCode.Alpha1))   // Switch to BASIC
        {
            controlScheme = ControlScheme.BASIC;
            Debug.Log("Set control scheme to " + controlScheme);
        }

        if (Input.GetKey(KeyCode.Alpha2))   // Switch to FLAP
        {
            controlScheme = ControlScheme.FLAP;
            Debug.Log("Set control scheme to " + controlScheme);
        }

        if (Input.GetKey(KeyCode.Alpha3))   // Switch to GRAVITY
        {
            controlScheme = ControlScheme.GRAVITY;
            Debug.Log("Set control scheme to " + controlScheme);
        }
    }

    // Check Ground
    private void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer); // Check to see if the player is grounded, then set the bool

        if (rb.velocity.y <= 0.0f)
        {
            isJumping = false;
        }

        if (isGrounded && !isJumping && slopeDownAngle <= maxSlopeAngle)
        {
            canJump = true;
        }
    }

    // Slope Check
    private void SlopeCheck()
    {
        Vector2 checkPos = transform.position - new Vector3(0.0f, colliderSize.y / 2, 0.0f);

        SlopeCheckX(checkPos);
        SlopeCheckY(checkPos);
    }

    private void SlopeCheckX(Vector2 checkPos)
    {

    }

    private void SlopeCheckY(Vector2 checkPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, groundLayer);

        if (hit)
        {
            slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;

            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if (slopeDownAngle != slopeDownAngleOld)
            {
                isOnSlope = true;   // Player is on a slope
            }

            slopeDownAngleOld = slopeDownAngle;

            Debug.DrawRay(hit.point, slopeNormalPerp, Color.red);
            Debug.DrawRay(hit.point, hit.normal, Color.green);
        }
    }

    // Apply Movement
    private void ApplyMovement()
    {
        float moveX = Input.GetAxis("Horizontal");  // Stores the direction when the left or right input is pressed

        // If not on a slope...
        if (isGrounded && !isOnSlope)
        {
            newVelocity.Set(moveSpeed * moveX, 0.0f);
            rb.velocity = newVelocity;
        }

        // If on a slope...
        else if (isGrounded && isOnSlope)
        {
            newVelocity.Set(moveSpeed * slopeNormalPerp.x * -moveX, moveSpeed * slopeNormalPerp.y * -moveX);
            rb.velocity = newVelocity;
        }

        // If in the air...
        else if (!isGrounded)
        {
            newVelocity.Set(moveSpeed * moveX, rb.velocity.y);
            rb.velocity = newVelocity;
        }
    }
    #endregion
}