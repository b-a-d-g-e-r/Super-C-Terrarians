using System;
using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement: MonoBehaviour {
	[Header("Horizontal Movement")]
	public float moveSpeed = 100f;
	public Vector2 direction;
	public Vector2 moveDirection = Vector2.zero;
	public float moveDirectionAngle;
	public bool facingRight = true;

	[Header("Vertical Movement")]
	public float jumpSpeed = 15f;
	public float jumpDelay = 0.25f;
	public int jumpTotal = 3;
	private bool isJumping = false;
	private float jumpTimer;
	public float coyoteTime = 0.1f;
	private float coyoteTimeCounter = 0f;

	[Header("Components")]
	public Rigidbody2D rb;
	public CapsuleCollider2D cc;
	public Animator animator;
	public LayerMask groundLayer;
	public PlayerInputActions playerControls;
	private InputAction move;
	private InputAction jump;

	[Header("Physics")]
	public float maxSpeed = 7f;
	public float linearDrag = 4f;
	public float gravity = 1f;
	public float fallMultiplier = 5f;

	private void Awake() {
		playerControls = new PlayerInputActions(); // Create a new instance of the PlayerInputActions script
	}

	private void OnEnable() {
		move = playerControls.Player.Move; // Get the move input action from the PlayerInputActions script
		move.Enable(); // Enable the move input action

		jump = playerControls.Player.Jump; // Get the jump input action from the PlayerInputActions script
		jump.Enable(); // Enable the jump input action
		jump.performed += Jump; // Add the Jump function as a callback for when the jump input action is performed
	}

	private void OnDisable() {
		move.Disable(); // Disable the move input action
		jump.Disable(); // Disable the jump input action
	}

	// Update is called once per frame
	void Update() {
		moveDirection = move.ReadValue<Vector2>(); // Read the value of the move input action and store it as a Vector2
		direction = new Vector2(moveDirection.x, 0); // Set the direction to the x value of the moveDirection Vector2

		moveDirectionAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg; // Calculate the angle of the moveDirection Vector2 in degrees

		moveCharacter(direction.x); // Call the moveCharacter function with the x value of the direction Vector2

		modifyPhysics(); // Call the modifyPhysics function
		
		if (onGround()) // Check if the player is on the ground.
		{
			jumpTotal = 3; // Set the total number of jumps the player can make to 3.
			isJumping = false; // Set the player's jumping state to false.
		}
	}

	void moveCharacter(float horizontal) {
		rb.AddForce(Vector2.right * horizontal * moveSpeed); // Apply a force in the x direction based on the horizontal input

		if ((horizontal > 0 && !facingRight) || (horizontal < 0 && facingRight)) { // Check if the player's direction has changed
			Flip(); // Call the Flip function to flip the player's sprite
		}
		if (Mathf.Abs(rb.velocity.x) > maxSpeed) { // Limit the player's horizontal velocity to the maxSpeed variable
			rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
		}
		animator.SetFloat("Horizontal", Mathf.Abs(rb.velocity.x)); // Set the Horizontal parameter in the animator based on the absolute value of the player's x velocity
		animator.SetFloat("Vertical", rb.velocity.y); // Set the Vertical parameter in the animator based on the player's y

	}
	void Jump(InputAction.CallbackContext context) {
		jumpTimer = Time.time + jumpDelay; // Set the jump timer to the current time plus the jump delay.
		if (jumpTimer > Time.time && jumpTotal > 0) // Check if the jump timer is greater than the current time and the player has jumps remaining.
		{
			jumpTotal--; // Decrease the number of jumps the player can make by 1.
			isJumping = true; // Set the player's jumping state to true.
			rb.velocity = new Vector2(0, 0); // Set the player's velocity to 0.
			rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse); // Add an upward force to the player to make them jump.
			jumpTimer = 0; // Reset the jump timer.
		}
	}

	void modifyPhysics() {

		bool changingDirections = (direction.x > 0 && rb.velocity.x < 0) || (direction.x < 0 && rb.velocity.x > 0); // Determine if the character is changing direction

		// If the character is on the ground
		if (onGround() == true) {

			// If the character is moving slowly or changing directions, apply linear drag
			if (Mathf.Abs(direction.x) < 0.4f || changingDirections) {
				rb.drag = linearDrag;
			} else {
				rb.drag = 0f;
			}

			rb.gravityScale = 0; // Set gravity scale to zero, since the character is on the ground
		}
		// If the character is not on the ground
		else {

			// Set the gravity scale and drag based on the character's falling speed
			rb.gravityScale = gravity;
			rb.drag = linearDrag * 0.15f;

			// If the character is falling
			if (rb.velocity.y < 0) {

				// If the player is pressing down, apply a fast fall
				if (moveDirection.y < -0.5f) {
					rb.gravityScale = gravity * fallMultiplier * 3;
				}
				// Otherwise, apply a normal fall
				else {
					rb.gravityScale = gravity * fallMultiplier;
				}
			}
			// If the character is jumping
			else if (rb.velocity.y > 0 && jump.inProgress) {
				rb.gravityScale = gravity * (fallMultiplier / 0.8f); // Apply less gravity during the jump, to allow jump height to be altered actively during the jump
			}
		}
	}

	// Flip the character horizontally
	void Flip() {
		facingRight = !facingRight;
		transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);
	}
	
	private bool onGround() {
		float heightBuffer = 0.05f;
		RaycastHit2D rayhit = Physics2D.Raycast(cc.bounds.center, Vector2.down, cc.bounds.extents.y + heightBuffer, groundLayer);
    
		if (rayhit.collider != null) {
			// Player is currently on the ground
			coyoteTimeCounter = coyoteTime;
			return true;
		}
		else {
			// Player is not on the ground, but may still be in coyote time
			coyoteTimeCounter -= Time.deltaTime;
			return coyoteTimeCounter > 0;
		}
	}

}