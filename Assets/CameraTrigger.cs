using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    public float targetYPosition;
    public Camera mainCamera; // Reference to the main camera
    public GameObject cinemachineCamera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Check if the player entered the collider
        {
            Vector3 newPosition = mainCamera.transform.position;
            newPosition.y = targetYPosition; // Set the Y position to the target value
            mainCamera.transform.position = newPosition;
            cinemachineCamera.transform.position = newPosition;
        }
    }
}
