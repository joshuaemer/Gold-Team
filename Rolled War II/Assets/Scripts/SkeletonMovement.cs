using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Note to self Josh, Boss is not going to attack state.

public class SkeletonMovement : MonoBehaviour {
    private Animator anim;
    public float speed;
    public int damage;
    
    private NavMeshAgent nav;
    public int hitpoints;
    public bool isBoss;
    private GameObject AIHandler;
    //Check is an empty static game object that needs to have children that are also static and empty. The AI will follow go to each child to create a path.
    private GameObject check;
    //Which direction the AI will follow the objects cannot have an absoulute value greater then the number of check's child objects -1
    
    private int direction;
    int speedHash;
    int attackHash;
    int deathHash;
    int nextPoint = 0;

    bool dead = false;
    //Raycasts Vars
    RaycastHit hit;
    int lineOfSight = 20;
    bool foundPlayer = false;
    bool isFacing = false;
    //Offsets the ray cast 
    private float offset;
    

    
    //Target is set to whatever player is seen first
    private GameObject target;

    //Time before damage is dealt
    private float timeWaited = 0;
    
    // Use this for initialization
    void Start () {
        AIHandler = GameObject.Find("AIMasterHandler").gameObject;
        if (isBoss)
        {
            check = AIHandler.transform.GetChild(1).gameObject;
            offset = 1.0f;
            
        }
        else
        {
            check = AIHandler.transform.GetChild(0).gameObject;
            offset = 0.5f;
            
        }
        
        nav = GetComponent<NavMeshAgent>();
        speedHash = Animator.StringToHash("Speed");
        attackHash = Animator.StringToHash("Attack");
        deathHash = Animator.StringToHash("isDead");
        anim = GetComponent<Animator>();
        nav.speed = speed;
        
       
    }

    // Update is called once per frame
    void LateUpdate () {
        //Checks if player is within line of sight if not move to the next checkpoint
        if (foundPlayer && target!=null)
        {
            print("found");
            if (InRange(2, target.transform.position))
            {
                //If the skeleton is not facing the player then make it face the player
                if (!isFacing)
                {

                    transform.LookAt(target.transform);
                    isFacing = true;

                }
                //If the the animator is in the attack state start to damage the player
                else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                {
                    
                    //Wait until the animation is over
                    if(timeWaited- Time.deltaTime >= 2*anim.GetCurrentAnimatorClipInfo(0).Length && InRange(2, target.transform.position))
                    {
                        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position + new Vector3(1,0,1), transform.localScale / 2, Quaternion.identity);
                        for(int i =0; i<hitColliders.Length; ++i) {
                            if (hitColliders[i].gameObject.CompareTag("Player"))
                            {
                                target.GetComponent<FPController>().TakeDamage(damage);
                            }
                        }
                       
                        timeWaited = 0;
                    }
                    else
                    {
                        timeWaited += Time.deltaTime;
                    }
                }
                else
                {   //Order is important here, it needs to go from stand run state->stand->attack
                    anim.SetFloat(speedHash, -1);

                    anim.SetBool(attackHash, true);
                    timeWaited = 0;
                }
            }

            else
            {
                
                nav.SetDestination(target.transform.position);
                isFacing = false;
                timeWaited = 0;

                //Order is important here, it needs to go from stand run attack->stand->run
                anim.SetBool(attackHash, false);
                anim.SetFloat(speedHash, speed);
                
            }
        }
        else
        {
            Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + offset, transform.position.z), transform.forward.normalized * lineOfSight, Color.green);
            if (Physics.Raycast(new Vector3(transform.position.x,transform.position.y+offset,transform.position.z), transform.forward, out hit,lineOfSight))
            {
                


                if (hit.transform.CompareTag("Player"))
                {

                    foundPlayer = true;
                    target = hit.transform.gameObject;
                    nav.SetDestination(target.transform.position);

                }
                //Boss has 2 casts this is beacuse the boss is tall
                else if (isBoss)
                {
                    Debug.DrawRay(new Vector3(transform.position.x, transform.position.y -2, transform.position.z), transform.forward.normalized * lineOfSight, Color.green);
                    if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y -2, transform.position.z), transform.forward, out hit, lineOfSight))
                    {


                        if (hit.transform.CompareTag("Player"))
                        {

                            foundPlayer = true;
                            target = hit.transform.gameObject;
                            nav.SetDestination(target.transform.position);

                        }

                    }
                }
            }
            if (!foundPlayer)
            {
                
                if (InRange(1, check.transform.GetChild(nextPoint).transform.position))
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

                anim.SetFloat(speedHash, speed);
                nav.SetDestination(check.transform.GetChild(nextPoint).position);

            }
        }
        
        
    }

    //Returns true if the enemy is atMost limit away from the otherPos in the x or y direction
    private bool InRange(int limit, Vector3 otherPos)
    {
        Vector3 pos = transform.position;
        return (Mathf.Abs(pos.x - otherPos.x) <= limit && Mathf.Abs(pos.z - otherPos.z) <= limit) && Mathf.Abs(pos.y - otherPos.y)<5;
    }

    public void TakeDamage(int damage)
    {
        
        hitpoints -= damage;
        
        if (hitpoints <= 0)
        {   //Switch to death state
            anim.SetBool(deathHash, true);
            //Destroy this object after death animation
            Vector3 last_pos = transform.position;
            if (!dead)
            {
                AIHandler.GetComponent<AIHandler>().Signal_death(last_pos,isBoss);
                Destroy(gameObject, gameObject.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length + 1.15f);
                //Let the game object know this has died
                dead = true;
            }
           
            

        }
    }

    //Used by AI handler sets the direction that the AI will follow the checkpoints
    public void Set_direction(int d) {
        direction = d;
    }
}
