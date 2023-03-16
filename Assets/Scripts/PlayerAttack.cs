using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public AttackData[] attacks;  // array of AttackData structs, which contain hitbox game objects and delay times

    public GameObject specialHitbox;  // reference to the special hitbox game object

    private bool isAttacking;  // flag to track if the character is currently attacking
    private PlayerMovement playerMovement;  // reference to the PlayerMovement script

    void Start()
    {
        // get the PlayerMovement component attached to the player
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        // check for attack input
        for (int i = 0; i < attacks.Length; i++)
        {
            if (Input.GetKeyDown(attacks[i].inputKey) && !isAttacking)
            {
                StartCoroutine(ActivateHitboxes(attacks[i]));
            }
        }
    }

    IEnumerator ActivateHitboxes(AttackData attack)
    {
        isAttacking = true;

        List<Collider2D> hitColliders = new List<Collider2D>();

        // activate each hitbox in sequence with a delay
        for (int i = 0; i < attack.hitboxes.Length; i++)
        {
            // check if this is the special hitbox
            if (attack.hitboxes[i] == specialHitbox)
            {
                // rotate the special hitbox based on the player's move direction angle
                specialHitbox.transform.rotation = Quaternion.Euler(0, 0, playerMovement.moveDirectionAngle);
            }

            attack.hitboxes[i].SetActive(true);
            yield return new WaitForSeconds(attack.hitboxDelays[i]);
            attack.hitboxes[i].SetActive(false);

            // add all colliders overlapping with the hitbox to the list
            Collider2D[] colliders = Physics2D.OverlapBoxAll(attack.hitboxes[i].transform.position, attack.hitboxes[i].transform.localScale, 0f);
            for (int j = 0; j < colliders.Length; j++)
            {
                if (colliders[j].gameObject.CompareTag("Enemy"))
                {
                    hitColliders.Add(colliders[j]);
                }
            }
        }

        // apply force to all objects in the hitColliders list
        for (int i = 0; i < hitColliders.Count; i++)
        {
            Rigidbody2D enemyRB = hitColliders[i].GetComponent<Rigidbody2D>();
            if (enemyRB != null)
            {
                Vector2 force = new Vector2(attack.hitboxForce.x * transform.right.x, attack.hitboxForce.y);
                enemyRB.AddForce(force);
            }
        }

        isAttacking = false;
    }
}

[System.Serializable]
public struct AttackData
{
    public KeyCode inputKey;  // input key for the attack
    public GameObject[] hitboxes;  // array of hitbox game objects with colliders
    public float[] hitboxDelays;  // array of delays between activating hitboxes
    public Vector2 hitboxForce; // force to be applied to any enemy hit by the hitbox, with X and Y components relative to the player's direction
}

