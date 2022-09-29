using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalScript : MonoBehaviour {

    [SerializeField] private Gradient[] gradients;
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private SpriteRenderer portalImage;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float portalDieTime = 4f;
    [SerializeField] private float effectRadius = 5f;
    [SerializeField] private float rotateSpeed = 10f;

    private ParticleSystem.ColorOverLifetimeModule colorOverLife;
    private Gradient currentGradient;

    // Start is called before the first frame update
    void Start() {
        // Random For Now
        currentGradient = gradients[Random.Range(0, gradients.Length)];
        colorOverLife = particleSystem.colorOverLifetime;
        colorOverLife.color = currentGradient;
        portalImage.color = currentGradient.Evaluate(0f);

        // Destroy in specified time
        Destroy(gameObject, portalDieTime);
    }

    void Update(){
        transform.Rotate(0, 0, rotateSpeed);

        var enemies = Physics.OverlapSphere(transform.position, effectRadius, enemyLayer);
        foreach(Collider col in enemies){
            EnemyScript enemy = col.GetComponent<EnemyScript>();
            enemy.GetSucked(transform.position);
        }
    }

    void OnDrawGizmosSelected(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, effectRadius);
    }
}
