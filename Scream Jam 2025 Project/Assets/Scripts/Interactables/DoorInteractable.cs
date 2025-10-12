using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class DoorInteractable : Interactable
{
    [SerializeField] private List<MonoBehaviour> conditionComponents = new(); // assign in inspector
    private List<IUnlockCondition> conditions = new();

    private bool isUnlocked = false;

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
    }

    private void Update()
    {
        if (CheckConditionsMet())
        {
            isUnlocked = true;
        }
        else
        {
            isUnlocked = false;
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
}
