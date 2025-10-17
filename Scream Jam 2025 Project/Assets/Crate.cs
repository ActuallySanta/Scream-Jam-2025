using UnityEngine;

public class Crate : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject crateObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.instance.OnReset += ResetToCheckpoint;    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ResetToCheckpoint()
    {
        crateObject.transform.position = spawnPoint.position;
    }
}
