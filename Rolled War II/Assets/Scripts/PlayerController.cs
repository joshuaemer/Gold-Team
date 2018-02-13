using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
    private Rigidbody rb;
    GameObject MyCamera;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        MyCamera = GameObject.FindGameObjectWithTag("MainCamera");
        MyCamera.GetComponent<CameraController>().SetPlayer(this.gameObject);
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (hasAuthority) {
            float move_horizontal = Input.GetAxis("Horizontal");
            float move_vertical = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(move_horizontal, 0, move_vertical);
            rb.AddForce(movement * 10);
        }
    }

}
