using System;
using UnityEngine;

public class Moon : MonoBehaviour
{
    [SerializeField] TerrainManager terrains;
    [SerializeField] Color colour;
    [SerializeField] TextAsset conversation;

    PlayerController player;

    void Start()
    {
        terrains.generateExtraNPC = true;
        terrains.extraNPCInfo = new NPCController.NPCInformation(
            new ConversationManager.Conversation(conversation, colour),
            true,
            true,
            "End"
        );
        
        player = FindObjectOfType<PlayerController>();
        Invoke(nameof(SetScope), 0.1f);
    }

    void SetScope()
    {
        player.scope = 35;
    }
}