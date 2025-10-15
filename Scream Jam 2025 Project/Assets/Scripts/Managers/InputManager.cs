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

    public event Action<InputAction.CallbackContext> OnMove;
    public event Action<InputAction.CallbackContext> OnJump;
    public event Action<InputAction.CallbackContext> OnThrow;
    public event Action<InputAction.CallbackContext> OnInteract;
    public event Action<InputAction.CallbackContext> OnPause;

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

        pause = actions.Player.Pause;
        pause.performed += HandlePausePerformed;
        pause.Enable();
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

    public void EnablePlayerEvents()
    {
        playerActionsEnabled = true;
        move.Enable();
        jump.Enable();
        skullThrow.Enable();
        interact.Enable();
    }

    public void DisablePlayerEvents()
    {
        playerActionsEnabled = false;
        move.Disable();
        jump.Disable(); 
        skullThrow.Disable(); 
        interact.Disable();
    }
}
