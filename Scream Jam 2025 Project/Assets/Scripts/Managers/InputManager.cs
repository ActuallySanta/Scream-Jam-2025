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

    public event Action<InputAction.CallbackContext> OnJump;
    public event Action<InputAction.CallbackContext> OnThrow;
    public event Action<InputAction.CallbackContext> OnInteract;

    public Vector2 MoveInput { get => move.ReadValue<Vector2>(); }

    public event Action<InputAction.CallbackContext> OnMove;

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
    }

    private void OnEnable()
    {
        move = actions.Player.Move;
        move.Enable();

        jump = actions.Player.Jump;
        jump.performed += HandleJumpPerformed;
        jump.Enable();

        skullThrow = actions.Player.Throw;
        skullThrow.performed += HandleThrowPerformed;
        skullThrow.Enable();

        interact = actions.Player.Interact;
        interact.performed += HandleInteractPerformed;
        interact.Enable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(gameObject);
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
}
