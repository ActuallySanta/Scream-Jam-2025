using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Interactable : MonoBehaviour
{
    public event Action OnInteract;
    public virtual void Interact()
    {
        OnInteract?.Invoke();
    }
}
