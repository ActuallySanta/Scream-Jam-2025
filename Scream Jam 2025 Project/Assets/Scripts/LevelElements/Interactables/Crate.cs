using UnityEngine;
using System;

public class Crate : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject crateObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        GameManager.instance.OnReset += ResetToCheckpoint;    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ResetToCheckpoint()
    {
        Debug.Log("resetToCheckpoint");
        crateObject.transform.position = spawnPoint.position;
    }
}
