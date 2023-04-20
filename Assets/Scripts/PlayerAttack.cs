using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack: MonoBehaviour {

	public AttackData[] attacks; // array of AttackData structs, which contain hitbox game objects and delay times
	public GameObject specialHitbox; // reference to the special hitbox game object
	private bool isAttacking; // flag to track if the character is currently attacking
	private bool isGrabbing; // flag to track if the character is currently grabbing
	private PlayerMovement playerMovement; // reference to the PlayerMovement script

	void Start() {
		// get the PlayerMovement component attached to the player
		playerMovement = GetComponent < PlayerMovement > ();
	}

	// Update is called once per frame
	void Update() {
		// check for attack input
		for (int i = 0; i < attacks.Length; i++) {
			if (Input.GetKeyDown(attacks[i].inputKey) && !isAttacking) {
				StartCoroutine(ActivateHitboxes(attacks[i]));
			}

			if (Input.GetKeyDown(attacks[i].inputKey) && isGrabbing)
			{
				isGrabbing = false;
			}
		}
	}

	IEnumerator ActivateHitboxes(AttackData attack) {
		isAttacking = true;

		// activate each hitbox in sequence with a delay
		for (int i = 0; i < attack.hitboxes.Length; i++) {
			// check if this is the special hitbox	
			if (attack.hitboxes[i] == specialHitbox) {	
				// rotate the special hitbox based on the player's move direction angle	
				if (playerMovement.moveDirection == Vector2.zero && playerMovement.facingRight == false) {	
					specialHitbox.transform.rotation = Quaternion.Euler(0, 0, 180f);	
				} else {	
					specialHitbox.transform.rotation = Quaternion.Euler(0, 0, playerMovement.moveDirectionAngle);	
				}	
			}
			// activate the hitbox
			attack.hitboxes[i].SetActive(true);

			// wait for the hitbox delay
			yield
			return new WaitForSeconds(attack.hitboxDelays[i]);

			// deactivate the hitbox
			attack.hitboxes[i].SetActive(false);

			// apply force to all colliders overlapping with the hitbox
			Collider2D[] colliders = Physics2D.OverlapCapsuleAll(
				attack.hitboxes[i].transform.position, 
				attack.hitboxes[i].GetComponent<CapsuleCollider2D>().size, 
				CapsuleDirection2D.Horizontal, 
				attack.hitboxes[i].transform.rotation.eulerAngles.z
			);
			for (int j = 0; j < colliders.Length; j++) {
				if (colliders[j].gameObject.CompareTag("Enemy")) {
					if (attack.isGrab == true && !isGrabbing)
					{
						StartCoroutine(GrabEnemy(colliders[j].gameObject, attack.throwForce, attack.knockbackValue));
					}
					else {
						Rigidbody2D enemyRB = colliders[j].GetComponent<Rigidbody2D>();
						if (enemyRB != null) {
							CharacterController controller = colliders[j].GetComponent<CharacterController>();
							Vector2 force = new Vector2(
								attack.hitboxForce.x * transform.right.x, 
								attack.hitboxForce.y
							) * (controller.knockbackPercent/100);
							enemyRB.AddForce(force);
							
							// add knockback percent to enemy CharacterController
							if (controller != null) {
								controller.knockbackPercent += attack.knockbackValue;
							}
						}
						
					}
					
				}
			}
		}

		isAttacking = false;
	}
	IEnumerator GrabEnemy(GameObject enemy, float force, float knockback) {
		isGrabbing = true;

		// Disable the enemy's rigidbody and collider
		Rigidbody2D enemyRB = enemy.GetComponent<Rigidbody2D>();
		Collider2D enemyCollider = enemy.GetComponent<Collider2D>();
		enemyRB.simulated = false;
		enemyCollider.enabled = false;
		playerMovement.rb.simulated = false;

		// Wait for the player to throw the enemy
		while (isGrabbing) {
			// Move the enemy to the player's position
			enemy.transform.position = transform.position + transform.right * 1f;
			yield return null;
		}

		// Enable the enemy's rigidbody and collider
		enemyRB.simulated = true;
		enemyCollider.enabled = true;
		playerMovement.rb.simulated = true;
		
		// get enemy character controller
		CharacterController controller = enemy.GetComponent<CharacterController>();
		
		// Apply a force to the enemy in the direction of the throw
		Vector2 throwDirection = playerMovement.moveDirection;
		enemyRB.AddForce(throwDirection * force * (controller.knockbackPercent/100));
		
		// add knockback percent from grab coroutine to enemy CharacterController
		if (controller != null) {
			controller.knockbackPercent += knockback;
		}
		isGrabbing = false;
	}
}

[System.Serializable]
public struct AttackData {
	public KeyCode inputKey; // input key for the attack
	public GameObject[] hitboxes; // array of hitbox game objects with colliders
	public float[] hitboxDelays; // array of delays between activating hitboxes
	public Vector2 hitboxForce; // force to be applied to any enemy hit by the hitbox, with X and Y components relative to the player's direction
	public float knockbackValue; // knock back to add to a player
	public bool isGrab; // whether the attack is a grab or not
	public float throwForce; // force to be applied to any enemy once thrown, with X and Y components relative to the player's input direction
}
