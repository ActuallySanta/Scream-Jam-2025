using UnityEngine;

public partial class HazardController : MonoBehaviour
{
    protected Collider2D trigger;

    protected virtual void Start()
    {
        trigger = GetComponent<Collider2D>();
    }

    public virtual void PerformHazard(IPlayer player) { }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        IPlayer player;
        if (collision.gameObject.TryGetComponent<IPlayer>(out player))
        {
            Debug.Log("attempted interaction with player");
            PerformHazard(player);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        IPlayer player;
        if (collision.gameObject.TryGetComponent<IPlayer>(out player))
        {
            Debug.Log("attempted interaction with player");
            PerformHazard(player);
        }
    }
}
