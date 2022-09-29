using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalBulletScript : MonoBehaviour {

    [HideInInspector] public EnemyType enemyType = EnemyType.SLIG;
    [SerializeField] private GameObject portalPrefab;

    [SerializeField] private Material[] mats;

    // Start is called before the first frame update
    public void Setup(EnemyType enemyType) {
        this.enemyType = enemyType;
        MeshRenderer sr = GetComponent<MeshRenderer>();
        switch(enemyType){
            case EnemyType.SLIG:
                sr.material = mats[0];
                break;
            case EnemyType.AVMED:
                sr.material = mats[1];
                break;
            case EnemyType.SMAF:
                sr.material = mats[2];
                break;
        }
    }

    void OnCollisionEnter(Collision other){
        if(other.gameObject.layer == LayerMask.NameToLayer("Ground")){
            Vector3 pos = transform.position;
            pos.y = 0;
            PortalScript portal = Instantiate(portalPrefab, pos, Quaternion.Euler(90, 0, 0)).GetComponent<PortalScript>();
            portal.Setup(enemyType);
            Destroy(gameObject);
        }
    }
}
