using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class FPController : NetworkBehaviour {

    public float speed = 3.0f;
    public float rotateSpeed = 3.0f;
    public float smoother = 10;
    public int hitpoints = 1000;
    private CharacterController controller;
    
    //Controls the elevators each elevator added needs both of theses vars drag each platform to the public 
    //object
    public GameObject platform1;
    private Vector3 platform1_start;
    

    // this is used to sync position and rotation of other players
    private Vector3 enemyPosition;
    private Quaternion enemyRotation;

    //Text for Player Stats
    public Text hitpointsText;
    public Text speedText;


    void Start() {
        controller = gameObject.GetComponent<CharacterController>();
        
        platform1 = GameObject.Find("Platform 1");
        platform1_start = platform1.transform.position;
        hitpointsText = GameObject.Find("Hit Points").GetComponent<Text>();
        speedText = GameObject.Find("Speed").GetComponent<Text>();
        hitpointsText.text = "HP = " + hitpoints.ToString();
        speedText.text = "Speed = " + speed.ToString();
    }

    // Update is called once per frame
    // Every client calls this on every player even if it isn't their's
    void Update () {
        if (!hasAuthority) {
            // Not the local client's player

            // sync the player's approximate position and rotation with some smoothing
            transform.position = Vector3.Lerp(transform.position, enemyPosition, Time.deltaTime * smoother);
            transform.rotation = Quaternion.Lerp(transform.rotation, enemyRotation, Time.deltaTime * smoother);
            return;
        }

        Camera.main.GetComponent<FollowPlayer>().setPlayer(transform);

        // We know this is OUR character to control so let's control it
        transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed, 0);
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        float curSpeed = speed * Input.GetAxis("Vertical");
        controller.SimpleMove(forward * curSpeed);
        CmdUpdatePlayer(transform.position, transform.rotation);
	}

    [Command]
    void CmdUpdatePlayer(Vector3 pos, Quaternion rot) {
        transform.position = pos;
        transform.rotation = rot;

        RpcUpdatePlayer(pos, rot);
    }

    [ClientRpc]
    void RpcUpdatePlayer(Vector3 pos, Quaternion rot) {
        enemyPosition = pos;
        enemyRotation = rot;
    }

    //This fucntion is used to interact with the environment
    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Bottom Trigger"))
        {


            if (platform1.transform.position.y == platform1_start.y)
            {

                platform1.GetComponent<PlatformBehavior>().Hit(0);

            }
        }

    }

}
