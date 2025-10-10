using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private BoxCollider2D cameraBoundingBox;

    [SerializeField] List<GameObject> objectsInScene;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        objectsInScene = new List<GameObject>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(cameraBoundingBox.bounds.center, cameraBoundingBox.bounds.size);
    }
}
