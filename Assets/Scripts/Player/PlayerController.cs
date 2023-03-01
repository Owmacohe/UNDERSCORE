using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 10;
    [SerializeField] float clickRadius = 2;
    [SerializeField] ConversationManager convoManager;

    Animator anim;
    Rigidbody rb;
    Camera main;

    Vector3 target, direction;
    bool moving, clickedNPC, pointerOverUI;
    float speedBoost;

    GameObject[] allNPCs;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        main = Camera.main;

        ResetMovement(false);

        allNPCs = GameObject.FindGameObjectsWithTag("NPC");
    }

    void Update()
    {
        pointerOverUI = EventSystem.current.IsPointerOverGameObject();
    }

    void OnFire()
    {
        if (!pointerOverUI)
        {
            Ray ray = main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit))
            {
                if (Vector3.Distance(transform.position, hit.point) > clickRadius &&
                    !hit.transform.gameObject.Equals(gameObject))
                {
                    clickedNPC = hit.transform.gameObject.CompareTag("NPC");

                    if (clickedNPC) convoManager.bubblePosition = hit.transform.position;
                    else
                    {
                        foreach (GameObject i in allNPCs)
                        {
                            if (Vector3.Distance(i.transform.position, hit.point) <= clickRadius)
                            {
                                clickedNPC = true;
                                convoManager.bubblePosition = i.transform.position;
                                break;
                            }
                        }
                    }

                    target = clickedNPC ? hit.transform.position : hit.point;
                    direction = target - transform.position;

                    moving = true;
                    anim.SetBool("isWalking", true);
                    if (direction.magnitude >= 15)
                    {
                        anim.SetBool("isJogging", true);
                        speedBoost = 1.5f;
                    }

                    direction = direction.normalized;

                    if (clickedNPC) target -= direction * clickRadius * 3;
                }
                else
                {
                    ResetMovement(false);

                    anim.SetTrigger("wiggle");
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (moving)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target);
            float speedFactor = speed * 0.001f * distanceToTarget * speedBoost;
            
            float max = speedBoost > 1 ? 0.15f : 0.1f;
            float min = 0.05f;

            if (speedFactor > max) speedFactor = max;
            if (speedFactor < min) speedFactor = min;

            transform.Rotate(Vector3.up * (Vector3.Angle(transform.forward, (transform.position + direction) - transform.position) * 0.05f));
            rb.position += direction * speedFactor;
            
            if (distanceToTarget <= 5)
            {
                if (distanceToTarget <= 0.1f)
                {
                    ResetMovement(clickedNPC);
                    clickedNPC = false;
                }
                else
                {
                    anim.SetBool("isJogging", false);
                    speedBoost = 1;
                }
            }
        }
    }

    void ResetMovement(bool endOfMovement)
    {
        moving = false;
        anim.SetBool("isJogging", false);
        anim.SetBool("isWalking", false);
        speedBoost = 1;

        if (endOfMovement) convoManager.NewConversation();
    }
}