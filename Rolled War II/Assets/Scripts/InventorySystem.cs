using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
                return "Shot Gun" + ammo;
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
            
            canvas[i].text = slots[i];

        }
	}
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
                ((ArrayList)map[5])[2] = (int)((ArrayList)map[5])[2]+1;
                break;
            default:
                print("ERROR INVALID GUN ID" + id.ToString());
                break;

        }

    }
  
    
}
