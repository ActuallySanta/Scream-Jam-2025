using UnityEngine;

public class CameraBounds : MonoBehaviour
{
    [SerializeField] private GameObject cameraObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            cameraObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            cameraObject.SetActive(false);
        }
    }
}
