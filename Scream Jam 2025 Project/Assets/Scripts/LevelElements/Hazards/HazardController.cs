using UnityEngine;

public abstract class HazardController : MonoBehaviour
{
    public abstract void PerformHazard(IPlayer player);
}
