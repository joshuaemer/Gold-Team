using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ManageTeams : NetworkBehaviour {
    // this will assign teams when the player connects based on the game mode
    [SerializeField]
    private NetworkManager manager;

    public Text text;
    
    void Update() {
        if (isServer) {

        CmdNumPlayers();
        }
    }

    [Command]
    void CmdNumPlayers() {
        RpcNumPlayers();
    }

    [ClientRpc]
    void RpcNumPlayers() {
        text.text = manager.numPlayers + "";
    }
}
