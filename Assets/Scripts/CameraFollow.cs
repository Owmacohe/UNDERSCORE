using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float speed = 0.5f;
    
    Vector3 offset;

    void Start()
    {
        offset = transform.position;
    }

    void FixedUpdate()
    {
        transform.position += ((player.position + offset) - transform.position) * (speed * 0.1f);
    }
}