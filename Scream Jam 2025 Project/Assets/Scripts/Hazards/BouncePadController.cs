using UnityEngine;

public class BouncePadController : HazardController
{
    [SerializeField] private float force;
    [SerializeField] private ForceMode2D forceMode;

    public override void PerformHazard(IPlayer player)
    {
        player.AddForce(transform.up * force, forceMode);
    }
}
