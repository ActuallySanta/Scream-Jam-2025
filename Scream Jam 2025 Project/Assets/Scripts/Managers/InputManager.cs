using System;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class InputManager : MonoBehaviour
{
    private static InputManager instance;
    public static InputManager Instance { get => instance; }

    private InputSystem_Actions actions;
    private InputAction move;
    private InputAction jump;
    private InputAction skullThrow;
    private InputAction interact;
    private InputAction pause;
    private InputAction forceRespawn;

    public event Action<InputAction.CallbackContext> OnMove;
    public event Action<InputAction.CallbackContext> OnJump;
    public event Action<InputAction.CallbackContext> OnThrow;
    public event Action<InputAction.CallbackContext> OnInteract;
    public event Action<InputAction.CallbackContext> OnPause;
    public event Action<InputAction.CallbackContext> OnForceRespawn;

    /// <summary>
    /// Vector2 value of the move input
    /// </summary>
    public Vector2 MoveInput { get => move.ReadValue<Vector2>(); }

    private bool playerActionsEnabled;
    public bool PlayerActionsEnabled { get => playerActionsEnabled; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        actions = new InputSystem_Actions();
        playerActionsEnabled = false;
    }

    private void OnEnable()
    {
        move = actions.Player.Move;
        move.performed += HandleMovePerformed;

        jump = actions.Player.Jump;
        jump.performed += HandleJumpPerformed;

        skullThrow = actions.Player.Throw;
        skullThrow.performed += HandleThrowPerformed;

        interact = actions.Player.Interact;
        interact.performed += HandleInteractPerformed;

        forceRespawn = actions.Player.ForceRespawn;
        forceRespawn.performed += HandleForceRespawnPerformed;

        pause = actions.Player.Pause;
        pause.performed += HandlePausePerformed;
        pause.Enable(); // this needs to be enabled seperatly
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void HandleMovePerformed(InputAction.CallbackContext ctx)
    {
        OnMove?.Invoke(ctx);
    }

    private void HandleJumpPerformed(InputAction.CallbackContext ctx)
    {
        OnJump?.Invoke(ctx);
    }

    private void HandleThrowPerformed(InputAction.CallbackContext ctx)
    {
        OnThrow?.Invoke(ctx);
    }

    private void HandleInteractPerformed(InputAction.CallbackContext ctx)
    {
        OnInteract?.Invoke(ctx);
    }

    private void HandlePausePerformed(InputAction.CallbackContext ctx)
    {
        OnPause?.Invoke(ctx);
    }

    private void HandleForceRespawnPerformed(InputAction.CallbackContext ctx)
    {
        OnForceRespawn?.Invoke(ctx);
    }

    /// <summary>
    /// Enables all input events related to player movement
    /// </summary>
    public void EnablePlayerMovementActions()
    {
        playerActionsEnabled = true;
        move.Enable();
        jump.Enable();
        skullThrow.Enable();
        interact.Enable();
        forceRespawn.Enable();
    }

    /// <summary>
    /// Disables all input events related to player movement
    /// </summary>
    public void DisablePlayerMovementActions()
    {
        playerActionsEnabled = false;
        move.Disable();
        jump.Disable();
        skullThrow.Disable();
        interact.Disable();
        forceRespawn.Disable();
    }
}
