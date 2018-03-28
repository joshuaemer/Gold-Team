using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Add this script to the Main Camera
//This script assumes you have already set the camera's desired postition and rotation
public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;
    
    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    // LateUpdate is called once per frame last
    void LateUpdate()
    {
        transform.position = player.transform.position + offset;

    }
}

