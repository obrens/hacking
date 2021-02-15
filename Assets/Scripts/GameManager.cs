using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton implementation
    public static GameManager Instance;
    
    void Awake() 
    {
        if(Instance == null) 
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } 
        else if(Instance != this) 
        {
            Destroy(this);
        }
    }
    #endregion

    public LevelSettings LevelSettings = new LevelSettings();

    private void Start()
    {
        EventManager.StartListening("start level", StartLevel);
    }

    private void OnDestroy() 
    {
        EventManager.StopListening("start level", StartLevel);
    }
    
    private void StartLevel() 
    {
        SceneManager.LoadScene("Level");
    }
}

public class LevelSettings
{
    // 5 - 15
    public int NodeCount = 10;
    // 1 - 4
    public int TreasureNodeCount = 2;
    // 1 - 2
    public int FirewallNodeCount = 1;
    // 0 - 4
    public int SpamNodeCount = 1;
    // 0.1 - 0.9
    public float SpamNodeDecrease = 0.5f;
    // 1 - 20
    public float TrapDelayTime = 5;
}
