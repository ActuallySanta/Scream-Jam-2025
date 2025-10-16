using System;
using Unity.VisualScripting;
using UnityEngine;

public class ButtonControlller : MonoBehaviour, IUnlockCondition
{
    [SerializeField] private Sprite pressedSprite;
    [SerializeField] private Sprite releaseSprite;

    public event Action OnButtonPressed;
    public event Action OnButtonReleased;

    [SerializeField] private LayerMask buttonLayerMask;
    private bool pressed;
    public bool unlockConditionMet => pressed;

    private SpriteRenderer sr;
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        UpdateSprite();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int collisionObjLayer = collision.gameObject.layer;
        if (((1 << collisionObjLayer) & buttonLayerMask.value) != 0) // checks if object colliding w this button shares a layer in this layer mask
        {
            Debug.Log("Button pressed");
            pressed = true;
            OnButtonPressed?.Invoke();
            UpdateSprite();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        int collisionObjLayer = collision.gameObject.layer;
        if (((1 << collisionObjLayer) & buttonLayerMask.value) != 0) // checks if object colliding w this button shares a layer in this layer mask
        {
            Debug.Log("button released");
            pressed = false;
            OnButtonReleased?.Invoke();
            UpdateSprite();
        }
    }

    private void UpdateSprite()
    {
        if (pressed)
        {
            sr.sprite = pressedSprite;
        }
        else
        {
            sr.sprite = releaseSprite;
        }
    } 
}
