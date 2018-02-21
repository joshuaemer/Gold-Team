using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MenuController : MonoBehaviour {

    private bool inGame = false;
    private bool toggle = false;
    public GameObject mainMenu;
    public GameObject playerMenu;
    public GameObject controllsMenu;

    // Host a Game
	public void Host(NetworkManager manager) {
        manager.StartHost();
        inGame = true;
        mainMenu.SetActive(false);
    }

    // Try to connect to a hosted game
    public void Connect(NetworkManager manager) {
        NetworkClient client = manager.StartClient();
    }

    // When the player gets connected it calls this function
    public void ConfirmConnect() {
        inGame = true;
        mainMenu.SetActive(false);
    }

    // Quit the Application
    public void Quit() {
        Application.Quit();
    }

    // Leave game or Stop Hosting a game
    public void Disconnect(NetworkManager manager) {
        try {
            manager.StopHost();
        }
        catch {
            manager.StopClient();
        }
        inGame = false;
        mainMenu.SetActive(true);
        playerMenu.SetActive(false);
        Camera.main.GetComponent<FollowPlayer>().setPlayer(null);
        Camera.main.transform.SetPositionAndRotation(new Vector3(0f, 50f, 0f), Quaternion.Euler(90, 0, 0));
    }

    // Display Controlls
    // TODO: Add ability to set controlls here maybe, probably somewhere else.
    public void Controlls() {
        playerMenu.SetActive(false);
        controllsMenu.SetActive(true);
    }

    // Leave the controlls menu back to pause menu
    public void ControllsBack() {
        controllsMenu.SetActive(false);
        playerMenu.SetActive(true);
    }

    // Wait for player to press ESC to bring up pause menu
    public void Update() {
        if(Input.GetKeyDown(KeyCode.Escape) && inGame) {
            playerMenu.SetActive(toggle);
            toggle = !toggle;
            controllsMenu.SetActive(false);
        }
    }
}
