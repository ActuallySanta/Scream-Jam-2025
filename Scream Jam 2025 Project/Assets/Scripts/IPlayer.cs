using UnityEngine;

// how other objects are allowed to interact with the player (ie hazards)
public interface IPlayer
{
    public void HurtPlayer();
    public void AddForce(Vector2 force, ForceMode2D mode = ForceMode2D.Force);

    // add more methods as needed
}
