using UnityEngine;

public class LeverInteractable : Interactable, IUnlockCondition
{
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color interactedColor;

    private bool pulled;
    public bool unlockConditionMet => pulled;
    
    private SpriteRenderer sr;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = defaultColor;
    }

    public override void Interact()
    {
        base.Interact();

        if (!pulled)
        {
            pulled = true;
            sr.color = interactedColor;
        }
        else
        {
            pulled = false;
            sr.color = defaultColor;
        }
    }
}
