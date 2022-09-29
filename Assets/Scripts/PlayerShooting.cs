using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour {
    
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject portalBulletPrefab;
    [SerializeField] private Transform portalShootPoint;
    [SerializeField] private float bulletSpeed = 50f;
    [SerializeField] private float portalBulletSpeed = 10f;
    [SerializeField] private Transform shootPoint;

    // Overheat
    [SerializeField] private Slider overheatSlider;
    [SerializeField] private float coolDownTime = 2f;
    [SerializeField] private int overheatTill = 500;

    private bool isShooting = false;
    private bool isOverheated = true;
    private float coolDownCtr;
    private float overheatCtr{
        get{
            return _overheatCtr;
        }
        set{
            if(value >= overheatTill){
                isOverheated = true;
                return;
            }
            if(value <= 0){
                isOverheated = false;
                return;
            }
            _overheatCtr = value;
        }
    }
    private float _overheatCtr;

    void Start(){
        coolDownCtr = coolDownTime;
        overheatSlider.maxValue = overheatTill;
        overheatSlider.value = 0;
    }

    // Update is called once per frame
    void Update() {

        // Portals
        if(Input.GetMouseButtonDown(0)){
            ShootPortals();
        }

        // Bullets Shooting
        if(Input.GetMouseButtonDown(1) && !isOverheated){
            isShooting = true;
        }
        if(Input.GetMouseButtonUp(1)){
            isShooting = false;
        }

        if(isOverheated){
            isShooting = false;
            coolDownCtr -= Time.deltaTime;
            if(coolDownCtr <= 0){
                isOverheated = false;
                overheatCtr = 0;
                coolDownCtr = coolDownTime;
            }
        }

        if(isShooting){
            overheatCtr++;
            ShootBullets();
        }
        else{
            overheatCtr--;
        }

        overheatSlider.value = overheatCtr;
    }

    // Shoot Portals
    void ShootPortals(){
        Rigidbody portalBullet = Instantiate(portalBulletPrefab, portalShootPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
        portalBullet.AddForce((transform.forward + Vector3.up).normalized * portalBulletSpeed * Time.deltaTime, ForceMode.Impulse);
    }

    void ShootBullets(){
        Vector3 shootDir = transform.forward;

        Rigidbody bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
        bullet.AddForce(shootDir * bulletSpeed);
        if(bullet.gameObject != null)Destroy(bullet.gameObject, 3f);
    }
}
