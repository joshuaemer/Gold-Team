using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour {
    [SyncVar]
    string gameType = " ";

    [SyncVar]
    int numPlayers = 0;

    public Text text;

    public Dictionary<int, int> playerDict = new Dictionary<int, int>();

    public NetworkManager manager;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!isServer){return;}
        numPlayers = manager.numPlayers;
        if (!playerDict.ContainsKey(numPlayers)) {
            if (gameType.Equals("TDM")) {
                playerDict.Add(numPlayers, numPlayers % 2);
            } else {
                playerDict.Add(numPlayers, numPlayers);
            }
        }
	}

    public void SetGameType(string type) {
        gameType = type;
    }

    public string GetGameType()
    {
        return gameType;
    }

    public int GetNumPlayers() {
        return numPlayers;
    }
}
