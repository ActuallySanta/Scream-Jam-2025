using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private BoxCollider2D attachedCol;

    private void Start()
    {
        attachedCol = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Update the active checkpoint when the player touches the trigger
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Sent Checkpoint info");
            GameManager.instance.SetCheckpoint(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        //Draw a visualization of the checkpoint
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(attachedCol.bounds.center, attachedCol.bounds.size);
    }
}
