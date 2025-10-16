using UnityEngine;

public class BouncePadController : HazardController
{
    [SerializeField] private float force;
    [SerializeField] private ForceMode2D forceMode;
    [SerializeField] private Sprite steppedOnSprite;
    [SerializeField] private Sprite noPressureSprite;

    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public override void PerformHazard(IPlayer player)
    {
        Debug.Log("Bouncing!");
        player.AddForce(transform.up * force, forceMode);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            sr.sprite = steppedOnSprite;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            sr.sprite = noPressureSprite;
        }
    }
}
