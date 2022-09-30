using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : MonoBehaviour {

    public enum ItemType{
        HEAL,
        INSTANTDEATH,
        MONEY
    }

    [SerializeField] private ItemType itemType;
    [SerializeField] private float force = 30;

    void Start(){
        Vector3 dir = Vector3.up + new Vector3(Random.Range(-0.3f, 0.3f), 0f, Random.Range(-0.3f, 0.3f));
        GetComponent<Rigidbody>().AddForce(dir * force, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision other){
        if(other.transform.CompareTag("Player")){
            switch(itemType){
                case ItemType.HEAL:
                    other.transform.GetComponent<PlayerStats>().health += 25;
                    break;
                case ItemType.INSTANTDEATH:
                    other.transform.GetComponent<PlayerStats>().Die();
                    break;
                case ItemType.MONEY:
                    other.transform.GetComponent<PlayerStats>().AddMoney(Random.Range(2, 10));
                    break;
            }
            Destroy(gameObject);
        }
    }

}
