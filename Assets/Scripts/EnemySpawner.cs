using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    [SerializeField] private GameManager manager;

    // Enemies normally (Should be small, but not zero)
    [SerializeField] private int minEnemies = 2;
    [SerializeField] private int maxEnemies = 5;

    // enemies spawn normally in random time between
    [SerializeField] private float enemySpawnTime = 2f;
    // Time of the total level
    [SerializeField] private float levelTime = 2 * 60f; 
    [SerializeField] private float enemySpawnHeight = 10;

    [SerializeField] private GameObject[] enemies;
    [SerializeField] private GameObject pointItLands;

    private int enemyCount = 0;
    private float timeCtr = 0;

    // Start is called before the first frame update
    void Start() {
        // Constant enemies from the start
        int nEnemies = Random.Range(minEnemies, maxEnemies);

        // Start the constant wave
        StartCoroutine(WaveStart(nEnemies));
    }

    void Update(){
        timeCtr += Time.deltaTime;
        // Spawn waves based on time (not yet)
    }

    IEnumerator WaveStart(int n_enemies){
        // Spawn Enemies Constantly till level ends
        while (timeCtr < levelTime){
            SpawnEnemy();
            yield return new WaitForSeconds(Random.Range(0.5f, enemySpawnTime));
        }
        manager.LevelEnd();
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
}

public enum EnemyType{
    SLIG, // Slow + Big
    AVMED, // Average + Medium
    SMAF // Small + Fast
}
