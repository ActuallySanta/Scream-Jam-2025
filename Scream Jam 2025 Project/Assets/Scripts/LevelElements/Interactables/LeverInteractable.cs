using UnityEngine;

public class LeverInteractable : Interactable, IUnlockCondition
{

    private bool pulled;
    public bool unlockConditionMet => pulled;
    
    private SpriteRenderer sr;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        //sr.color = Color.red;
    }

    public override void Interact()
    {
        base.Interact();

        if (!pulled)
        {
            pulled = true;
            //sr.color = Color.green;
        }
        else
        {
            pulled = false;
            //sr.color = Color.red;
        }

        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }
}
