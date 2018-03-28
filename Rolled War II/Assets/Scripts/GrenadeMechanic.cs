using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeMechanic : MonoBehaviour {
    [SerializeField]
    private float timer = 3;

    [SerializeField]
    private int grenade_damage = 0;

    public GameObject prefab;

    private List<GameObject> currentCollisions = new List<GameObject>();

    void Start() {
        Destroy(gameObject, timer);
    } 

    void OnDestroy() {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 10);
        for (int i = 0; i < hitColliders.Length; ++i) {
            GameObject current = hitColliders[i].gameObject;
            if (current.CompareTag("Monster")) {
                current.GetComponent<SkeletonMovement>().TakeDamage(grenade_damage);
            }

            else if (current.CompareTag("Player")) {
                current.GetComponent<FPController>().TakeDamage(grenade_damage);
            }
        }
        GameObject vfx = Instantiate(prefab, transform.position, transform.rotation);
        vfx.GetComponent<ParticleSystem>().Play();
        Destroy(vfx, 2);
    }
}
