using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputManagerScript : MonoBehaviour
{
    public UnityEvent<InputAction.CallbackContext> onMove;
    public UnityEvent<InputAction.CallbackContext> onJump;

    private PlayerController playerController;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GameManager.instance.playerGameObject.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        onMove.Invoke(context);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        onJump.Invoke(context);
    }
}
