using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 10;
    [SerializeField] float clickRadius = 2;
    [SerializeField] ConversationManager convoManager;
    [SerializeField] TerrainManager terrManager;
    [SerializeField] LayerMask UILayer;

    Animator anim;
    Rigidbody rb;
    Camera main;

    Vector3 target, direction;
    bool moving, clickedNPC, pointerOverUI;
    NPCController targetController;
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
                if ((hit.transform.parent != null && hit.transform.parent.gameObject.CompareTag("Terrain")) ||
                    hit.transform.gameObject.CompareTag("NPC") ||
                    hit.transform.gameObject.CompareTag("Player"))
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

                        if (clickedNPC)
                        {
                            target -= direction * clickRadius * 3;
                            targetController = hit.transform.GetComponent<NPCController>();
                        }
                    }
                    else
                    {
                        ResetMovement(false);

                        anim.SetTrigger("wiggle");
                    }   
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
                    ResetMovement(clickedNPC, targetController);
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

    void ResetMovement(bool startConvo, NPCController controller = null)
    {
        moving = false;
        anim.SetBool("isJogging", false);
        anim.SetBool("isWalking", false);
        speedBoost = 1;

        if (startConvo) convoManager.NewConversation(controller);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.parent != null && collision.transform.parent.gameObject.CompareTag("Terrain"))
        {
            terrManager.GenerateAround(collision.transform.position);
        }
        else
        {
            ResetMovement(clickedNPC && collision.gameObject.CompareTag("NPC"), targetController);
        }
    }
}