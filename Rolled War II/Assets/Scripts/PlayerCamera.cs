using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Camera))]
public class PlayerCamera : NetworkBehaviour {
    [SerializeField]
    private Camera cam;

	void Start () {
        CmdActivateCameras();
	}

    [Command]
    void CmdActivateCameras() {
        RpcActivateCameras();
    }

    [ClientRpc]
    void RpcActivateCameras() {
        try {
            Camera.main.gameObject.SetActive(false);
        }
        catch {

        }
        if (!hasAuthority) {
            cam.enabled = false;
            return;
        }
        cam.enabled = true;
    }
}
