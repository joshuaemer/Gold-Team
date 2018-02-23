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
    //AR: 3
    //SMG: 4
    //Grenades: 5
   
    private string[] slots = { "", "", "", "", "",""};
    //Maps gun id to index, current clip and current ammo amount
    private int free_slot;
    private Hashtable map;

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
                return "AR-15 " + ammo;
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
        return current;
    }
}
