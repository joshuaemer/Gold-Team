using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class GunComponent : NetworkBehaviour {
    public int id;//The id of the weapon

    
    public GameObject inv;
    //Grenade vars
    public GameObject explosion;// Will be null for all other weapons, this is the prefab for the explosion.
    private int grenade_damage = 500;
    private float g_force = 5f; //Force applied when thrown
    private bool isThrown = false;
    private bool hasLanded = false;
    private Vector3 oldPos; // Position of grednade at the previous frame
    private bool isChild = true; // Is the grenade still a child of the player?
    private GameObject grenade; // The parent of this script's object
    private GameObject new_grenade; // The next grenade
    private float blast_scale = 7f;// Scale where damage will be done
    private bool damageDealt = false; // Make sure damage is only dealt once

    private float timer;

    //For creating the place holder object
    private Vector3 create_pos;
    private Quaternion create_rot;
    //Player who currently holds the weapon
    private GameObject _Player;
    // Use this for initialization
    void Start () {
        if (id == 5)
        {

            grenade = transform.parent.gameObject;
            

        }
	}
	
	// Update is called once per frame
	void Update () {

        if(inv == null && transform.parent.parent != null)
        {
            inv = transform.parent.parent.parent.GetChild(0).gameObject;
        }
        //Update is only used for grenades

       
        if (isThrown && id==5)
        {
            
            //Make sure the thrown grenade is no longer the player's child
            //Create a place holder object so the weapon can be switched right after.
            if (isChild && !InRange(1, _Player.transform.position))
            {
                grenade.transform.parent = null;
                isChild = false;
               
            }
            //Wait until the grenade has stopped moving

            else if (Time.time - timer >= 3)
            {
                print("IsMoving: " + IsMoving());
                print("hasLanded: " + hasLanded);

                //Remove the old grenades rigid body
                Destroy(grenade.GetComponent<Rigidbody>());
                //Set this just in case so that the next if block is not run on the next frame
                hasLanded = true;
                GameObject exp = Instantiate(explosion, grenade.transform.position, Quaternion.identity) as GameObject;
                ParticleSystem ps = exp.GetComponent<ParticleSystem>();
                ps.Play();
                grenade.transform.GetChild(0).gameObject.transform.localScale = new Vector3(1, 1, 1) * blast_scale;
                if (!damageDealt)
                {
                    DetectHit();
                    damageDealt = true;
                }
                
                //Debug.Break();
                Destroy(exp, ps.main.duration);

                
                Destroy(grenade, ps.main.duration);
            }

            //To handle if grenade hits an AI or Player and is pushed by it
            else if (!hasLanded) { 
                if (grenade.GetComponent<Rigidbody>().velocity.y == 0)
                {
                    hasLanded = true;
                }
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

    public void Throw_grenade(GameObject Player) {
        //Make sure a new grenade cannot be thrown yet

        //Function should not be called if the weapon is not a grenade
        if (id != 5) { return; }

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
        grenade.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
        grenade.transform.parent = null;
        isChild = false;

        CreatePlaceHolder();
        timer = Time.time;
    }

    //Throws the grenade will be called in fire in Inventory System
    //The player game object is passed so we know we are using the correct player object
    [Command]
    public void Cmd_Throw_grenade(GameObject Player)
    {
        //Make sure a new grenade cannot be thrown yet
       
        //Function should not be called if the weapon is not a grenade
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
        grenade.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
        grenade.transform.parent = null;
        isChild = false;

        CreatePlaceHolder();
        timer = Time.time;
    }

    private bool InRange(int limit, Vector3 otherPos)
    {
        Vector3 pos = transform.position;
        return (Mathf.Abs(pos.x - otherPos.x) <= limit || Mathf.Abs(pos.z - otherPos.z) <= limit) && Mathf.Abs(pos.y - otherPos.y) < 5;
    }

    //Creates a placeholder for the players weapon this is so weapon switching still works after a grenade is thrown.
    //Once the old grenade has landed this one needs to have its mesh renderer enabled
    private void CreatePlaceHolder()
    {
        new_grenade = Instantiate(inv.GetComponent<InventorySystem>().getGrenadePrefab(), new Vector3(0, 0, 0), create_rot) as GameObject;
        
        Destroy(new_grenade.transform.GetChild(0).GetComponent<BoxCollider>());
        new_grenade.transform.parent = _Player.transform;
        new_grenade.transform.SetSiblingIndex(0);
        new_grenade.transform.localPosition = create_pos;
        new_grenade.transform.localRotation = create_rot;

        new_grenade.transform.localRotation = Quaternion.Euler(0, 90, 0);
        //new_grenade.name = "new g";
        //if there are no grenades left the object still needs to be created however don't show it
        if (!inv.GetComponent<InventorySystem>().HasAmmoLeft(5))
        {
            new_grenade.GetComponent<MeshRenderer>().enabled = false;
        }
        
    }

    private bool IsMoving()
    {
        if (grenade.GetComponent<Rigidbody>() == null)
        {
            return true;
        }
        else
        {
            return grenade.GetComponent<Rigidbody>().velocity.magnitude > 0;
        }
    }
    
    //Detects what is within range of the blast and deals damage accordingly
    private void DetectHit()
    {
        Collider[] hitColliders = Physics.OverlapBox(grenade.transform.position, grenade.transform.GetChild(0).transform.localScale / 2, Quaternion.identity);
        for(int i = 0; i<hitColliders.Length; ++i)
        { GameObject current = hitColliders[i].gameObject;
            if (current.CompareTag("Monster")){
                current.GetComponent<SkeletonMovement>().TakeDamage(grenade_damage);
            }

            else if (current.CompareTag("Player")&& current.GetInstanceID() != _Player.GetInstanceID())
            {
                current.GetComponent<FPController>().TakeDamage(grenade_damage);
            }
        }
    }
    private void OnDestroy()
    { 
        if (inv != null)
        {
            inv.GetComponent<InventorySystem>().setNextGrenade(true);
        }
    }
}
