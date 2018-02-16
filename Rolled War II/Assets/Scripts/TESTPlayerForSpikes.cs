using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTPlayerForSpikes : MonoBehaviour {
    private Rigidbody rb;
    public GameObject SpikePit1;
    public GameObject SpikeDoor1;
    public float speed = 10.0f;
    public float hitpoints = 1000f;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        SpikePit1 = GameObject.Find("Spike Pit");
        SpikeDoor1 = SpikePit1.transform.GetChild(0).gameObject;
    }
	
	// Update is called once per frame
	void Update () {
        float move_horizontal = Input.GetAxis("Horizontal");
        float move_vertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(move_horizontal, 0, move_vertical);
        rb.AddForce(movement * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Spike Door")) {
            SpikeDoor1.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Spike Trigger")) {
            hitpoints = hitpoints - 0.1f;
        }
    }

  

   
}
