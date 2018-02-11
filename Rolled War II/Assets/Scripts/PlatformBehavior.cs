using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlatformBehavior : MonoBehaviour {
    
    private Rigidbody rb;
    
    public GameObject shaft;
    private bool up;
    //public Text test;
    
    private int speed;

    // Use this for initialization
    void Start () {
        
        rb = GetComponent<Rigidbody>();
        up = false;
        
        speed = 5;
    }
	
	// Update is called once per frame
	void Update () {
        if (up)
        {
            
            if (transform.position.y < shaft.transform.lossyScale.y)
            {
                rb.AddForce(new Vector3(0, speed, 0));
            }
            
        }
        else
        {
            up = false;
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bottom Trigger")) {
            if(transform.position.y == 0) {
                up = true;
            }
        }
    }

}
