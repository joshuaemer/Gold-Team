﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class AIHandler : MonoBehaviour {
   
    
    
    


    public GameObject skeleton_Boss_prefab;
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
    private GameObject check_Boss;

    //AI Info
    
    private int ai_limit = 2;
    private int ai_count =0;
    private int wave = 1;
    private int old_wave;

    //Text
    private Text AI_Text;
    private bool initSet;
    

    //RNG
    private System.Random rand;

    
    


	// Use this for initialization
	void Start () {
        AI_Text = transform.GetChild(2).gameObject.transform.GetComponentInChildren<Text>();
        check = transform.GetChild(0).gameObject;
        check_Boss = transform.GetChild(1).gameObject;
        rand = new System.Random();
        create(false);
        
        old_wave = wave;
	}
	
	// Update is called once per frame
	void Update () {
        //Sets AI text when player is spawned
        if (!initSet)
        {
            if (GameObject.FindGameObjectWithTag("Player") != null)
            {
                UpdateText(false);
                initSet = true;
            }
        }
        //This means there are currently no AI in play
		if(ai_count == 0)
        {
            //Then spawn Boss, Wave gets incremented when all Bosses are killed
            if(old_wave == wave)
            {
                create(true);
                UpdateText(true);

            }
            else
            {
                create(false);
            }
        }
	}

    //Creates AI's up to ai_limit or Bosses up to wave # depending on isBoss
    void create(bool isBoss)
    {
        
        int rand_index;
        int rand_direction;
        int limit;
        int bound;
        GameObject spawn;
        GameObject points;
        GameObject monster;
        //Since randoms upper bound is exclusive it is set to childCount not childCount -1;
        if (isBoss)
        {
            points = check_Boss;
            spawn = skeleton_Boss_prefab;
            bound = check_Boss.transform.childCount;
            limit = wave;
        }
        else
        {
            points = check;
            spawn = skeleton_prefab;
            bound = check.transform.childCount;
            limit = ai_limit;
        }
        
        while(ai_count<limit)
        {
            rand_index = rand.Next(0, bound);
            monster =Instantiate(spawn, points.transform.GetChild(rand_index).gameObject.transform.position,Quaternion.identity);
            rand_direction = rand.Next((bound-1) * -1, bound);
            monster.GetComponent<SkeletonMovement>().Set_direction(rand_direction);
            
            ai_count += 1;
        }
    }

    //Signals the AI Handler that an AI has died
    public void Signal_death(Vector3 create_pos,bool isBoss)
    {
        ai_count -= 1;
        if (!isBoss)
        {
            UpdateText(false);
        }
        else if( isBoss && ai_count == 0)
        {
            old_wave = wave;
            wave += 1;
            ai_limit = (int)(ai_limit * 1.5);
            UpdateText(false);
        }
       
        
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

    //Updats text to current values
    private void UpdateText(bool isBoss) {
        String last;
        if (isBoss)
        {
            last = "Boss";
        }
        else
        {
            last = ai_count.ToString();
        }
        AI_Text.text = "Wave: " + wave.ToString() + " Monsters Remaining: " + last;
    }
    
}
