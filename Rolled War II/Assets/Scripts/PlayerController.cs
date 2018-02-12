using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Set bottom Trigger's tag to "Bottom Trigger"
//platform1 must be dragged to the correct spot in the script component or it will not work
//If getting an error on line "pb1.up = true;" comment it out then do the above

public class PlayerController : MonoBehaviour {
    private Rigidbody rb;
    public GameObject platform1;
    private Vector3 platform_start;
    
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        platform_start = platform1.transform.position;
        
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        float move_horizontal = Input.GetAxis("Horizontal");
        float move_vertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(move_horizontal, 0, move_vertical);
        rb.AddForce(movement * 10);
    }
    void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Bottom Trigger"))
        {
            print("HERE");
            if (platform1.transform.position.y == platform_start.y)
            {
                platform1.GetComponent<PlatformBehavior>().setBoolUp(true);
                print("SET");
            }
        }
    }
}
