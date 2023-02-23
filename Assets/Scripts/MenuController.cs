using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
	public GameManager gameManager;
	public Button startGameButton;
    public Button werewolfButton;
	
	private Character character;
	
	public void OnWerewolfButton()
    {
		Debug.Log("bing chilling");
		character = werewolfButton.GetComponent<Werewolf>();
    }

    public void OnStartGame()
    {
		gameManager.OnCharacterSelected(character);
		gameManager.StartGame();
		startGameButton.interactable = false;
    }
}
