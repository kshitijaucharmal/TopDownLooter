using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalBulletScript : MonoBehaviour {

    [SerializeField] private enum BulletType{
        GREEN, YELLOW, PURPLE
    }

    [SerializeField] private GameObject portalPrefab;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    void OnCollisionEnter(Collision other){
        if(other.gameObject.layer == LayerMask.NameToLayer("Ground")){
            Vector3 pos = transform.position;
            pos.y = 0;
             Instantiate(portalPrefab, pos, Quaternion.Euler(90, 0, 0));
            Destroy(gameObject);
        }
    }
}
