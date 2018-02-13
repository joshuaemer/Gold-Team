using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Add this script to the Main Camera
//This script assumes you have already set the camera's desired postition and rotation
public class CameraController : MonoBehaviour
{
    GameObject player = null;
    private Vector3 offset;
    
    void Start()
    {
        
    }

    public void SetPlayer(GameObject p) {
        player = p;
        offset = transform.position - player.transform.position;
    }

    // LateUpdate is called once per frame last
    void LateUpdate()
    {
        if(player != null) {
            transform.position = player.transform.position + offset;
        }
    }
}

