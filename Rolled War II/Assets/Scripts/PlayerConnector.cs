﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerConnector : NetworkBehaviour {
    // This is in charge of handling spawning the player and some other client/server things
    // This is not for controlling the character

    public MenuController menu;
    public int score = 0;
    private GameObject myPlayer;
	// Use this for initialization
	void Start () {
		// Is this my local PlayerConnector?
        if(!isLocalPlayer) {
            return;
        }
        // Tell the MenuController I, the client, did connect
        menu = GameObject.FindGameObjectWithTag("MenuController").GetComponent<MenuController>();
        CmdSpawnUnit();
    }

    public GameObject PlayerPrefab;

    // Functions tagged [Command] are only executed on the server and must start with 'Cmd'
    [Command]
    void CmdSpawnUnit() {
        GameObject go = Instantiate(PlayerPrefab);
        NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
        RpcAssignPlayer(go);
    }

    [ClientRpc]
    void RpcAssignPlayer(GameObject go) {
        if (isLocalPlayer) {
            myPlayer = go;
            menu.Connect();
        }
    }
}
