using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    GameManager Instance;
    
    void Awake() {
        if(Instance == null) {
            Instance = this;
        } else if(Instance != this) {
            Destroy(this);
        }
        DontDestroyOnLoad(gameObject);
        EventManager.StartListening("start level", StartLevel);
    }

    private void OnDestroy() {
        EventManager.StopListening("start level", StartLevel);
    }
    
    private void StartLevel() {
        SceneManager.LoadScene("Level");
    }
}
