using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AIHandler : MonoBehaviour {
    
    //Make sure only one boss exists at a time
    
    //Make sure boss only one boss exists at a time
    //Boss should drop multiple objects
    
    



    public GameObject skeleton_prefab;
    //Prefabs for all dropable objects

    public GameObject speed_prefab;
    public GameObject health_prefab;
    public GameObject pistol_prefab;
    public GameObject sniper_prefab;
    public GameObject shotgun_prefab;
    public GameObject ak_prefab;
    public GameObject grenade_prefab;
    public GameObject smg_prefab;

    //The number of possible drops
    private int drop_count = 8;
    //Check Point object
    private GameObject check;

    //AI Info
    
    private int ai_limit = 7;
    private int ai_count =0;
    

    //RNG
    private System.Random rand;

    
    


	// Use this for initialization
	void Start () {
        check = transform.GetChild(0).gameObject;
        rand = new System.Random();
        create();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //Creates AI's up to ai_limit
    void create()
    {
        
        int rand_index;
        int rand_direction;
        //Since randoms upper bound is exclusive it is set to childCount not childCount -1;
        int bound = check.transform.childCount;
        GameObject monster;
        while(ai_count<ai_limit)
        {
            rand_index = rand.Next(0, bound);
            monster =Instantiate(skeleton_prefab, check.transform.GetChild(rand_index).gameObject.transform.position,Quaternion.identity);
            rand_direction = rand.Next((bound-1) * -1, bound);
            monster.GetComponent<SkeletonMovement>().Set_direction(rand_direction);
            
            ai_count += 1;
        }
    }

    //Signals the AI Handler that an AI has died
    public void Signal_death(Vector3 create_pos)
    {
        ai_count -= 1;
        
        GameObject drop = null;



        int id = rand.Next(0, drop_count);

        switch (id){
            case 0:
                drop = speed_prefab;
                break;
            case 1:
                drop = health_prefab;
                break;
            case 2:
                drop = pistol_prefab;
                break;
            case 3:
                drop = sniper_prefab;
                break;
            case 4:
                drop = shotgun_prefab;
                break;
            case 5:
                drop = ak_prefab;
                break;
            case 6:
                drop = grenade_prefab;
                break;
            case 7:
                drop = smg_prefab;
                break;
        }

        //Create drop here at create pos;
        //Call this function upon death in skeleton movement
        create_pos.y += 1;
        Instantiate(drop, create_pos,Quaternion.identity);
        
        
    }



    
}
