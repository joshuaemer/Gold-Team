using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ThrowGrenade : NetworkBehaviour {
    private int delay = 3;
    private float lastThrow = 0;
    [SerializeField]
    private float force = 20;

    public GameObject grenade;

	// Update is called once per frame
	void Update () {
        if (!hasAuthority) { return; }

        if(Input.GetKeyDown(KeyCode.G) && Time.time - lastThrow >= delay) {
            CmdThrowGrenade(transform.position, transform.rotation);
        }
	}

    [Command]
    void CmdThrowGrenade(Vector3 position, Quaternion rotation) {
        GameObject go = Instantiate(grenade, position + new Vector3(0, 2, 0), rotation);
        Vector3 dir = transform.forward;
        go.GetComponent<Rigidbody>().AddForce(dir * force, ForceMode.Impulse);
        NetworkServer.Spawn(go);
    }
}
