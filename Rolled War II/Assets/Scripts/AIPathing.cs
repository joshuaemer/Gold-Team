using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIPathing : MonoBehaviour {

    NavMeshAgent agent;

	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
	}

    void Update() {
        GameObject go = GameObject.FindGameObjectWithTag("Player");
        if(go != null) {
            MoveToPlayer(go);
        }
    }

	public void MoveToPlayer(GameObject player) {
        agent.SetDestination(player.transform.position);
    }
}
