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
                Die();
            }
            // Not required cause enemy cannot gain health
            // if(value >= stats.maxHealth) return;
            _health = value;
        }
    }
    private int _health = 500;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fill;
    [SerializeField] private EnemyStats stats;

    private Transform playerTarget;
    private float speed;

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
        // Particle System
        Destroy(gameObject);
    }

    public void GetSucked(Vector3 pos){
        // Get sucked at this position
        transform.localScale *= 0.2f;
        Destroy(gameObject, 2f);
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
    public string name = "SlowAsShit";
    public int maxHealth = 500;
    public float speed = 4f;
    public float rotationSpeed = 50f;
    public float MaxDist = 10f;
    public float MinDist = 4f;
    public int damage = 20;
}
