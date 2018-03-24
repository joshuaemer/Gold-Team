using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//3/24/2018
//Josh: Note to self TODO call create Place Holder after current grenade is no longer a player child
//Make sure weapon switching still works
//Then set inv next greadnade to true so it can fire a grenade again.
//Next steps will be explsion and damage


public class GunComponent : MonoBehaviour {
    public int id;

    private GameObject inv;
    //Grenade vars
    private int grenade_damage = 500;
    private float g_force = 5f;
    private bool isThrown = false;
    private bool hasLanded = false;
    private Vector3 oldPos;
    private bool isChild = true;
    private GameObject grenade;
    private GameObject new_grenade;
    //For creating the place holder object
    private Vector3 create_pos;
    private Quaternion create_rot;
    //Player who currently holds the weapon
    private GameObject _Player;
    // Use this for initialization
    void Start () {
        //Even thought this is set for every weapon this will be used when id == 5
        grenade = transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        if(inv == null)
        {
            inv = GameObject.Find("Inventory");
        }
        //Update is only used for grenades
		if(isThrown && id==5)
        {
            //Make sure the thrown grenade is no longer the player's child
            //Create a place holder object so the weapon can be switched right after.
            if (isChild && !InRange(1,_Player.transform.position))
            {
                grenade.transform.parent = null;
                isChild = false;
                //CreatePlaceHolder(grenade);
            }
            //Wait until the grenade has stopped moving
            if (oldPos == grenade.transform.position)
            {
                hasLanded = true;
                //Give the grenade trigger a collider again
                this.gameObject.AddComponent<BoxCollider>();
                print("Landed");
            }



            oldPos = grenade.transform.position;
        }
	}

    public int GetID()
    {
        return id;
    }

    public void setID(int new_id)
    {
        id = new_id;
    }
    

    //Throws the grendade will be called in fire in Inventory System

    public void Throw_grenade(GameObject Player)
    {
        //Make sure a new grenade cannot be thrown yet
       
        //Fucntion should not be called if the weapon is not a grenade
        if(id != 5) { return; }

        _Player = Player;
       create_pos = grenade.transform.localPosition;
       create_rot = grenade.transform.localRotation;

        
        
        
        //Giving the object a rigid body so that we may add force to it
        grenade.gameObject.AddComponent<Rigidbody>();
        //Finding the objects position and adding to its y to put an angle on it.
        Vector3 dir = _Player.transform.forward;
        dir.y += 1;

        grenade.gameObject.GetComponent<Rigidbody>().AddForce(dir * g_force, ForceMode.Impulse);
        isThrown = true;
    }

    private bool InRange(int limit, Vector3 otherPos)
    {
        Vector3 pos = transform.position;
        return (Mathf.Abs(pos.x - otherPos.x) <= limit || Mathf.Abs(pos.z - otherPos.z) <= limit) && Mathf.Abs(pos.y - otherPos.y) < 5;
    }

    //Creates a placeholder for the players weapon this is so weapon switching stills works after a grenade is thrown.
    private void CreatePlaceHolder(GameObject org)
    {
        new_grenade = Instantiate(inv.GetComponent<InventorySystem>().getGrenadePrefab()) as GameObject;
        new_grenade.GetComponent<MeshRenderer>().enabled = false;
        new_grenade.transform.parent = _Player.transform;
        new_grenade.transform.SetSiblingIndex(0);
        new_grenade.transform.localPosition = create_pos;
        new_grenade.transform.localRotation = create_rot;

        new_grenade.transform.localRotation = Quaternion.Euler(0, 90, 0);
    }
}
