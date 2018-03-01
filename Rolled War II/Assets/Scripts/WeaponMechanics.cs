using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponMechanics : NetworkBehaviour {
	private GameObject inv;
	
	

	// Use this for initialization
	void Start () {
		
        //Get the Player's Inventory
		inv = GameObject.Find("Inventory");


	}
	
	// Update is called once per frame
	void LateUpdate () {
        if (!hasAuthority) { return; }
		// If the left mouse button is clicked, fire.
		if (Input.GetMouseButton(0)) {
			// TODO
			shoot();

		}
		else if (Input.GetKey("e")) {
            //Switch Weapons
			inv.GetComponent<InventorySystem>().switchWeapon();
		}
	}

	


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
	

}
