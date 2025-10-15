using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    //Respawning
    public GameObject playerGameObject;
    [SerializeField] private GameObject playerPrefab;
    public delegate void OnPlayerRespawnEventHandler();
    public event OnPlayerRespawnEventHandler OnPlayerRespawn;
    private InputAction respawnPlayer;

    //Checkpoints
    public GameObject currCheckpoint { get; private set; }
    public delegate void OnCheckpointChangeEventHandler();
    public event OnCheckpointChangeEventHandler OnCheckpointChange;

    //Candy
    public List<CandyObject> accquiredCandy = new List<CandyObject>();
    public int TotalCandyAccquired { get => accquiredCandy.Count; }
    public event Action OnAddCandy;

    //Pause Menu
    public bool isPaused { get; private set; }
    public event Action OnPause;

    //Debug Mode
    public bool isDebugging { get; private set; }

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

        if (!InputManager.Instance.PlayerActionsEnabled)
        {
            InputManager.Instance.EnablePlayerEvents();
        }
        InputManager.Instance.OnPause += HandlePauseInput;

        //Initialize Bools
        isDebugging = false;
    }

    private void HandlePauseInput(InputAction.CallbackContext ctx)
    {
        TogglePause();
    }

    private void Update()
    {
        //Debugging Buttons
        if (isDebugging)
        {
            Debug.Log("Debugging!");
            if (Input.GetKeyDown(KeyCode.R))
            {
                RespawnPlayer();
            }
        }
    }

    /// <summary>
    /// Allow toggling of the debugging keybinds
    /// </summary>
    public void ToggleDebugMode(InputAction.CallbackContext context)
    {
        if (context.performed) isDebugging = !isDebugging;

    }

    /// <summary>
    /// Pause the entire game and alert the subscribed methods
    /// </summary>
    public void TogglePause()
    {
        OnPause?.Invoke();

        if (!isPaused)
        {
            InputManager.Instance.EnablePlayerEvents();
            Time.timeScale = 1.0f;
            isPaused = false;
        }
        else
        {
            InputManager.Instance.DisablePlayerEvents();
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
    public void CollectCandy(CandyObject _candy)
    {
        if (!accquiredCandy.Contains(_candy))
        {
            OnAddCandy?.Invoke();
            accquiredCandy.Add(_candy);
        }
    }

    public void RespawnPlayer()
    {
        if (currCheckpoint != null)
        {
            Checkpoint checkpoint = currCheckpoint.GetComponent<Checkpoint>();

            if (checkpoint.returnHeadOnLoad)
            {
                PlayerController playerController = playerGameObject.GetComponent<PlayerController>();
                playerController.ResetHead();
            }

            Destroy(playerGameObject);
            playerGameObject = null;

            OnPlayerRespawn?.Invoke();
            playerGameObject = Instantiate(playerPrefab, currCheckpoint.transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("No Checkpoint Set");
        }
    }
}
