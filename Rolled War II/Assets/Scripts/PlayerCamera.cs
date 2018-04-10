using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerCamera : NetworkBehaviour {
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private AudioListener listener;

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
            listener.enabled = false;
            return;
        }
        cam.enabled = true;
        listener.enabled = true;
    }
}
