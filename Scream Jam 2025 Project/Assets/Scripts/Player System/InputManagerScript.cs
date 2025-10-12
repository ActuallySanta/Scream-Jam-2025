using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[System.Serializable]
public class InputActionEvent : UnityEvent<InputAction.CallbackContext> { }


public class InputManagerScript : MonoBehaviour
{
    public InputActionEvent onMove;
    public InputActionEvent onJump;
    public InputActionEvent onThrow;
    public InputActionEvent onInteract;

    [SerializeField] private PlayerController player;

    private void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            Debug.Log("E key pressed");
            onInteract.Invoke(new InputAction.CallbackContext());
        }
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
        Debug.Log("On interact fired   " + onInteract.GetPersistentEventCount());
        onInteract.Invoke(context);
    }
}
