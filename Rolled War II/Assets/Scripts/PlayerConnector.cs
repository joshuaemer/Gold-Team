using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerConnector : NetworkBehaviour {
    // This is in charge of handling spawning the player and some other client/server things
    // This is not for controlling the character

    public MenuController menu;

    [SerializeField]
    private int team;

	// Use this for initialization
	void Start () {
		// Is this my local PlayerConnector?
        if(!isLocalPlayer) {
            return;
        }

        // Tell the MenuController I, the client, did connect
        menu = GameObject.FindGameObjectWithTag("MenuController").GetComponent<MenuController>();
        menu.ConfirmConnect();
        CmdSpawnUnit();
	}

    public GameObject PlayerPrefab;
    private GameObject myUnit;

	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer) {
            // Not me
            return;
        }

        // Here we do some server wide things if we want
    }

    // Functions tagged [Command] are only executed on the server and must start with 'Cmd'
    [Command]
    void CmdSpawnUnit() {
        GameObject go = Instantiate(PlayerPrefab);
        NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
    }
}
