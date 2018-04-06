using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

    private uint lobbySize = 4;
    private string lobbyName;

    private bool inGame = false;
    private bool toggle = false;
    public GameObject mainCamera;
    public GameObject mainMenu;
    public GameObject playerMenu;
    public GameObject controllsMenu;
    public GameObject gameOver;
    public Text score;
    public GameObject lobbyListItemPrefab;
    public Transform lobbyListParent;
    private NetworkManager manager;

    private List<GameObject> lobbyList = new List<GameObject>();

    [SerializeField]
    private Text status;

    GameManager gm;

    void Start() {
        manager = NetworkManager.singleton;
        if(manager.matchMaker == null) {
            manager.StartMatchMaker();
        }

        RefreshLobbies();
    }

    public void RefreshLobbies() {
        ClearLobbyList();
        manager = NetworkManager.singleton;
        if (manager.matchMaker == null) {
            manager.StartMatchMaker();
        }
        manager.matchMaker.ListMatches(0, 20, "", false, 0, 0, OnMatchList);
        status.text = "Loading....";
    }

    public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches) {
        status.text = "";
        if(!success || matches == null) {
            status.text = "Couldn't get lobby list.";
            return;
        }

        foreach(MatchInfoSnapshot match in matches) {
            GameObject lobbyListItemGO = Instantiate(lobbyListItemPrefab);
            lobbyListItemGO.transform.SetParent(lobbyListParent);

            LobbyListItem lobbyListItem = lobbyListItemGO.GetComponent<LobbyListItem>();
            if (lobbyListItem != null) {
                lobbyListItem.Setup(match, JoinRoom);
            }


            // as well as setting up a callback function that will join the game.

            lobbyList.Add(lobbyListItemGO);
        }

        if (lobbyList.Count == 0) {
            status.text = "No rooms at the moment.";
        }
    }

    void ClearLobbyList() {
        for(int i = 0; i < lobbyList.Count; i++) {
            Destroy(lobbyList[i]);
        }
        lobbyList.Clear();
    }

    public void SetLobbyName(string name) {
        lobbyName = name;
    }

    public void CreateLobby() {
        if (lobbyName != "" && lobbyName != null) {
            manager.matchMaker.CreateMatch(lobbyName, lobbySize, true, "", "", "", 0, 0, manager.OnMatchCreate);
            inGame = true;
            mainMenu.SetActive(false);
            
        }
    }

    public void JoinRoom(MatchInfoSnapshot _match) {
        manager.matchMaker.JoinMatch(_match.networkId, "", "", "", 0, 0, manager.OnMatchJoined);
        StartCoroutine(WaitForJoin());
        inGame = true;
        mainMenu.SetActive(false);
    }

    IEnumerator WaitForJoin() {
        ClearLobbyList();

        int countdown = 5;
        while (countdown > 0) {
            status.text = "JOINING... (" + countdown + ")";

            yield return new WaitForSeconds(1);

            countdown--;
        }
        
        status.text = "Failed to connect.";
        yield return new WaitForSeconds(1);

        MatchInfo matchInfo = manager.matchInfo;
        if (matchInfo != null) {
            manager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, manager.OnDropConnection);
            manager.StopHost();
        }

        RefreshLobbies();

    }

    // Quit the Application
    public void Quit() {
        Application.Quit();
    }

    // Leave game or Stop Hosting a game
    public void Disconnect() {
        MatchInfo matchInfo = manager.matchInfo;
        manager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, manager.OnDropConnection);
        manager.StopHost();
        inGame = false;
        playerMenu.SetActive(false);
        mainCamera.transform.SetPositionAndRotation(new Vector3(0f, 50f, 0f), Quaternion.Euler(90, 0, 0));

        manager = NetworkManager.singleton;
        if (manager.matchMaker == null) {
            manager.StartMatchMaker();
        }

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

    public void GameOver() {
        playerMenu.SetActive(false);
        controllsMenu.SetActive(false);
        gameOver.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void ToMainMenu() {
        gameOver.SetActive(false);
        mainMenu.SetActive(true);
        mainCamera.SetActive(true);
    }

    // Wait for player to press ESC to bring up pause menu
    public void Update() {
        if(Input.GetKeyDown(KeyCode.Escape) && inGame) {
            playerMenu.SetActive(toggle);
            toggle = !toggle;
            controllsMenu.SetActive(false);
            score.text = "";
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player")) {
                score.text += go.GetComponent<FPController>().hitpoints +  "\n";
            }
        }
    }
}
