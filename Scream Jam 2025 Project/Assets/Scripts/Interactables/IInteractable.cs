using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class IInteractable
{
    public UnityEvent<Action> OnInteract;

    public virtual void Interact()
    {
    }
}
