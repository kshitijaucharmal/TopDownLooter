using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : MonoBehaviour {

    public enum ItemType{
        HEAL,
        INSTANTDEATH
    }

    [SerializeField] private ItemType itemType;

    void OnCollisionEnter(Collision other){
        if(other.transform.CompareTag("Player")){
            switch(itemType){
                case ItemType.HEAL:
                    other.transform.GetComponent<PlayerStats>().health += 100;
                    break;
                case ItemType.INSTANTDEATH:
                    other.transform.GetComponent<PlayerStats>().Die();
                    break;
            }
            Destroy(gameObject);
        }
    }

}
