using System;
using Unity.VisualScripting;
using UnityEngine;

public class ButtonControlller : MonoBehaviour
{
    public event Action OnButtonPressed;
    public event Action OnButtonReleased;

    [SerializeField] private LayerMask buttonLayerMask;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int collisionObjLayer = collision.gameObject.layer;
        if (((1 << collisionObjLayer) & buttonLayerMask.value) != 0) // checks if object colliding w this button shares a layer in this layer mask
        {
            Debug.Log("Button pressed");
            OnButtonPressed?.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("button released");
        OnButtonReleased?.Invoke();
    }
}
