using UnityEngine;

public class ChildHazard : HazardController
{
    public override void PerformHazard(IPlayer player)
    {
        Debug.Log("Test");
    }
}
