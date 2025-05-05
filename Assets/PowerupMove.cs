using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupMove : MonoBehaviour
{
    public float speed = 0.5f; // Speed of the movement
    public float height = 0.3f; // Height of the movement

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * speed) * height;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}
