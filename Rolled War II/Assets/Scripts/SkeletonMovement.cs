using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMovement : MonoBehaviour {
    //private Animator anim;
    private float speed =10f;
    
    private bool isMoving = false;
    // Use this for initialization
    void Start () {
        
        transform.position = new Vector3(0,0,0);
        //anim = GetComponent<Animator>();
        
        //anim.SetBool("isMoving",isMoving);
        //anim.SetFloat("speed", speed);
    }

    // Update is called once per frame
    void Update () {
        transform.position =new Vector3(Input.GetAxis("Horizontal"), 0,0);
        
        float curSpeed = speed * Input.GetAxis("Vertical");
        isMoving = true;
        
        
        
        isMoving = false;
    }
}
