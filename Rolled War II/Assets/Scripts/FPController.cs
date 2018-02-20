﻿using System.Collections;
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
    public int hitpoints = 1000;
    private CharacterController controller;
    
    
    
    
    
    

    

    // this is used to sync position and rotation of other players
    private Vector3 enemyPosition;
    private Quaternion enemyRotation;

    //Text for Player Stats
    public Text hitpointsText;
    public Text speedText;

    //Speed Timer
    private int speedFrameWait = 500;
    private int speedFrameWaited;

    //Inventory
    public GameObject Inv;

    void Start() {
        controller = gameObject.GetComponent<CharacterController>();
        
        
        hitpointsText = GameObject.Find("Hit Points").GetComponent<Text>();
        speedText = GameObject.Find("Speed").GetComponent<Text>();
        hitpointsText.text = "HP = " + hitpoints.ToString();
        speedText.text = "Speed = " + speed.ToString();
        Inv = GameObject.Find("Inventory");
        
        
        
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

        Camera.main.GetComponent<FollowPlayer>().setPlayer(transform);

        // We know this is OUR character to control so let's control it
        transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed, 0);
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        float curSpeed = speed * Input.GetAxis("Vertical");
        controller.SimpleMove(forward * curSpeed);
        CmdUpdatePlayer(transform.position, transform.rotation);
        hitpointsText.text = "HP = " + hitpoints.ToString();
        speedText.text = "Speed = " + speed.ToString();

        if(speed != init_speed)
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




            other.transform.parent.gameObject.GetComponentInChildren<PlatformBehavior>().Hit(0);
                

            
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




            other.transform.parent.gameObject.GetComponentInChildren<PlatformBehavior>().Hit(0);



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

            }
            other.transform.parent.gameObject.SetActive(false);
        }

        else if (other.CompareTag("Weapon"))
        {
            
            int id = other.gameObject.GetComponent<GunComponent>().id;
            Inv.GetComponent<InventorySystem>().Add(id);
            other.transform.parent.gameObject.SetActive(false);
            print("HIT " + id.ToString());
        }
        else if (other.CompareTag("Grenade"))
        {
            
            Inv.GetComponent<InventorySystem>().Add(5);
            other.gameObject.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Spike Trigger"))
        {

            
            hitpoints -= 1;
            other.transform.parent.GetChild(0).gameObject.SetActive(true);
        }
    }

}
