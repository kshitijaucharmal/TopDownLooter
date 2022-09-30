using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerShooting : MonoBehaviour {
    
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject portalBulletPrefab;
    [SerializeField] private Transform portalShootPoint;
    [SerializeField] private float bulletSpeed = 50f;
    [SerializeField] private float portalBulletSpeed = 10f;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private MeshRenderer canon;

    // Overheat
    [SerializeField] private Slider overheatSlider;
    [SerializeField] private TMP_Text overheatText;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fill;
    [SerializeField] private float coolDownTime = 2f;
    [SerializeField] private int overheatTill = 500;

    [SerializeField] private EnemyType selectedPortal;
    [SerializeField] private Image portalIndicator;
    [SerializeField] private Animator animator;

    [SerializeField] private Color purpleColor;

    private int portalCtr{
        get{
            return _portalCtr;
        }
        set{
            if(value == _portalCtr) return;
            SetBullet(value);

            _portalCtr = value;
        }
    }
    private int _portalCtr;

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
                overheatText.enabled = true;
                animator.SetBool("overheating", true);
                return;
            }
            if(value <= 0){
                isOverheated = false;
                overheatText.enabled = false;
                animator.SetBool("overheating", false);
                return;
            }
            _overheatCtr = value;
        }
    }
    private float _overheatCtr;
    private PlayerStats stats;
    
    void SetBullet(int ctr){
        // Set portal Bullet
        switch(_portalCtr){
            case 0:
                selectedPortal = EnemyType.SLIG;
                portalIndicator.color = Color.green;
                canon.material.color = Color.green;
                break;
            case 1:
                selectedPortal = EnemyType.AVMED;
                portalIndicator.color = purpleColor;
                canon.material.color = purpleColor;
                break;
            case 2:
                selectedPortal = EnemyType.SMAF;
                portalIndicator.color = Color.yellow;
                canon.material.color = Color.yellow;
                break;
            default:
                Debug.Log("Somethings Wrong");
                break;
        }
    }

    void Start(){
        stats = GetComponent<PlayerStats>();

        coolDownCtr = coolDownTime;
        overheatSlider.maxValue = overheatTill;
        overheatSlider.value = 0;
        fill.color = gradient.Evaluate(0f);
        portalCtr = 0;
        overheatText.enabled = false;

        portalIndicator.color = Color.green;
        canon.material.color = Color.green;
    }

    // Update is called once per frame
    void Update() {
        // Portals
        if(Input.GetMouseButtonDown(0)){
            ShootPortals();
        }

        // Scroll to change portal
        float scroll = Input.GetAxisRaw("Mouse ScrollWheel");

        if (scroll > 0) {
            if(portalCtr >= 2) portalCtr = 0;
            else portalCtr++;
        }
        if (scroll < 0) {
            if(portalCtr <= 0) portalCtr = 2;
            else portalCtr--;
        }

        // Buttons to change portal
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            portalCtr = 0;
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)){
            portalCtr = 1;
        }
        if(Input.GetKeyDown(KeyCode.Alpha3)){
            portalCtr = 2;
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
        fill.color = gradient.Evaluate(overheatSlider.normalizedValue);
    }

    // Shoot Portals
    void ShootPortals(){
        
        switch(selectedPortal){
            case EnemyType.SLIG:
                if(stats.money >= 50){
                    stats.money -= 50;
                }
                else return;
                break;
            case EnemyType.AVMED:
                if(stats.money >= 20){
                    stats.money -= 20;
                }
                else return;
                break;
            case EnemyType.SMAF:
                if(stats.money >= 10){
                    stats.money -= 10;
                }
                else return;
                break;
        }

        GameObject portalBullet = Instantiate(portalBulletPrefab, portalShootPoint.position, Quaternion.identity);
        portalBullet.GetComponent<PortalBulletScript>().Setup(selectedPortal);
        portalBullet.GetComponent<Rigidbody>().AddForce(Vector3.up * portalBulletSpeed * Time.deltaTime, ForceMode.Impulse);
    }

    void ShootBullets(){
        Vector3 shootDir = transform.forward;

        Rigidbody bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
        bullet.AddForce(shootDir * bulletSpeed);
        if(bullet.gameObject != null)Destroy(bullet.gameObject, 3f);
    }
}
