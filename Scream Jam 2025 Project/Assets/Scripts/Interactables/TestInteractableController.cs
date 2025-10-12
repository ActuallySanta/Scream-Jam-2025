using System.Collections;
using UnityEngine;

public class TestInteractableController : Interactable
{
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color interactedColor;
    [SerializeField] private float delay;

    private SpriteRenderer sr;

    private Coroutine currentCountdown;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = defaultColor;
    }

    public override void Interact()
    {
        base.Interact();

        sr.color = interactedColor;
        if (currentCountdown != null)
        {
            StopCoroutine(currentCountdown);
        }
        
        currentCountdown = StartCoroutine(ResetColorAfterDelay(delay));
    }

    private IEnumerator ResetColorAfterDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        sr.color = defaultColor;
    }
}
