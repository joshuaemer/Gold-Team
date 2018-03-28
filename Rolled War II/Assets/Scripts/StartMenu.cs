using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class StartMenu : MonoBehaviour {

	public void Host(NetworkManager manager) {
        manager.StartHost();
    }

    public void Connect(NetworkManager manager) {
        manager.StartClient();
    }

    public void Disable(GameObject go) {
        go.SetActive(false);
    }

    public void Quit() {
        Application.Quit();
    }
}
