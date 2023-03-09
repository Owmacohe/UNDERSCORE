using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float speed = 0.5f;
    [SerializeField] float shakeAmount = 0.15f;
    
    Vector3 offset;
    Vector3 shakeOffset;

    void Start()
    {
        offset = transform.position;
        
        ChangeShakeOffset();
    }

    void FixedUpdate()
    {
        transform.position += ((player.position + offset + shakeOffset) - transform.position) * (speed * 0.05f);
    }

    void ChangeShakeOffset()
    {
        shakeOffset = new Vector3(
            Random.Range(-shakeAmount, shakeAmount), 
            Random.Range(-shakeAmount, shakeAmount), 
            0);
        
        Invoke(nameof(ChangeShakeOffset), 0.75f);
    }
}