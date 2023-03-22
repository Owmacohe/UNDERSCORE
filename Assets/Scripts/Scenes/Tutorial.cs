using System;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] TipManager tips;
    [SerializeField] TerrainManager terrains;
    [SerializeField] Color colour;
    [SerializeField] TextAsset conversation;

    int tutorialProgress = -1;

    void Start()
    {
        float initialWaitTime = 22;
        
        tips.ShowTip("Hey you", Vector3.forward * 10, 5, initialWaitTime);
        tips.ShowTip("Can you hear me?", Vector3.forward * 15 + Vector3.right * 20, 5, initialWaitTime + 2);
        tips.ShowTip("<b>[CLICK]</b> over here!", Vector3.right * -15, 7.5f, initialWaitTime + 5);
        tips.ShowTip("Keep coming!", Vector3.right * -20, 3, initialWaitTime + 15);
        
        Invoke(nameof(Increment), initialWaitTime + 15);
    }

    void Update()
    {
        if (terrains.currentTerrain.x < 0)
        {
            if (tutorialProgress == 0)
            {
                tips.ShowTip("You're almost there!", Vector3.right * -20, 3, 2);
                
                terrains.generateExtraNPC = true;
                terrains.extraNPCInfo = new NPCController.NPCInformation(
                    new ConversationManager.Conversation(conversation, colour),
                    true,
                    true,
                    "SignForest"
                );
                
                tutorialProgress++;
            }
        }
        
        if (tutorialProgress == 1 && terrains.lastExtraNpcTransform != null)
        {
            tips.ShowTip("<b>[CLICK]</b> here!", terrains.lastExtraNpcTransform.position, 5, 0, false);

            tutorialProgress++;
        }
    }

    void Increment()
    {
        tutorialProgress++;
    }
}