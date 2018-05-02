using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class FPController : NetworkBehaviour {

    public float speed = 10.0f;
    private float init_speed;
    public float rotateSpeed = 3.0f;
    public float smoother = 10;
    public int MaxHealth;
    private InventorySystem inv;
    public float hSensitivity = 2.0f;
    public float vSensitivity = 2.0f;
    public float yaw = 0;
    public float pitch = 0;
    public int hitpoints = 1000;

    private CharacterController controller;
    private MenuController menu;
    private NetworkManager manager;

    public Camera playerCamera;

    // this is used to sync position and rotation of other players
    private Vector3 enemyPosition;
    private Quaternion enemyRotation;

    //Text for Player Stats
    public Text hitpointsText;
    public Text speedText;

    //Speed Timer
    private int speedFrameWait = 500;
    private int speedFrameWaited;


    void Start() {
        controller = gameObject.GetComponent<CharacterController>();
        menu = GameObject.FindGameObjectWithTag("MenuController").GetComponent<MenuController>();
        
        hitpointsText = GameObject.Find("Hit Points").GetComponent<Text>();
        speedText = GameObject.Find("Speed").GetComponent<Text>();
        hitpointsText.text = "HP = " + hitpoints.ToString();
        speedText.text = "Speed = " + speed.ToString();
        inv = transform.GetComponentInChildren<InventorySystem>();

        manager = NetworkManager.singleton;
        if (manager.matchMaker == null) {
            manager.StartMatchMaker();
        }

        ConnectionConfig myConfig = new ConnectionConfig();
        myConfig.AddChannel(QosType.ReliableSequenced);
        myConfig.MaxCombinedReliableMessageCount = 20;
        myConfig.MaxCombinedReliableMessageSize = 500;

        init_speed = speed;
        MaxHealth = hitpoints;
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

        // We know this is OUR character to control so let's control it
        transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed, 0);
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        float curSpeed = speed * Input.GetAxis("Vertical");
        controller.SimpleMove(forward * curSpeed);
        CmdUpdatePlayer(transform.position, transform.rotation);

        
        if (Math.Abs(pitch - vSensitivity * Input.GetAxis("Mouse Y")) < 90) {
            pitch -= vSensitivity * Input.GetAxis("Mouse Y");
            playerCamera.transform.localEulerAngles = new Vector3(pitch, yaw, 0);
        }
        if (Math.Abs(yaw + hSensitivity * Input.GetAxis("Mouse X")) < 50){
            yaw += hSensitivity * Input.GetAxis("Mouse X");
            playerCamera.transform.localEulerAngles = new Vector3(pitch, yaw, 0);
        }

        speedText.text = "Speed = " + speed.ToString();

        if(inv == null)
        {
            inv = transform.GetComponentInChildren<InventorySystem>();
        }
        

        if(speed > init_speed)
        {
            if(speedFrameWaited<speedFrameWait)
            {
                ++speedFrameWaited;
            }

            else
            {
                speedFrameWaited = 0;
                speed = init_speed;

            }
        }
    }

    public bool HasAuthority() {
        return hasAuthority;
    }

    [Command]
    void CmdUpdatePlayer(Vector3 pos, Quaternion rot) {
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
            

            if (other.transform.parent.GetChild(1).gameObject.transform.position.y == other.transform.parent.GetComponentInChildren<PlatformBehavior>().start_position.y)
            {
                
                other.transform.parent.GetComponentInChildren<PlatformBehavior>().Hit(0);

            }
        }

        else if (other.CompareTag("Spike Door"))
        {
            other.transform.parent.gameObject.SetActive(false);
        }

        else if (other.CompareTag("Exit Trigger"))
        {
            other.transform.parent.gameObject.SetActive(false);
        }

        else if (other.gameObject.CompareTag("Bottom Trigger Spike"))
        {


            if (other.transform.parent.GetChild(1).gameObject.transform.position.y == other.transform.parent.GetComponentInChildren<PlatformBehavior>().start_position.y)
            {

                other.transform.parent.GetComponentInChildren<PlatformBehavior>().Hit(0);

            }
        }

        else if (other.CompareTag("Speed")) {
            other.gameObject.SetActive(false);

            speed +=5;
            speedFrameWaited = 0;
        }

        else if (other.CompareTag("Health"))
        {
            if (hitpoints < MaxHealth)
            {
                hitpoints = Math.Min(hitpoints+100,MaxHealth);
                hitpointsText.text = "HP = " + hitpoints.ToString();

            }
            other.transform.parent.gameObject.SetActive(false);
        }
        else if (other.CompareTag("Weapon"))
        {   //Add the gun to the inventory
            if (!hasAuthority) { return; }
            inv.GetComponent<InventorySystem>().Add(other.GetComponent<GunComponent>().GetID());
            
            //Since other is the trigger we need to get rid of the entire game object
            Destroy(other.transform.parent.gameObject);
        }

    }

    void OnDestroy() {
        if (hasAuthority) {
            menu.GameOver();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Spike Trigger"))
        {

            TakeDamage(1);
            other.transform.parent.GetChild(0).gameObject.SetActive(true);
        }
    }


    public void TakeDamage(int damage) {
        if(!isServer) { return; }

        hitpoints -= damage;
        hitpointsText.text = "HP: " + hitpoints;
        if (hitpoints <= 0) {
            Destroy(gameObject);
        }
    }

    //Resets speed to init speed or sets speed to init speed/2
    public void setSpeed(bool reset)
    {
        if (reset)
        {
            speed = init_speed;
        }
        else {
            speed = init_speed / 2;
        }
        speedText.text = "Speed = " + speed.ToString();
    }

    
}
