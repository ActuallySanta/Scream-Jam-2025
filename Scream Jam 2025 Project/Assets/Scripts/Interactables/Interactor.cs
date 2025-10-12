using NUnit.Framework;
using UnityEngine;
using System;
using System.Linq;

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
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactionRadius, interactionMask);
        
        Collider2D nearest = hits
            .OrderBy(h => (h.transform.position - transform.position).sqrMagnitude)
            .FirstOrDefault();

        if (nearest)
        {
            //Debug.Log("interactable found");
            OnInteractableInRange?.Invoke(nearest.GetComponent<Interactable>());
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
