using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class DoorInteractable : MonoBehaviour
{
    [SerializeField] private List<MonoBehaviour> conditionComponents = new(); // assign in inspector
    private List<IUnlockCondition> conditions = new();
    [SerializeField] private Sprite openDoorSprite;
    [SerializeField] private Sprite closeDoorSprite;

    private bool isUnlocked = false;

    [SerializeField] private bool inverseConditional = true;

    private Collider2D collider;
    private SpriteRenderer sr;

    private void Awake()
    {
        if (conditionComponents.Count > 0)
        {
            // Filter only components that implement IUnlockCondition
            foreach (var comp in conditionComponents)
            {
                if (comp != null && comp is IUnlockCondition condition)
                {
                    conditions.Add(condition);
                }
                else
                {
                    Debug.LogWarning($"{comp.name} does not implement IUnlockCondition!");
                }
            }
        }
        else
        {
            Debug.LogWarning("No unlock conditions are on this door. Is this intentional?");
        }    

        collider = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        switch (inverseConditional)
        {
            case true:
                if (CheckConditionsMet())
                {
                    collider.enabled = false;
                    sr.sprite = openDoorSprite;
                    //sprite.color = Color.green;
                }
                else
                {
                    collider.enabled = true;
                    sr.sprite = closeDoorSprite;
                    //sprite.color = Color.red;
                }
                break;
            case false:
                if (CheckConditionsMet())
                {
                    collider.enabled = true;
                    sr.sprite = closeDoorSprite;
                    //sprite.color = Color.red;
                }
                else
                {
                    collider.enabled = false;
                    sr.sprite = openDoorSprite;
                    //sprite.color = Color.green;
                }
                break;
        }
    }

    private bool CheckConditionsMet()
    {
        foreach (var cond in conditions)
        {
            if (!cond.unlockConditionMet)
            {
                return false;
            }
        }
        return true;
    }
}
