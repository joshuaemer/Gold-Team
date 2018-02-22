using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < 2; ++i)
        {
            transform.Rotate(new Vector3(45,0, 0) * Time.deltaTime);
        }
    }
}
