using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 10;
    [SerializeField] float clickRadius = 2;
    [SerializeField] Image fade;

    [SerializeField] GameObject quitCanvas;

    [Header("Awakened")]
    [SerializeField] MeshRenderer rightEye;
    [SerializeField] MeshRenderer leftEye;
    [SerializeField] Material defaultEyesMaterial, awakenedEyesMaterial, ascendedMaterial;
    [SerializeField] GameObject rightSpotlight, leftSpotlight;

    float fadeSpeed;

    Animator anim;
    Rigidbody rb;
    Camera main;
    
    ConversationManager convoManager;
    TerrainManager terrManager;

    Vector3 target, direction;
    bool moving, clickedNPC, pointerOverUI;
    NPCController targetController;
    float speedBoost;

    GameObject[] allNPCs;

    [HideInInspector] public bool isFading, fadeIn;
    [HideInInspector] public bool pauseMovement;
    [HideInInspector] public float scope;

    void Start()
    {
        fade.enabled = true;

        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        main = Camera.main;

        ResetMovement(false);

        allNPCs = GameObject.FindGameObjectsWithTag("NPC");
        
        scope = Single.PositiveInfinity;
    }

    void Update()
    {
        pointerOverUI = EventSystem.current.IsPointerOverGameObject();
    }

    void OnFire()
    {
        if (!pauseMovement && !pointerOverUI)
        {
            Ray ray = main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit))
            {
                if (((hit.transform.parent != null &&
                     (hit.transform.parent.gameObject.CompareTag("Terrain") || hit.transform.parent.gameObject.CompareTag("NPC"))) ||
                    hit.transform.gameObject.CompareTag("Player")) && Vector3.Distance(Vector3.zero, hit.point) < scope)
                {
                    if (Vector3.Distance(transform.position, hit.point) > clickRadius &&
                        !hit.transform.gameObject.Equals(gameObject))
                    {
                        if (hit.transform.parent != null) clickedNPC = hit.transform.parent.gameObject.CompareTag("NPC");

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
                        
                        if (clickedNPC)
                        {
                            targetController = hit.transform.parent.GetComponent<NPCController>();
                            
                            if (Vector3.Distance(transform.position, hit.transform.position) <= clickRadius)
                            {
                                ResetMovement(true, targetController);
                                return;   
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
    }

    void OnToggleExit()
    {
        quitCanvas.SetActive(!quitCanvas.activeSelf);
    }

    void FixedUpdate()
    {
        if (!pauseMovement && moving)
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

        if (pauseMovement && moving) ResetMovement(false);

        if (isFading)
        {
            fade.color = new Color(0, 0, 0, fade.color.a + (fadeIn ? -0.01f : 0.0001f * fadeSpeed)); // TODO: better fade speed

            if ((fadeIn && fade.color.a <= 0.01) || (!fadeIn && fade.color.a >= 0.99)) isFading = false;
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

    public void FadeOut(float fadingSpeed)
    {
        isFading = true;
        fadeIn = false;
        fadeSpeed = fadingSpeed;
    }

    public void SwitchToAwakened()
    {
        rightEye.material = awakenedEyesMaterial;
        leftEye.material = awakenedEyesMaterial;

        rightSpotlight.SetActive(true);
        leftSpotlight.SetActive(true);
    }

    public void SwitchToAscended()
    {
        GetComponentInChildren<SkinnedMeshRenderer>().material = ascendedMaterial;
    }

    public void UpdateAfterSceneSwitch()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.Euler(Vector3.up * 180);
        
        convoManager = FindObjectOfType<ConversationManager>();
        terrManager = FindObjectOfType<TerrainManager>();
        
        isFading = true;
        fadeIn = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.parent != null && collision.transform.parent.gameObject.CompareTag("Terrain"))
            terrManager.GenerateAround(collision.transform.position);
        else if (collision.transform.parent != null && collision.transform.parent.gameObject.CompareTag("NPC"))
        {
            convoManager.bubblePosition = collision.transform.position;
            ResetMovement(true, collision.transform.parent.GetComponent<NPCController>());
        }
    }
}