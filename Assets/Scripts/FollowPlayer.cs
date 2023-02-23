using System;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float speed = 0.5f;
    
    Vector3 offset;

    void Start()
    {
        offset = transform.position;
    }

    void FixedUpdate()
    {
        transform.position += ((player.transform.position + offset) - transform.position) * (speed * 0.1f);
    }
}