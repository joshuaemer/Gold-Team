using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponMechanics : NetworkBehaviour
{
    //keeps track of what guns are in each slot based on ID
    //Pistol: 0
    //Shotgun: 1
    //Sniper: 2
    //AK47: 3
    //M4: 4
    //Grenades: 5

    private bool weaponCooled = true; //Bool variable to determine whether or not the weapon has cooled down directly after firing
    /*Fire rates (in seconds) of each of the 6 weapons.*/
    private float pistolFireRate = 0.2f;
    private float shotgun_fireRate = 1.5f;
    private float sniper_fireRate = 3f;
    private float AK_fireRate = 0.1f;
    private float M4_fireRate = 0.08f;
    private float grenade_fireRate = 2.5f;
    public Camera playerCam;
    public GameObject inv;
    private float nextFire;
    private int curr = 0; //'curr' represents the currently selected weapon in the Inventory
    public AudioClip pistol_shot;
    public AudioClip shotgun_shot;
    public AudioClip sniper_shot;
    public AudioClip smg_shot;
    public AudioClip ak_shot;
    public AudioClip reload;
    public AudioSource source;

    // Use this for initialization
    void Start()
    {

        
        
        source = GetComponent<AudioSource>();

    }


    // Update is called once per frame
    /*Checking Input.getMouseButtonDown(0) checks if the left mouse button is clicked down at the current frame in Update (semi-automatic fire)
      Input.getMouseButton(0) checks if the left mouse button is being held at the current frame (automatic fire).
    */
    void Update()
    {
        if (!hasAuthority) { return; }
        // If the "e" key is pressed, switch weapons by calling switchWeapon() function in Inventory System.
        if (Input.GetKeyDown("e"))
        {
            //Switch Weapons
            curr = inv.GetComponent<InventorySystem>().switchWeapon();
        }

        if (curr == 0 && weaponCooled)
        { //Checks if the current weapon is the pistol and if the weapon is cooled
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(waitForNSeconds(pistolFireRate));
            }
        }
        //Checks if the current weapon is the shotgun and if the weapon is cooled
        else if (curr == 1 && weaponCooled)
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(waitForNSeconds(shotgun_fireRate));
            }
        }
        //Checks if the current weapon is the sniper rifle and if the weapon is cooled
        else if (curr == 2 && weaponCooled)
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(waitForNSeconds(sniper_fireRate));
            }
        }
        //Checks if the current weapon is the AK-47 and if the weapon is cooled
        else if (curr == 3 && weaponCooled)
        {
            if (Input.GetMouseButton(0))
            {
                StartCoroutine(waitForNSeconds(AK_fireRate));
            }
        }
        //Checks if the current weapon is the M4 (called SMG in game) and if the weapon is cooled
        else if (curr == 4 && weaponCooled)
        {
            if (Input.GetMouseButton(0))
            {
                StartCoroutine(waitForNSeconds(M4_fireRate));
            }
        }
        //Checks if the current weapon is the grenade and if the weapon is cooled
        else if (curr == 5)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //StartCoroutine(waitForNSeconds(grenade_fireRate));
                shoot();
            }
        }
        else
        {
            //Debug.LogError("ERR: Invalid curr value: " + curr + ".  Quitting....");
        }
    }
    /*waitForNSeconds(float N): Function that takes in a float argument called N, calls the shoot() function,
    sets the boolean variable weaponCooled to false, and delays the function for N seconds before turning
    weaponCooled back to true.  This function returns an IEnumerator that is used as an arguemnt in the
    StartCoroutine function above.
    
     */
    IEnumerator waitForNSeconds(float N)
    {
        shoot();
        weaponCooled = false;
        yield return new WaitForSeconds(N);
        weaponCooled = true;
    }


    void shoot()
    {
        if (inv.GetComponent<InventorySystem>().Fire())
        {
            switch (curr) {
                case 0:
                    source.PlayOneShot(pistol_shot);
                    break;
                case 1:
                    source.PlayOneShot(shotgun_shot);
                    break;
                case 2:
                    source.PlayOneShot(sniper_shot);
                    break;
                case 3:
                    source.PlayOneShot(ak_shot);
                    break;
                case 4:
                    source.PlayOneShot(smg_shot);
                    break;
            }
            CmdShoot(transform.position, playerCam.transform.forward);
        }
    }

    [Command]
    void CmdShoot(Vector3 pos, Vector3 rot)
    {
        float pistol_damage = 125f;
        float shotgun_damage = 350f;
        float sniper_damage = 525f;
        float ak47_damage = 90f;
        float m4_damage = 75f;
        float damage = 0;

        switch (curr) {
            case 0:
                damage = pistol_damage;
                break;
            case 1:
                damage = shotgun_damage;
                break;
            case 2:
                damage = sniper_damage;
                break;
            case 3:
                damage = ak47_damage;
                break;
            case 4:
                damage = m4_damage;
                break;
        }
        RaycastHit hit;
        if (Physics.Raycast(pos, rot, out hit))
        {
            /*if (hit.transform.CompareTag("Player"))
            {
                hit.transform.gameObject.GetComponent<FPController>().TakeDamage((int)damage);
            }*/
            if (hit.transform.CompareTag("Monster"))
            {
                hit.transform.gameObject.GetComponent<SkeletonMovement>().TakeDamage((int)damage);
            }
        }
    }
}
