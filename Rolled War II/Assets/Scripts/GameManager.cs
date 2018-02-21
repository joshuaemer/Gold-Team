using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour {
    [SyncVar]
    string gameType;

    [SyncVar]
    int numPlayers;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!isServer){return;}
        // we know this is the server
        numPlayers = Network.connections.Length;
        print(numPlayers);
	}

    public void SetGameType(string type) {
        gameType = type;
    }

    public string GetGameType()
    {
        return gameType;
    }
}
