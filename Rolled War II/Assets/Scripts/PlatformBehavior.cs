using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlatformBehavior : MonoBehaviour {
    
    
    
    public GameObject shaft;
    private bool up;
    
    
    private float speed;

    // Use this for initialization
    void Start () {
        
        
        up = false;
        
        speed = (float)0.1;
    }
	
	// Update is called once per frame
	void LateUpdate () {
        if (up)
        {
            print("up is true");
            if (transform.position.y < shaft.transform.lossyScale.y-transform.lossyScale.y)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + speed, transform.position.z);
            }
            
        }
        else
        {
            up = false;
        }
	}

    public void setBoolUp(bool b)
    {
        up = b;
    }

}
