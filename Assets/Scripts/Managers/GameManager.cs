using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool isVeg = true;
    public int stageTillBoss = 6;
    private int stageRemainTillBoss;

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else if (Instance != this) {
            Destroy(gameObject);
        }
    }
    void Start() {
        SceneManager.LoadScene("Scenes/Menu") ;
        stageRemainTillBoss = stageTillBoss;
    }
    void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
             SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex ) ;
        }
        if (Input.GetKeyDown(KeyCode.Space) && stageRemainTillBoss > 0) {
            SwitchStageTheme();
            UpdateStageRemaining();
            SceneManager.LoadScene("Scenes/MainScene");
        } else if (stageRemainTillBoss == 0) {
            UpdateStageRemaining();
            SceneManager.LoadScene("Scenes/BossScene");
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene("Scenes/Menu") ;
        }
    }
    void SwitchStageTheme() {
        isVeg = !isVeg;
    }
    void UpdateStageRemaining() {
        stageRemainTillBoss--;
    }
}