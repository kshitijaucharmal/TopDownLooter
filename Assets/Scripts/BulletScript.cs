using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {
    public BulletStats stats;

    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        
    }

    void OnCollisionEnter(Collision other){
        if(other.transform.CompareTag("Enemy")){
            other.transform.GetComponent<EnemyScript>().Damage(stats.damage);
        }
        Destroy(gameObject);
    }
}

[System.Serializable]
public class BulletStats{
    public int damage;
}
