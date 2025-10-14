using UnityEngine;

public class TestHazard : HazardController
{
    public override void PerformHazard(IPlayer player)
    {
        player.HurtPlayer();
        Debug.Log("Called to hurt the player");
    }
}
