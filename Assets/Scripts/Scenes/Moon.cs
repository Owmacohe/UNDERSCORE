using System;
using UnityEngine;

public class Moon : MonoBehaviour
{
    [SerializeField] TerrainManager terrains;
    [SerializeField] Color colour;
    [SerializeField] TextAsset conversation;

    void Start()
    {
        terrains.generateExtraNPC = true;
        terrains.extraNPCInfo = new NPCController.NPCInformation(
            new ConversationManager.Conversation(conversation, colour),
            true,
            true,
            "End"
        );
    }
}