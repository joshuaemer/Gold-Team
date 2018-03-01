using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InventorySystem : MonoBehaviour {
    //keeps track of what guns are in each slot based on id
    //Pistol: 0
    //Shotgun: 1
    //Sniper: 2
    //AK: 3
    //SMG: 4
    //Grenades: 5
   
    private string[] slots = { "", "", "", "", "",""};
    //Maps gun id to index, current clip and current ammo amount
    private int free_slot;
    private Hashtable map;
    private GameObject Player;
    private int current = 0;

    //Ammo limits
   private int pistolLimit = 100;
   private int shotgunLimit = 100;
   private int sniperLimit = 100;
   private int arLimit = 100;
   private int smgLimit = 100;
   private int grenadeLimit = 6;

    //clip sizes
   private int pistolClip = 50;
   private int shotgunClip = 50;
   private int sniperClip = 50;
   private int arClip = 50;
   private int smgClip = 50;


    //Objects for each gun prefab
    public GameObject pistol_prefab;
    public GameObject shotgun_prefab;
    public GameObject sniper_prefab;
    public GameObject ak_prefab;
    public GameObject smg_prefab;
    public GameObject grenade_prefab;

    //Offsets for switching the guns else they end up in wack posistions
    private Vector3 pistol_offset = new Vector3(-1.34f, -2.76f, -0.22f);
    private Quaternion pistol_rot_offset= Quaternion.Euler(-1.101f, -26.355f, 2.125f);
    private Vector3 shotgun_offset = new Vector3(0, 0, 0);
    private Quaternion shotgun_rot_offset = Quaternion.Euler(-1.101f, -26.355f, 2.125f);
    private Vector3 sniper_offset = new Vector3(0, 0, 0);
    private Quaternion sniper_rot_offset = Quaternion.Euler(-1.101f, -26.355f, 2.125f);
    private Vector3 ak_offset = new Vector3(0, 0, 0);
    private Quaternion ak_rot_offset = Quaternion.Euler(-1.101f, -26.355f, 2.125f);
    private Vector3 smg_offset = new Vector3(0, 0, 0);
    private Quaternion smg_rot_offset = Quaternion.Euler(-1.101f, -26.355f, 2.125f);
    private Vector3 grenade_offset = new Vector3(0, 0, 0);
    private Quaternion grenade_rot_offset = Quaternion.Euler(-1.101f, -26.355f, 2.125f);

    private Hashtable pos_offsets;
    private Hashtable rot_offsets;

    //Formats string to display on screen based on gun id.
    string format(int id)
    {
        
        string ammo = "";
        if (id != 5)
        {
            ammo = ((ArrayList)map[id])[1].ToString() + "/" + ((ArrayList)map[id])[2].ToString();
            
        }

        switch (id)
        {
            case 0:
                return "Pistol " + ammo;
            case 1:
                return "ShotGun " + ammo;
            case 2:
                return "Sniper " + ammo;
            case 3:
                return "AK-47 " + ammo;
            case 4:
                return "SMG " + ammo;
            case 5:
                return "Grenade " + ((ArrayList)map[id])[1].ToString();
        }
        return "ERROR INVALID GUN ID";
    }
    // Use this for initialization
    void Start () {
        
        map = new Hashtable
        {
            { 0, new ArrayList { 0, pistolClip, pistolLimit } },
            { 5, new ArrayList { 5, grenadeLimit } }
        };

        pos_offsets = new Hashtable
        {
            {0, pistol_offset },
            {1, shotgun_offset },
            {2, sniper_offset },
            {3, ak_offset },
            {4, smg_offset },
            {5, grenade_offset }

        };

        rot_offsets = new Hashtable
        {
            {0, pistol_rot_offset },
            {1, shotgun_rot_offset },
            {2, sniper_offset },
            {3, ak_rot_offset },
            {4, smg_rot_offset },
            {5, grenade_rot_offset }

        };
        slots[0] = format(0);
        slots[1] = "Empty";
        slots[2] = "Empty";
        slots[3] = "Empty";
        slots[4] = "Empty";
        slots[5] = format(5);
        free_slot = 1;
        

    }
    
	
	// Update is called once per frame
	void Update () {
        Text [] canvas = transform.GetChild(0).gameObject.transform.GetComponentsInChildren<Text>();
        for (int i = 0; i < 6; ++i) {
            //Update text based on slots array
            canvas[i].text = slots[i];

        }
        if (Player == null)
        {   //If Player was not created yet try to find it again
            Player = GameObject.Find("MattPlayer(Clone)");
        }
       
	}
    //Used for Weapon Pickups
    //If the gun is already in the inventory refill ammo
    //Else Add the gun to the inventory
    public void Add(int id) {
        
        switch (id)
        {
            case 0:
                ((ArrayList)map[0])[2] = pistolLimit;
                break;
            case 1:
                if (map.ContainsKey(1))
                {
                    ((ArrayList)map[1])[2] = shotgunLimit;

                }
                else
                {
                    
                    map.Add(id, new ArrayList {free_slot,shotgunClip,shotgunLimit });
                    slots[free_slot] = format(id);
                    free_slot += 1;
                }
                break;
            case 2:
                if (map.ContainsKey(2))
                {
                    ((ArrayList)map[2])[2] = sniperLimit;

                }
                else
                {
                    
                    map.Add(id, new ArrayList { free_slot, sniperClip, sniperLimit });
                    slots[free_slot] = format(id);
                    free_slot += 1;
                }
                break;

            case 3:
                if (map.ContainsKey(3))
                {
                    ((ArrayList)map[3])[2] = arLimit;

                }
                else
                {
                    
                    map.Add(id, new ArrayList { free_slot, arClip, arLimit });
                    slots[free_slot] = format(id);
                    free_slot += 1;
                }
                break;
            case 4:
                if (map.ContainsKey(4))
                {
                    ((ArrayList)map[4])[2] = smgLimit;

                }
                else
                {
                    
                    map.Add(id, new ArrayList { free_slot, smgClip, smgLimit });
                    slots[free_slot] = format(id);
                    free_slot += 1;
                }
                break;
            case 5:
                
                ((ArrayList)map[5])[1] = (int)((ArrayList)map[5])[1]+1;
                slots[5] = format(id);

                break;
            default:
                print("ERROR INVALID GUN ID" + id.ToString());
                break;

        }

    }

    //subtracts one from the ammoClip or reloads if comepletly out of ammo does nothing
    //returns false if it could not fire.
    //Else True
    public bool Fire()
    {
        int id = current;
        int ammoInClip = 0;
        int ammoInInv = 0;
        if (map.ContainsKey(id))
        {
            ammoInClip = (int)((ArrayList)map[id])[1];
            if (id == 5)
            {
                
                
                if(ammoInClip > 0)
                {
                    ((ArrayList)map[5])[1] = (int)((ArrayList)map[5])[1] - 1;
                    slots[(int)((ArrayList)map[id])[0]] = format(id);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (ammoInClip > 0)
                {
                    ((ArrayList)map[id])[1] = (int)((ArrayList)map[id])[1] - 1;
                    slots[(int)((ArrayList)map[id])[0]] = format(id);
                    return true;
                }
                else
                {
                    ammoInInv = (int)((ArrayList)map[id])[2];
                    switch (id)
                    {
                        case 0:
                            ((ArrayList)map[id])[1] = Math.Min(pistolClip, ammoInInv) + (int)((ArrayList)map[id])[1];

                            break;
                        case 1:
                            ((ArrayList)map[id])[1] = Math.Min(shotgunClip, ammoInInv) + (int)((ArrayList)map[id])[1];

                            break;
                        case 2:
                            ((ArrayList)map[id])[1] = Math.Min(sniperClip, ammoInInv) + (int)((ArrayList)map[id])[1];

                            break;

                        case 3:
                            ((ArrayList)map[id])[1] = Math.Min(arClip, ammoInInv) + (int)((ArrayList)map[id])[1];

                            break;
                        case 4:
                            ((ArrayList)map[id])[1] = Math.Min(smgClip, ammoInInv) + (int)((ArrayList)map[id])[1];

                            break;
                    }
                    if ((int)((ArrayList)map[id])[1] > 0) {
                        ((ArrayList)map[id])[2] = (int)((ArrayList)map[id])[2]-(int)((ArrayList)map[id])[1];
                        slots[(int)((ArrayList)map[id])[0]] = format(id);
                        return true;
                    }
                    else {
                        return false;
                    }
                }
            }
            
        }
        else { return false; }
    }
    //Switchs current weapon returns new id
    public int switchWeapon()
    {
        int next = current + 1;
        while (!map.ContainsKey(next))
        {
            if(next == 6)
            {
                next = 0;
            }
            else
            {
                next += 1;
            }
        }

        current = next;
        //Needs to be orginally set to null
        GameObject current_gun_prefab = null;
        Vector3 create_pos = Player.transform.GetChild(0).gameObject.transform.position;
        Quaternion create_rot = Player.transform.GetChild(0).gameObject.transform.rotation;
        GameObject Gun;
        print(current);
        switch (current)
        { 
            case 0:
                current_gun_prefab = pistol_prefab;
                break;
            case 1:
                current_gun_prefab = shotgun_prefab;
                break;
            case 2:
                current_gun_prefab = sniper_prefab;
                break;
            case 3:
                current_gun_prefab = ak_prefab;
                break;
            case 4:
                current_gun_prefab = smg_prefab;
                break;
            case 5:
                current_gun_prefab = grenade_prefab;
                break;
         
        }
        //Create the gun object and disable it's trigger since this gun has already been picked up
        Gun = Instantiate(current_gun_prefab, new Vector3(0,0,0), Quaternion.Euler(0,0,0))as GameObject;
        Gun.transform.position = Player.transform.position - (Vector3)pos_offsets[current];
        Gun.transform.rotation = (Quaternion)rot_offsets[current];
        Destroy(Gun.GetComponent<BoxCollider>());
        //Destroy the old gun
        Destroy(Player.transform.GetChild(0).gameObject);
        //Make the new gun the first child of the player
        Gun.transform.parent = Player.transform;

        Gun.transform.SetSiblingIndex(0);
        
        return current;
    }
}
