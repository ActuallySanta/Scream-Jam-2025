using NUnit.Framework;
using UnityEngine;
using System;

public class Interactor : MonoBehaviour
{
    [SerializeField] private float interactionRadius;
    [SerializeField] private LayerMask interactionMask;

    public event Action<Interactable> OnInteractableInRange;

    // Update is called once per frame
    void Update()
    {
        DetectInteractable();
    }

    void DetectInteractable()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, interactionRadius, interactionMask);
        if (hit)
        {
            //Debug.Log("interactable found");
            OnInteractableInRange?.Invoke(hit.GetComponent<Interactable>());
        }
        else
        {
            OnInteractableInRange?.Invoke(null);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
