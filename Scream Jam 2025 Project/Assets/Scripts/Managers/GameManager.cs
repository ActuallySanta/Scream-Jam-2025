using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    //Respawning
    public GameObject playerGameObject { get;private set; }
    [SerializeField] private GameObject playerPrefab;
    public delegate void OnPlayerRespawnEventHandler();
    public event OnPlayerRespawnEventHandler OnPlayerRespawn;


    //Checkpoints
    public GameObject currCheckpoint { get; private set; }
    public delegate void OnCheckpointChangeEventHandler();
    public event OnCheckpointChangeEventHandler OnCheckpointChange;

    //Candy
    public List<CandyObject> accquiredCandy = new List<CandyObject>();
    public delegate void OnCandyGetEventHandler();
    public event OnCandyGetEventHandler OnCandyGet;

    //Pause Menu
    public bool isPaused { get; private set; }
    public delegate void OnPauseEventHandler();
    public event OnPauseEventHandler OnPause;

    void Awake()
    {
        //Implement the singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //Set the game object as persistent
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Pause the entire game and alert the subscribed methods
    /// </summary>
    public void TogglePause()
    {
        OnPause?.Invoke();

        if (!isPaused)
        {
            Time.timeScale = 1.0f;
            isPaused = false;
        }
        else
        {
            Time.timeScale = 0;
            isPaused = true;
        }
    }

    /// <summary>
    /// Set the new checkpoint and notify all methods subscribed
    /// </summary>
    /// <param name="_checkpoint">The new checkpoint that needs to be set</param>
    public void SetCheckpoint(GameObject _checkpoint)
    {
        if (currCheckpoint == null || _checkpoint != currCheckpoint)
        {
            currCheckpoint = _checkpoint;
            OnCheckpointChange?.Invoke();
            Debug.Log("New checkpoint is: " + currCheckpoint.name);
        }
    }

    /// <summary>
    /// Add a candy instance to the list of acquired candies and notify all methods subscribed
    /// </summary>
    /// <param name="_candy">The accquired candy</param>
    public void GetCandy(CandyObject _candy)
    {
        if (!accquiredCandy.Contains(_candy))
        {
            OnCandyGet?.Invoke();
            accquiredCandy.Add(_candy);
        }
    }

    public void RespawnPlayer()
    {
        Destroy(playerGameObject);
        playerGameObject = null;

        OnPlayerRespawn?.Invoke();
        playerGameObject = Instantiate(playerPrefab, currCheckpoint.transform.position,Quaternion.identity);
        
    }
}
