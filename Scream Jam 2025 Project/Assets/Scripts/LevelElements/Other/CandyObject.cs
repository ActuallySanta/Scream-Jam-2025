using UnityEngine;


[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class CandyObject : MonoBehaviour
{
    private SpriteRenderer sr;
    private Collider2D col;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.instance.CollectCandy(this);
            Hide();
        }
    }

    public void Show()
    {
        sr.enabled = true;
        col.enabled = true;
    }

    public void Hide()
    {
        sr.enabled = false;
        col.enabled = false;
    }
}
