using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class DoorInteractable : MonoBehaviour
{
    [SerializeField] private List<MonoBehaviour> conditionComponents = new(); // assign in inspector
    private List<IUnlockCondition> conditions = new();

    private bool isUnlocked = false;

    [SerializeField] private bool conditionUnlocks = true;

    private Collider2D collider;
    private SpriteRenderer sprite;

    private void Awake()
    {
        // Filter only components that implement IUnlockCondition
        foreach (var comp in conditionComponents)
        {
            if (comp is IUnlockCondition condition)
            {
                conditions.Add(condition);
            }   
            else
            {
                Debug.LogWarning($"{comp.name} does not implement IUnlockCondition!");
            }
        }

        collider = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        switch (conditionUnlocks)
        {
            case true:
                if (CheckConditionsMet())
                {
                    collider.enabled = false;
                    sprite.color = Color.green;
                }
                else
                {
                    collider.enabled = true;
                    sprite.color = Color.red;
                }
                break;
            case false:
                if (CheckConditionsMet())
                {
                    collider.enabled = true;
                    sprite.color = Color.red;
                }
                else
                {
                    collider.enabled = false;
                    sprite.color = Color.green;
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
    /*
    public override void Interact()
    {
        base.Interact();

        Debug.Log("Door used!");

        if (isUnlocked)
        {
            Debug.Log("Door is unlocked! lemme in");
        }
        else
        {
            Debug.Log("Door is not unlocked. BLOCKED");
        }
    }
    */
}
