using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlatformBehavior : MonoBehaviour {
    
    
    
    public GameObject shaft;
    
    private bool up; //Tell Platform to go up
    private bool down; //Tell Platform to go down

    //Bellow is Used to differenate between triggers caused when already on the elevator
    //See Hit for example usage 
    private bool upActive; 
    

    private Vector3 start_position;

    private int waitFramesDown; //Elevator waits for this many frames before going back down
    private int framesWaitedDown;//Number of frames waited while in up position
    private int waitFramesUp;//Makes Elevator wait before going up again
    private int frameWaitedUp;
    private int waitFramesReset;
    private int framesWaitedReset;

    private float speed;//Elevator speed when going up
    private float downspeed;//Elevator speed going down.
    private bool hasUpBeenSet; //Prevents elevator from imediatly going back up after 1st use
    



    // Use this for initialization
    void Start () {
        framesWaitedDown = 0;
        waitFramesDown = 420;
        
        framesWaitedReset = 0;
        waitFramesReset = 500;
        up = false;
        down = false;
        
        
        speed = (float)0.1;
        downspeed = (float)0.04;
        start_position = transform.position;
        hasUpBeenSet = false;
    }
	
	// Update is called once per frame
	void LateUpdate () {

        //Resets elevator hasUp been set bool to prevent having to wait for elevator every time

        if (hasUpBeenSet) {
            if (framesWaitedReset != waitFramesReset)
            {
                framesWaitedReset += 1;
            }
            else
            {
                hasUpBeenSet = false;
            }
        }

        else
        {
            framesWaitedReset = 0;

        }
        if (up)
        {
            if (!hasUpBeenSet || framesWaitedDown == waitFramesDown)
            {
                if (transform.position.y < shaft.transform.lossyScale.y - transform.lossyScale.y - 0.25)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y + speed, transform.position.z);
                }


                else
                {
                    up = false;
                    down = true;
                    hasUpBeenSet = true;
                    framesWaitedDown = 0;


                }

            }
            else {
                framesWaitedDown += 1;
            }
        }

        else if (down)
        {
            if (transform.position.y > start_position.y)
            {
                if (waitFramesDown == framesWaitedDown)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y - downspeed, transform.position.z);
                }
                else
                {
                    framesWaitedDown += 1;
                }
            }


            else
            {
                transform.position = start_position;
                framesWaitedDown = 0;
                

            }
        }
    }
    //Signals platform that a trigger has been hit
    //int id, 0 is for bottom trigger
    public void Hit(int id)
    {
        if (id == 0)
        {
            
            up = true;
                
                
            
            
        
        }
    }

}
