using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponMechanics : NetworkBehaviour {
	private GameObject inv;
	private int curr= 0;
	

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
            if (Input.GetMouseButtonDown(0))
            {
                // TODO
                if (curr == 0)
                {
                    waitForNSeconds(0.4f);
                    shoot();
                }
                else if (curr == 1)
                {
                    waitForNSeconds(2);
                    shoot();
                }
                else if (curr == 2)
                {
                    //yield return new WaitForSeconds(4);
                    waitForNSeconds(4);
                    shoot();
                }
                else if (curr == 3)
                {
                    waitForNSeconds(0.05f);
                    shoot();
                }
                else if (curr == 4)
                {
                    waitForNSeconds(0.05f);
                    shoot();
                }
                else if (curr == 5)
                {
                    //yield return new WaitForSeconds(3);
                    waitForNSeconds(3);
                }
                else
                {
                    Debug.LogError("ERR: Invalid curr value: " + curr + ".  Quitting....");
                    Application.Quit();
                }
            }
		}
		else if (Input.GetKeyDown("e")) {
            //Switch Weapons
			curr = inv.GetComponent<InventorySystem>().switchWeapon();
		}
	}
    

	/*
	waitForNSeconds(float n): Function that takes in a float argument called n, and delays the function that calls this one
	by that number n in seconds.  For example, n = 0.25f means delay by a quarter of a second, and n=4
	means delay by 4 seconds.  This essentially puts a lower bound on the time that would be necessary
	wait before a mouse click (fire) would register.
	 */
	IEnumerator waitForNSeconds(float n) {
		yield return new WaitForSeconds(n);
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
            else if (hit.transform.CompareTag("Monster"))
            {
                hit.transform.gameObject.GetComponent<SkeletonMovement>().takeDamage(10);
            }
        }
    }
	

}
