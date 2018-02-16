using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FPController : NetworkBehaviour {

    public float speed = 3.0f;
    public float rotateSpeed = 3.0f;
    public float smoother = 10;
    private CharacterController controller;

    // this is used to sync position and rotation of other players
    private Vector3 enemyPosition;
    private Quaternion enemyRotation;

    void Start() {
        controller = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    // Every client calls this on every player even if it isn't their's
    void Update () {
        if (!hasAuthority) {
            // Not the local client's player

            // sync the player's approximate position and rotation with some smoothing
            transform.position = Vector3.Lerp(transform.position, enemyPosition, Time.deltaTime * smoother);
            transform.rotation = Quaternion.Lerp(transform.rotation, enemyRotation, Time.deltaTime * smoother);
            return;
        }

        Camera.main.GetComponent<FollowPlayer>().setPlayer(transform);

        // We know this is OUR character to control so let's control it
        transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed, 0);
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        float curSpeed = speed * Input.GetAxis("Vertical");
        controller.SimpleMove(forward * curSpeed);
        CmdUpdatePlayer(transform.position, transform.rotation);
	}

    [Command]
    void CmdUpdatePlayer(Vector3 pos, Quaternion rot) {
        transform.position = pos;
        transform.rotation = rot;

        RpcUpdatePlayer(pos, rot);
    }

    [ClientRpc]
    void RpcUpdatePlayer(Vector3 pos, Quaternion rot) {
        enemyPosition = pos;
        enemyRotation = rot;
    }
}
