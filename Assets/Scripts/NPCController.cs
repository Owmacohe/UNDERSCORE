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

    [HideInInspector] public NPCInformation info;
    [HideInInspector] public bool hasInteracted;
    Light l;

    void Start()
    {
        l = GetComponentInChildren<Light>();
        l.intensity = 0;
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
    }
}