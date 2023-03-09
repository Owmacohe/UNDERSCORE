using System;
using UnityEngine;

public class SignForest : MonoBehaviour
{
    [SerializeField] TipManager tips;
    [SerializeField] ConversationManager conversations;
    
    int current;

    void FixedUpdate()
    {
        if (conversations.currentConversation == current + 1 && !conversations.isInConversation)
        {
            switch (current)
            {
                case 0:
                    tips.ShowTip("How did that make you feel?", Vector3.forward * -8, 7.5f, 4);
                    tips.ShowTip("Did it help?", Vector3.forward * -3 + Vector3.right * 5, 3, 8);
                    tips.ShowTip("The comfort of acknowledgement is a strange one", Vector3.forward * 5, 10, 10);
                    break;
                case 1:
                    tips.ShowTip("You see now", Vector3.right * -8, 5, 4);
                    tips.ShowTip("It's all connected", Vector3.right * 8, 5, 6);
                    tips.ShowTip("There\'s will behind every action", Vector3.forward * 5, 7.5f, 10);
                    break;
                case 2:
                    tips.ShowTip("Keep moving.", Vector3.right * 5, 3, 4);
                    tips.ShowTip("Keep moving.", Vector3.right * 5 + Vector3.forward * -2, 3, 6);
                    tips.ShowTip("Keep moving.", Vector3.right * 5 + Vector3.forward * -4, 3, 8);
                    tips.ShowTip("Keep moving.", Vector3.right * 5 + Vector3.forward * -6, 3, 10);
                    tips.ShowTip("Keep moving.", Vector3.right * 5 + Vector3.forward * -8, 3, 12);
                    break;
            }
            
            current++;
        }

        if (conversations.currentConversation > current + 1) current = conversations.currentConversation - 1;
    }
}