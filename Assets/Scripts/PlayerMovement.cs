using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
	[Header("Horizontal Movement")] 
	public float horizontalAcceleration;
	
	[Header("Components")]
	public Rigidbody2D rigidbody;
	public Animator animator;

	private bool isFacingRight = true;
	
	private void Start()
	{
		vectorGravity = new Vector2(0, -Physics2D.gravity.y);
		rigidbody = GetComponent<Rigidbody2D>();
	}
	private void Update()
	{
		animator.SetFloat("Horizontal", Mathf.Abs(rigidbody.velocity.x));
		animator.SetFloat("Vertical", rigidbody.velocity.y);
		
		rigidbody.velocity = new Vector2(horizontal * velocity, rigidbody.velocity.y);

		if (!isFacingRight && horizontal > 0f)
		{
			Flip();
		}

		else if (isFacingRight && horizontal < 0f)
		{
			Flip();
		}
	}
}