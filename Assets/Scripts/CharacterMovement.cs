using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float runMultiplier = 2f;
    public float rotationSpeed = 15f;
    [SerializeField] public float jumpForce = 5f; // strength of jump
    [SerializeField] private float doubleJumpForce = 4f; 

    private Rigidbody rb;
    private float horizontalInput;
    private float verticalInput;

    [Header("Ground Check Settings")]
    [SerializeField] private float groundDistance = 0.5f; // ray length
    [SerializeField] private LayerMask groundMask;        // ground layers

    private bool isGrounded;            // true when standing on the ground
    private bool jumpRequested = false; // set in Update(), used later
    private bool doubleJumpRequested = false; // set in TryDoubleJump() 

    private bool hasDoubleJumped = false;     // one per jump 

    public bool IsGrounded => isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {

        isGrounded = Physics.Raycast(transform.position + transform.up*groundDistance/2, -transform.up, groundDistance, groundMask);
        // Back on the ground: the double jump is available again 

        if (isGrounded) 
        { 
            hasDoubleJumped = false; 
        } 

        //3D Movement Inputs (X and Z axes)
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");


        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            jumpRequested = true;
        }
    }

    void FixedUpdate()
    {
        // Apply Physics Movement
        MovePlayer();

        if (jumpRequested) 
        { 
            Jump(jumpForce); 
            jumpRequested = false; 
        } 

        // NEW: the second jump, in mid-air 
        if (doubleJumpRequested) 
        { 
            Jump(doubleJumpForce); 
            doubleJumpRequested = false; 
        } 
    }

    void MovePlayer()
    {
        float currentSpeed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed *= runMultiplier;
        }

        Vector3 moveDir = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        Vector3 targetVelocity = moveDir * currentSpeed;
        targetVelocity.y = rb.linearVelocity.y;
        rb.linearVelocity = targetVelocity;

        if (moveDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    private void Jump(float force) 
    { 

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z); 

        rb.AddForce(Vector3.up * force, ForceMode.VelocityChange); 

    } 

    public bool TryDoubleJump() 
    { 
        // Not allowed on the ground, or if it was already used 
        if (isGrounded || hasDoubleJumped) return false; 
        
        doubleJumpRequested = true; 
        hasDoubleJumped = true; 

        return true; 
    }

}