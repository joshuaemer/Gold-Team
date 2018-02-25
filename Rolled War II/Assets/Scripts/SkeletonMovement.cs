using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


//TODO
//Wont stop rotating when attacking
//Next need to add attack animation
public class SkeletonMovement : MonoBehaviour {
    private Animator anim;
    private float speed =5f;
    private NavMeshAgent nav;
   
    //Check is an empty static game object that needs to have children that are also static and empty. The AI will follow go to each child to create a path.
    public GameObject check;
    //Which direction the player will follow the objects cannot have an absoulute value greater then the number of check's child objects -1
    
    public int direction;
    int speedHash;
    int nextPoint = 0;

    //Raycasts Vars
    RaycastHit hit;
    int lineOfSight = 15;
    bool foundPlayer = false;
    bool isFacing = false;
    private GameObject target;
    // Use this for initialization
    void Start () {
        nav = GetComponent<NavMeshAgent>();
        speedHash = Animator.StringToHash("Speed");
        anim = GetComponent<Animator>();
        nav.speed = speed;

        
        //anim.SetBool("isMoving",isMoving);
        //anim.SetFloat("speed", speed);
    }

    // Update is called once per frame
    void Update () {
        //Checks if player is within line of sight if not move to the next checkpoint
        if (foundPlayer)
        {
            if (inRange(2, target.transform.position)&& !isFacing)
            {
                print("ATTACK");
                transform.LookAt(target.transform);
                isFacing = true;
                
            }
            else
            {
                print("Locating");
                nav.SetDestination(target.transform.position);
                isFacing = false;
            }
        }
        else
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                

                if (hit.transform.CompareTag("Player") && inRange(lineOfSight, hit.transform.position))
                {
                    foundPlayer = true;
                    target = hit.transform.gameObject;
                    nav.SetDestination(target.transform.position);
                    
                }
            }
            if (!foundPlayer)
            {
                
                if (inRange(1, check.transform.GetChild(nextPoint).transform.position))
                {

                    nextPoint += direction;
                    if (nextPoint > check.transform.childCount - 1)
                    {
                        nextPoint = 0;
                    }
                    else if (nextPoint < 0)
                    {
                        nextPoint = check.transform.childCount - 1;
                    }
                }

                anim.SetFloat(speedHash, 1);
                nav.SetDestination(check.transform.GetChild(nextPoint).position);

            }
        }
    }
    //Returns true if the enemy is atMost limit away from the otherPos in the x or y direction
    bool inRange(int limit, Vector3 otherPos)
    {
        Vector3 pos = transform.position;
        return Mathf.Abs(pos.x - otherPos.x) <= limit || Mathf.Abs(pos.z - otherPos.z) <= limit;
    }
}
