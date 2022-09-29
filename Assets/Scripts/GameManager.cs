using UnityEngine;

public class GameManager : MonoBehaviour {

    public int level = 1;
    public bool gamePaused = false;

    [SerializeField] private GameObject levelEndUI;
    [SerializeField] private GameObject pauseGameUI;

    // Start is called before the first frame update
    void Start() {
        levelEndUI.SetActive(false);
        pauseGameUI.SetActive(gamePaused);
    }

    void SetGameState(bool state){
        gamePaused = state;
        if(gamePaused){
            PauseGame();
        }
        else{
            ResumeGame();
        }
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetButtonDown("Pause")){
            SetGameState(!gamePaused);
        }
    }

    public void LevelEnd(){
        Debug.Log(string.Format("Level {0} Ended", level));
        levelEndUI.SetActive(true);
        Time.timeScale = 0; // Stop Time
    }

    public void PauseGame(){
        // Enable Some Pause Menu
        pauseGameUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeGame(){
        // Disable the pause menu
        pauseGameUI.SetActive(false);
        Time.timeScale = 1;
    }
}
