using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene("Scenes/Menu") ;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
             SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex ) ;
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
             SceneManager.LoadScene("Scenes/MainScene");
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
             SceneManager.LoadScene("Scenes/Menu") ;
        }
    }
}

public enum GameState {

}