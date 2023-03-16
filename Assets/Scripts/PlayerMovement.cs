using System;
using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    [Header("Horizontal Movement")]
    public float moveSpeed = 100f;
    public Vector2 direction;
    public Vector2 moveDirection = Vector2.zero;
    public float moveDirectionAngle;
    private bool facingRight = true;

    [Header("Vertical Movement")]
    public float jumpSpeed = 15f;
    public float jumpDelay = 0.25f;
    public int jumpTotal = 3;
    private bool isJumping = false;
    private float jumpTimer;

    [Header("Components")]
    public Rigidbody2D rb;
    public CapsuleCollider2D cc;
    public Animator animator;
    public LayerMask groundLayer;
    public GameObject characterHolder;
    public PlayerInputActions playerControls;
    private InputAction move;
    private InputAction jump;
    private InputAction lightAttack;

    [Header("Physics")]
    public float maxSpeed = 7f;
    public float linearDrag = 4f;    
    public float gravity = 1f;
    public float fallMultiplier = 5f;
    

    private void Awake()
    {
        playerControls = new PlayerInputActions();
        
    }
    private void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();

        jump = playerControls.Player.Jump;
        jump.Enable();
        jump.performed += Jump;
    }

    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
    }
    
    // Update is called once per frame
    void Update()
    {
        moveDirection = move.ReadValue<Vector2>();

        direction = new Vector2(moveDirection.x, 0);

        moveDirectionAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;

        moveCharacter(direction.x);

        modifyPhysics();    
    }

    void moveCharacter(float horizontal) {
        rb.AddForce(Vector2.right * horizontal * moveSpeed);

        if ((horizontal > 0 && !facingRight) || (horizontal < 0 && facingRight)) {
            Flip();
        }
        if (Mathf.Abs(rb.velocity.x) > maxSpeed) {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
        }
        animator.SetFloat("Horizontal", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("Vertical",rb.velocity.y);
    }
    void Jump(InputAction.CallbackContext context){
        jumpTimer = Time.time + jumpDelay;

        if (onGround())
        {
            jumpTotal = 3;
            isJumping = false;
        }

        if (jumpTimer > Time.time && jumpTotal > 0)
        {
            jumpTotal--;
            isJumping = true;
            rb.velocity = new Vector2(0, 0);
            rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            jumpTimer = 0;
        }
    }

    private bool onGround()
    {
        float heightBuffer = 0.05f;
        RaycastHit2D rayhit = Physics2D.Raycast(cc.bounds.center, Vector2.down, cc.bounds.extents.y + heightBuffer, groundLayer);
        return rayhit.collider != null;
    }
    
    void modifyPhysics() {
        bool changingDirections = (direction.x > 0 && rb.velocity.x < 0) || (direction.x < 0 && rb.velocity.x > 0);

        if(onGround() == true){
            if (Mathf.Abs(direction.x) < 0.4f || changingDirections) {
                rb.drag = linearDrag;
            } else {
                rb.drag = 0f;
            }
            rb.gravityScale = 0;
        }
        else{
            rb.gravityScale = gravity;
            rb.drag = linearDrag * 0.15f;

            if (rb.velocity.y < 0) // add condition to check if player is falling
            {
                if (moveDirection.y < -0.5f) // add condition to check if player is pressing down
                {
                    rb.gravityScale = gravity * fallMultiplier * 3; // apply fast fall
                }
                else
                {
                    rb.gravityScale = gravity * fallMultiplier; // apply normal fall
                }
            }
            else if(rb.velocity.y > 0 && jump.inProgress)
            {
                rb.gravityScale = gravity * (fallMultiplier / 0.6f);
            }
        }
    }
    
    void Flip() {
        facingRight = !facingRight;
        transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);
    }
}