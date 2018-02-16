using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTCameraSpikes : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;
    // Use this for initialization
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
