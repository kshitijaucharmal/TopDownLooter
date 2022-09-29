using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalScript : MonoBehaviour {

    [HideInInspector] public EnemyType enemyType;
    [SerializeField] private Gradient[] gradients;
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private SpriteRenderer portalImage;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float portalDieTime = 4f;
    [SerializeField] private float effectRadius = 5f;
    [SerializeField] private float rotateSpeed = 10f;

    private ParticleSystem.ColorOverLifetimeModule colorOverLife;
    private Gradient currentGradient;

    public void Setup(EnemyType type) {
        this.enemyType = type;

        switch(type){
            case EnemyType.SLIG:
                currentGradient = gradients[0];
                break;
            case EnemyType.AVMED:
                currentGradient = gradients[1];
                break;
            case EnemyType.SMAF:
                currentGradient = gradients[2];
                break;
            default:
                Debug.Log("Somethings Wrong");
                break;
        }

        colorOverLife = particleSystem.colorOverLifetime;
        colorOverLife.color = currentGradient;
        portalImage.color = currentGradient.Evaluate(0f);
    }

    void Start(){
        // Destroy in specified time
        Destroy(gameObject, portalDieTime);
    }

    void Update(){
        transform.Rotate(0, 0, rotateSpeed);

        var enemies = Physics.OverlapSphere(transform.position, effectRadius, enemyLayer);
        foreach(Collider col in enemies){
            EnemyScript enemy = col.GetComponent<EnemyScript>();
            if(enemy.stats.enemyType == enemyType)
                enemy.GetSucked(transform.position);
        }
    }

    void OnDrawGizmosSelected(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, effectRadius);
    }
}
