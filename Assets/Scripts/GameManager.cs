using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject player1;
    
	private Character character;

    // This method is called when the player selects a character on the select menu
    public void OnCharacterSelected(Character character)
    {
        this.character = character;
    }

// This method is called when the player clicks the "Start Game" button on the select menu
    public void StartGame()
    {
// Instantiate the player object based on the selected character
 //       player1 = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
//        player1.GetComponent<Player>().character = character;
    }
}