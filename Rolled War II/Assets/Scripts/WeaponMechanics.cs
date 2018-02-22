using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponMechanics : NetworkBehaviour {
	private GameObject inv;
	
	public int coord_x, coord_y, coord_z;
	public Rigidbody bullet;
	public Hashtable map;
	private int weaponID; //Random number to be determned by outside function

	// Use this for initialization
	void Start () {
		//Weapon secondary = new Weapon(0, 30, 15);

		inv = GameObject.Find("Inventory");


	}
	
	// Update is called once per frame
	void Update () {
        if (!hasAuthority) { return; }
		// If the left mouse button is clicked, fire RigidBodies from the weapon
		if (Input.GetMouseButton(0)) {
			// TODO
			shoot();

		}
		else if (Input.GetKey("e")) {
			inv.GetComponent<InventorySystem>().switchWeapon();
		}
	}

	// Function that takes in 
	/*Weapon cycle(int weaponID) {
		return ammoCounts[weaponID];
		//return weaponID == 6 ? 0 : ++weaponID;
	}*/


	void shoot() {
		if (inv.GetComponent<InventorySystem>().Fire()) {
            CmdShoot(transform.position, transform.forward);
		}
	}

    [Command]
    void CmdShoot(Vector3 pos, Vector3 rot) {
		RaycastHit hit;
		if(Physics.Raycast(pos, rot, out hit)) {
			if (hit.transform.CompareTag("Player")) {
				hit.transform.gameObject.GetComponent<FPController>().TakeDamage(100);
			}
        }
    }
	/*public class Weapon{
		int ID;
		int reserves;
		int currentMagAmmo;
		public Weapon(int _ID, int _reserves, int _currentMagAmmo) {
			ID = _ID;
			reserves = _reserves;
			currentMagAmmo = _currentMagAmmo;
		}
	}*/

}
