using UnityEngine;

public class TestHazard : HazardController
{
    public override void PerformHazard(IPlayer player)
    {
        Debug.Log("Test");
    }
}
