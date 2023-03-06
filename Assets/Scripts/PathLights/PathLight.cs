using System;
using UnityEngine;

public class PathLight : MonoBehaviour
{
    [SerializeField] float speed = 0.01f;
    [SerializeField] float maxOpacity = 0.5f;
    
    SpriteRenderer rend;
    GameObject player;
    
    void Start()
    {
        rend = GetComponentInChildren<SpriteRenderer>();
        rend.color = new Color(1, 1, 1, 0);

        player = GameObject.FindWithTag("Player");
    }

    void FixedUpdate()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        float distanceOffset = 0.5f / distance;
        
        if (rend.color.a < maxOpacity) rend.color = new Color(1, 1, 1, rend.color.a + speed);

        rend.color = new Color(1, 1, 1, rend.color.a * distanceOffset);
    }
}