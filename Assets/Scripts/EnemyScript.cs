using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour {

    public int health{
        get{
            return _health;
        }
        set{
            if(value <= 0){
                _health = 0;
                Die();
                return;
            }
            // Not required cause enemy cannot gain health
            // if(value >= stats.maxHealth) return;
            healthSlider.value = value;
            fill.color = gradient.Evaluate(healthSlider.normalizedValue);
            _health = value;
        }
    }
    private int _health = 500;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fill;

    [SerializeField] private GameObject[] itemsToDrop;

    public EnemyStats stats;

    private Transform playerTarget;
    private float speed;
    private bool dead = false;

    void Start(){
        speed = stats.speed;

        _health = stats.maxHealth;
        healthSlider.maxValue = health;
        fill.color = gradient.Evaluate(1f);
        healthSlider.value = health;

        playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FixedUpdate(){
        if(playerTarget == null) return;
        if(Vector3.Distance(transform.position, playerTarget.position) >= stats.MinDist){
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerTarget.position - transform.position), stats.rotationSpeed * Time.deltaTime);
            transform.position += transform.forward * speed * Time.deltaTime;
        }
    }

    void Die(){
        if(dead) return;
        
        int n_items = 0;
        if(Random.Range(0f, 1f) < 0.01f) n_items = 2;
        if(Random.Range(0f, 1f) < 0.3) n_items = 1;

        for(int i = 0; i < n_items; i++){
            GameObject item = itemsToDrop[Random.Range(0, itemsToDrop.Length)];
            Vector3 pos = transform.position + new Vector3(Random.Range(-0.5f, 0.5f), 0f, Random.Range(-0.5f, 0.5f));
            Instantiate(item, pos, Quaternion.identity);
        }
        dead = true;
        Destroy(gameObject);
    }

    public void GetSucked(Vector3 pos){
        // Get sucked at this position
        Destroy(gameObject);
    }

    public void Damage(int val){
        health -= val;
        healthSlider.value = health;
        fill.color = gradient.Evaluate(healthSlider.normalizedValue);
    }

    void OnCollisionEnter(Collision other){
        Transform hitObject = other.transform;
        if(hitObject.CompareTag("Player")){
            hitObject.GetComponent<PlayerStats>().Damage(stats.damage);
        }
    }
}

[System.Serializable]
public class EnemyStats{
    public EnemyType enemyType;
    public int maxHealth = 500;
    public float speed = 4f;
    public float rotationSpeed = 50f;
    public float MaxDist = 10f;
    public float MinDist = 4f;
    public int damage = 20;
}
