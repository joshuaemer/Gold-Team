using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;



//TODO Fix gun prefabs by removing parent object.

public class InventorySystem : NetworkBehaviour
{
    //keeps track of what guns are in each slot based on id
    //Pistol: 0
    //Shotgun: 1
    //Sniper: 2
    //AK: 3
    //SMG: 4
    //Grenades: 5

    private string[] slots = { "", "", "", "", "", "" };
    //Maps gun id to index, current clip and current ammo amount
    private int free_slot;
    private Hashtable map;
    private GameObject Player;
    private int current = 0;

    //Ammo limits
    private int pistolLimit = 120;
    private int shotgunLimit = 40;
    private int sniperLimit = 30;
    private int arLimit = 120;
    private int smgLimit = 135;
    private int grenadeLimit = 6;

    //clip sizes
    private int pistolClip = 12;
    private int shotgunClip = 8;
    private int sniperClip = 6;
    private int arClip = 30;
    private int smgClip = 35;

    //Grenade vars
    //Handles grenade fire rate
    private bool nextGrenade = true;
   

    //Objects for each gun prefab
    public GameObject pistol_prefab;
    public GameObject shotgun_prefab;
    public GameObject sniper_prefab;
    public GameObject ak_prefab;
    public GameObject smg_prefab;
    public GameObject grenade_prefab;
    //Determines if the initial gun has been set
    private bool initialSet = false;




    //Formats string to display on screen based on gun id.
    string format(int id)
    {
        string result = "";
        string ammo = "";
        if (id != 5)
        {
            ammo = ((ArrayList)map[id])[1].ToString() + "/" + ((ArrayList)map[id])[2].ToString();

        }

        switch (id)
        {
            case 0:
                result += "Pistol " + ammo;
                break;
            case 1:
                result += "ShotGun " + ammo;
                break;
            case 2:
                result += "Sniper " + ammo;
                break;
            case 3:
                result += "AK-47 " + ammo;
                break;
            case 4:
                result += "SMG " + ammo;
                break;
            case 5:
                result += "Grenade " + ((ArrayList)map[id])[1].ToString();
                break;
        }
        if (id == current)
        {
            result = "*" + result;
        }

        return result;
    }
    // Use this for initialization
    void Start()
    {

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
    void Update()
    {
        if (!transform.parent.GetComponent<FPController>().HasAuthority()) { return; }
        Text[] canvas = transform.GetChild(0).gameObject.transform.GetComponentsInChildren<Text>();
        for (int i = 0; i < 6; ++i)
        {
            //Update text based on slots array
            canvas[i].text = slots[i];

        }
        if (Player == null)
        {   //If Player was not created yet try to find it again
            Player = transform.parent.gameObject;
        }
        else if (!initialSet)
        {   //Give the player their starting pistol
            initialSet = true;
            setWeapon(0);
        }

    }
    //Used for Weapon Pickups
    //If the gun is already in the inventory refill ammo
    //Else Add the gun to the inventory
    public void Add(int id)
    {

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

                    map.Add(id, new ArrayList { free_slot, shotgunClip, shotgunLimit });
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

                ((ArrayList)map[5])[1] = (int)((ArrayList)map[5])[1] + 1;
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
                if (!nextGrenade) { return false; }

                if (ammoInClip > 0)
                {
                    ((ArrayList)map[5])[1] = (int)((ArrayList)map[5])[1] - 1;
                    slots[(int)((ArrayList)map[id])[0]] = format(id);




                    //Below will throw the grenade
                    //The guncomponent script belongs to the weapons child so we need to call it from there.
                    nextGrenade = false;
                    GameObject grenade = Player.transform.GetChild(0).gameObject;

                    grenade.gameObject.transform.GetChild(0).gameObject.GetComponent<GunComponent>().Cmd_Throw_Grenade(Player);
                    



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

                    if ((int)((ArrayList)map[id])[1] > 0)
                    {
                        ((ArrayList)map[id])[2] = (int)((ArrayList)map[id])[2] - (int)((ArrayList)map[id])[1];
                        slots[(int)((ArrayList)map[id])[0]] = format(id);
                       

                        return true;
                    }
                    else
                    {
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
        int last = current;
        int next = current + 1;
        while (!map.ContainsKey(next))
        {
            if (next == 6)
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

        setWeapon(current);
        slots[(int)((ArrayList)map[last])[0]] = format(last);
        slots[(int)((ArrayList)map[current])[0]] = format(current);

        return current;
    }


    void setWeapon(int id)
    {

        GameObject current_gun_prefab = null;
        Vector3 create_pos = Player.transform.GetChild(0).gameObject.transform.localPosition;
        Quaternion create_rot = Player.transform.GetChild(0).gameObject.transform.localRotation;
        GameObject Gun;

        switch (id)
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

        Gun = Instantiate(current_gun_prefab, new Vector3(0, 0, 0), create_rot) as GameObject;
        //Remove guns trigger

        Destroy(Gun.transform.GetChild(0).GetComponent<BoxCollider>());

        //Destroy the old gun
        Destroy(Player.transform.GetChild(0).gameObject);
        //Make the new gun the first child of the player
        Gun.transform.parent = Player.transform;

        Gun.transform.SetSiblingIndex(0);
        //Set local position to that of the old gun
        Gun.transform.localPosition = create_pos;
        Gun.transform.localRotation = create_rot;
        
        Gun.transform.localRotation = Quaternion.Euler(0, 90, 0);
        if (!HasAmmoLeft(5) && id == 5)
        {
            Gun.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    public void setNextGrenade(bool next)
    {
        nextGrenade = next;
    }
    public GameObject getGrenadePrefab()
    {
        return grenade_prefab;
    }

    public bool HasAmmoLeft(int id)
    {
        return (int)((ArrayList)map[id])[1] > 0;
    }
}