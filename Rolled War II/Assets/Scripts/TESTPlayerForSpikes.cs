using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTPlayerForSpikes : MonoBehaviour {
    private Rigidbody rb;

    
    public GameObject SpikeDoor1;
    public GameObject SpikeExit1;


    public float speed = 10.0f;
    public float hitpoints = 1000f;
    public GameObject spike_platform1;
    private Vector3 spike_platform1_start;


    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
         
        SpikeDoor1 = GameObject.Find("Spike Pit").transform.GetChild(0).gameObject;
        SpikeExit1 = GameObject.Find("Spike Pit").transform.GetChild(1).gameObject;
        spike_platform1 = GameObject.Find("Spike Platform 1");
        spike_platform1_start = spike_platform1.transform.position;
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
        if (other.CompareTag("Spike Door"))
        {
            SpikeDoor1.SetActive(false);
        }

        else if (other.CompareTag("Exit Trigger"))
        {
            SpikeExit1.SetActive(false);
        }

        else if (other.gameObject.CompareTag("Bottom Trigger"))
        {


            if (spike_platform1.transform.position.y == spike_platform1_start.y)
            {

                spike_platform1.GetComponent<PlatformBehavior>().Hit(0);

            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Spike Trigger")) {
            hitpoints = hitpoints - 0.1f;
            SpikeDoor1.SetActive(true);
        }
    }

  

   
}
