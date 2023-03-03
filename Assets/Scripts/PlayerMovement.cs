using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    [Header("Horizontal Movement")]
    public float moveSpeed = 100f;
    public Vector2 direction;
    public Vector2 moveDirection = Vector2.zero;
    private bool facingRight = true;

    [Header("Vertical Movement")]
    public float jumpSpeed = 15f;
    public float jumpDelay = 0.25f;
    private float jumpTimer;

    [Header("Components")]
    public Rigidbody2D rb;
    public Animator animator;
    public LayerMask groundLayer;
    public GameObject characterHolder;
    public InputAction playerControls;

    [Header("Physics")]
    public float maxSpeed = 7f;
    public float linearDrag = 4f;
    public float gravity = 1f;
    public float fallMultiplier = 5f;

    public void OnEnable()
    {
        playerControls.Enable();
    }

    public void OnDisable()
    {
        playerControls.Disable();
    }
    
    // Update is called once per frame
    void Update()
    {
        moveDirection = playerControls.ReadValue<Vector2>();

        if (Input.GetButtonDown("Jump"))
        {
            jumpTimer = Time.time + jumpDelay;
        }

        animator.SetBool("onGround", onGround());

        direction = new Vector2(moveDirection.x, 0);

        moveCharacter(direction.x);
        if (jumpTimer > Time.time && onGround())
        {
            Jump();
        }

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
    void Jump(){
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        jumpTimer = 0;
    }

    private bool onGround()
    {
        return transform.Find("GroundCheck").GetComponent<GroundCheck>().onGround;
    }
    void modifyPhysics() {
        bool changingDirections = (direction.x > 0 && rb.velocity.x < 0) || (direction.x < 0 && rb.velocity.x > 0);

        if(onGround()){
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
            if(rb.velocity.y < 0)
            {
                rb.gravityScale = gravity * fallMultiplier;
            }
            else if(rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.gravityScale = gravity * (fallMultiplier / 2);
            }
        }
    }
    void Flip() {
        facingRight = !facingRight;
        transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);
    }

}