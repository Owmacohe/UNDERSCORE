using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class NPCController : MonoBehaviour
{
    public class NPCInformation
    {
        public ConversationManager.Conversation conversation;
        public bool switchSceneOnEnd, completed, ignoreOrder;
        public string targetScene;

        public NPCInformation(ConversationManager.Conversation conv, bool ignore = false, bool switchScene = false, string target = "")
        {
            conversation = conv;
            ignoreOrder = ignore;
            switchSceneOnEnd = switchScene;
            targetScene = target;
        }
    }
    
    [SerializeField] float followSpeed = 4;

    [HideInInspector] public bool followPlayer;
    [HideInInspector] public NPCInformation info;
    [HideInInspector] public bool hasInteracted;
    Light l;

    Transform playerTransform;
    ConversationManager convoManager;
    Animator anim;

    void Start()
    {
        l = GetComponentInChildren<Light>();
        l.intensity = 0;

        playerTransform = GameObject.FindWithTag("Player").transform;
        convoManager = FindObjectOfType<ConversationManager>();
        anim = GetComponentInChildren<Animator>();
    }

    void FixedUpdate()
    {
        if (hasInteracted && l.intensity < 2)
        {
            Color.RGBToHSV(info.conversation.colour, out float h, out float _, out _);
            
            l.intensity += 0.01f;
            l.color = Color.HSVToRGB(h, l.intensity / 2f, 1);
                
            GetComponentInChildren<SkinnedMeshRenderer>().material.color =
                info.conversation.colour.Equals(Color.white) ? info.conversation.colour : Color.HSVToRGB(h, l.intensity / 10f, 1);
        }

        if (followPlayer && !convoManager.isInConversation && (info == null || !info.completed))
        {
            Vector3 pos = transform.position;
            Vector3 playerPos = playerTransform.position;
            float distance = Vector3.Distance(pos, playerPos);
            
            if (distance is <= 30 and >= 5)
            {
                if (!anim.GetBool("isWalking")) anim.SetBool("isWalking", true);

                Vector3 dir = (playerPos - pos).normalized;
                transform.position += dir * (followSpeed * 0.01f);
                
                transform.LookAt(playerTransform);
            }
            else
            {
                if (anim.GetBool("isWalking")) anim.SetBool("isWalking", false);
            }
        }
        else
        {
            if (anim.GetBool("isWalking")) anim.SetBool("isWalking", false);
        }
    }
}