using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class InputManagerScript : MonoBehaviour
{
    public UnityEvent<InputAction.CallbackContext> onMove;
    public UnityEvent<InputAction.CallbackContext> onJump;
    public UnityEvent<InputAction.CallbackContext> onThrow;
    public UnityEvent<InputAction.CallbackContext> onInteract;

    public GameObject Player;
    private PlayerController playerController;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = Player.GetComponent<PlayerController>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        onMove.Invoke(context);
    }

    public void OnThrow(InputAction.CallbackContext context)
    {
        onThrow.Invoke(context);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        onJump.Invoke(context);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        playerController.OnInteract(context);
    }
}
