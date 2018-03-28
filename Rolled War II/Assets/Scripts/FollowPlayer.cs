using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// derived this solution from: https://answers.unity.com/questions/1157437/making-my-camera-follow-player-in-multiplayer.html

public class FollowPlayer : MonoBehaviour {

    private Transform playerTransform;

    void Update() {
        if(playerTransform != null) {
            transform.position = playerTransform.transform.position;
            transform.rotation = playerTransform.rotation;
        }
    }
	
	public void setPlayer(Transform transform) {
        playerTransform = transform;
    }
}
