using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateOnCollision : MonoBehaviour {

    [SerializeField] private EnemyScript script;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private bool enableComponentOnHit = true;
    [SerializeField] private bool disableThisOnHit = true;
    [SerializeField] private string hitToDisableTag;

    void Start(){
        script.enabled = !enableComponentOnHit;
        rb.useGravity = true;
        rb.isKinematic = false;
    }

    void OnCollisionEnter(Collision other){
        if(other.transform.CompareTag(hitToDisableTag)){
            script.enabled = enableComponentOnHit;
            rb.useGravity = false;
            rb.isKinematic = true;
            this.enabled = disableThisOnHit;
        }
    }

}
