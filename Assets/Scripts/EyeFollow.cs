using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EyeFollow : MonoBehaviour
{
    [SerializeField] Transform eyes;
    
    [Header("Follow")]
    [SerializeField] bool follow = true;
    enum FollowTypes { Cursor, Player }
    [SerializeField] FollowTypes followType;
    [SerializeField] Vector2 lookAmount = new Vector2(0.1f, 0.15f);
    
    [Header("Blink")]
    [SerializeField] bool blink = true;
    [SerializeField] float blinkInterval = 3;
    [SerializeField, Range(0, 1)] float winkChance = 0.2f;

    Vector3 startPosition;
    Transform player;
    bool isBlinking;
    Transform blinkEye;

    void Start()
    {
        startPosition = eyes.localPosition;
        
        if (blink) Invoke(nameof(Blink), Random.Range(blinkInterval, blinkInterval * 2));

        player = GameObject.FindWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        if (follow)
        {
            Vector2 temp = Vector2.zero;

            if (followType.Equals(FollowTypes.Cursor))
            {
                temp = new Vector2(
                    (Input.mousePosition.x / Screen.width) - 0.5f,
                    (Input.mousePosition.y / Screen.height) - 0.5f);
            }
            else if (followType.Equals(FollowTypes.Player))
            {
                Vector3 dir = (player.position - transform.position).normalized;
                temp = new Vector2(dir.x, dir.z);
            }
            
            Vector2 normalizedPosition = new Vector2(
                -lookAmount.x * temp.x,
                lookAmount.y * temp.y);
            
            //if (followType.Equals(FollowTypes.Player)) print(normalizedPosition);

            eyes.localPosition = startPosition + (Vector3)normalizedPosition;
        }
    }

    void Blink()
    {
        if (!isBlinking)
        {
            blinkEye = Random.Range(0f, 1f) <= winkChance ? eyes.GetChild(Random.Range(0, 2)) : eyes;
        }
        
        Vector3 localScale = blinkEye.localScale;
        blinkEye.localScale = new Vector3(localScale.x, localScale.y * (isBlinking ? 2 : 0.5f), localScale.z);
        
        isBlinking = !isBlinking;

        Invoke(nameof(Blink), isBlinking ? 0.5f : Random.Range(blinkInterval, blinkInterval * 2));
    }
}