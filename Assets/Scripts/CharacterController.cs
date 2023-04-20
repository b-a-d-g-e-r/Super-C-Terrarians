using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterController : MonoBehaviour {
    public int maxStocks = 3;
    public int currentStocks;
    public float knockbackPercent;
    public Vector2 respawnPosition;
    private bool collidedWithKillZone = false; // keeps track of whether the player has collided with a Kill Zone
    private void Start()
    {
        currentStocks = maxStocks;
        respawnPosition = Vector2.zero; // set the respawn position to the center of the stage
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Kill Zone") && !collidedWithKillZone && gameObject.CompareTag("Player") && !gameObject.CompareTag("Attacks")) // check if the collider has the tag "Kill Zone"
        {
            collidedWithKillZone = true;
            currentStocks--;
            if (currentStocks <= 0)
            {
                SceneManager.LoadScene("Results");
            }
            else
            {
                Respawn();
                collidedWithKillZone = false;
            }
        }
    }

    private void Respawn()
    {
        transform.position = respawnPosition; // move the player to the respawn position
    }
}


