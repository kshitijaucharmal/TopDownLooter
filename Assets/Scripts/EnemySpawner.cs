using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemySpawner : MonoBehaviour {

    [SerializeField] private GameManager manager;

    // Enemies normally (Should be small, but not zero)
    [SerializeField] private int minEnemies = 2;
    [SerializeField] private int maxEnemies = 5;

    // Enemies in Waves (Should be much more)
    [SerializeField] private int minEnemiesInWave = 6;
    [SerializeField] private int maxEnemiesInWave = 10;
    // enemies spawn normally in random time between
    [SerializeField] private float enemySpawnTime = 2f;
    // Time of the total level
    [SerializeField] private float levelTime = 2 * 60f; 
    [SerializeField] private int nWaves = 3;
    [SerializeField] private float enemySpawnHeight = 10;

    [Header("UI Stuff")]
    [SerializeField] private Slider waveSlider;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fill;
    [SerializeField] private TMP_Text waveInfo;

    [SerializeField] private GameObject[] enemies;
    [SerializeField] private GameObject pointItLands;

    private int enemyCount = 0;
    private float timeBtwnWaves;
    private float timeBtwnSpawns;
    private float timeCtr = 0;

    // Start is called before the first frame update
    void Start() {
        // Constant enemies from the start
        int nEnemies = Random.Range(minEnemies, maxEnemies);
        timeBtwnWaves = levelTime / nWaves;
        timeBtwnSpawns = timeBtwnWaves / nEnemies;

        // Slider Stuff
        waveSlider.maxValue = levelTime;
        waveSlider.value = 0;
        fill.color = gradient.Evaluate(0f);
        // Somehow draw lines on waveSlider to represent wave
        // Dunno How to do it yet :(

        // Start the constant wave
        StartCoroutine(WaveStart(nEnemies, 1, true));
    }

    void Update(){
        timeCtr += Time.deltaTime;
        waveSlider.value = (int)timeCtr;
        fill.color = gradient.Evaluate(waveSlider.normalizedValue);
        // Spawn waves based on time (not yet)
    }

    IEnumerator WaveStart(int n_enemies, int n, bool constant){
        if(constant){
            waveInfo.text = "Level " + manager.level;
            // Spawn Enemies Constantly till level ends
            while (timeCtr < levelTime){
                SpawnEnemy();
                yield return new WaitForSeconds(Random.Range(0.5f, enemySpawnTime));
            }
            manager.LevelEnd();
        }
        else{
            // Show some info
            string info = string.Format("Wave {0} started", n);
            Debug.Log(info);
            waveInfo.text = info;

            // Spawn Many Enemies at once
            for(int i = 0; i < n_enemies; i++){
                SpawnEnemy();
                // Spawn next one immediately or at a random interval??
                yield return new WaitForSeconds(Random.Range(0.001f, timeBtwnSpawns));
            }
            WaveEnd();
        }
    }

    void SpawnEnemy(){
        GameObject enemy = enemies[Random.Range(0, enemies.Length)];
        Vector3 pos = new Vector3(Random.Range(-25, 25), enemySpawnHeight, Random.Range(-10, 10));
        int depth = 100;

        while(!GoodPosition(pos) || depth <= 0){
            pos = new Vector3(Random.Range(-25, 25), enemySpawnHeight, Random.Range(-10, 10));
            depth--;
        }
        Instantiate(enemy, pos, Quaternion.identity, transform);
        enemyCount++;
    }

    bool GoodPosition(Vector3 pos){
        Ray cameraRay = new Ray(pos, -Vector3.up);
        RaycastHit hit;
        if(Physics.Raycast(cameraRay, out hit)){
            if (hit.transform.CompareTag("Ground")){
                var yup = hit.point + new Vector3(0, 0.2f, 0);
                var s = Instantiate(pointItLands, yup, Quaternion.Euler(90, 0, 0)).transform;
                Destroy(s.gameObject, 1f);
                return true;
            }
            else{
                return false;
            }
        }
        return false;
    }

    void WaveEnd(){
        // Stuff to do when wave ends
        Debug.Log("Wave Ended.");
    }
}

public enum EnemyType{
    SLIG, // Slow + Big
    AVMED, // Average + Medium
    SMAF // Small + Fast
}
