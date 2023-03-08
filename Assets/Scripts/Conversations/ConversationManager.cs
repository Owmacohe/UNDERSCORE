using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConversationManager : MonoBehaviour
{
    [SerializeField] GameObject speechBubble;
    [SerializeField] TextAsset[] conversationOrder;
    [SerializeField] Color[] colourOrder;
    [SerializeField] bool[] repeatableOrder;

    GameObject bubbles;
    [HideInInspector] public Vector3 bubblePosition;

    int currentConversation;
    bool isInConversation;
    NPCController.NPCInformation info;

    PlayerController player;
    
    public class Node
    {
        public string name;
        public string statement;
        public List<string> responses;

        public Node(string n, string s)
        {
            name = n;
            statement = s;
            responses = new List<string>();
        }
    }

    public class Conversation
    {
        public Color colour;
        public string current;
        public List<Node> nodes;

        public Conversation(TextAsset txt, Color col)
        {
            colour = col;
            current = "";
            nodes = new List<Node>();
            
            bool isResponses = false;
            string[] lines = txt.text.Split('\n');

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                
                if (!line.Equals(""))
                {
                    if (line.Equals("###"))
                    {
                        isResponses = true;
                    }
                    else
                    {
                        string[] names = line.Substring(1, lines[i].Length - 3).Split(',');
                        
                        string temp = "";

                        i++;

                        while (i < lines.Length && !lines[i].Trim().Equals("") && !lines[i].Trim()[0].Equals('['))
                        {
                            temp += (lines[i].Trim().Equals("<br>") ? "" : lines[i].Trim()) + "\n";
                            i++;
                        }
                        
                        foreach (string name in names)
                        {
                            if (!isResponses) nodes.Add(new Node(name, temp));
                            else
                            {
                                Node tempNode = FindNode(name);

                                if (tempNode == null) Debug.Log("No node with name: \"" + name + "\" found!");
                                else tempNode.responses.Add(temp);
                            }
                        }
                    }
                }
            }
        }

        public Node FindNode(string name)
        {
            foreach (Node i in nodes)
                if (i.name.Equals(name))
                    return i;

            return null;
        }
    }

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    public void NewConversation(NPCController controller)
    {
        // null && less
        // !null && repeatable
        
        if (!isInConversation &&
            (controller.info == null && currentConversation < conversationOrder.Length) ||
            (controller.info != null && controller.info.repeatable))
        {
            if (controller.info == null || !controller.info.repeatable)
            {
                Conversation temp = new Conversation(
                    conversationOrder[currentConversation],
                    colourOrder[currentConversation]
                );
            
                controller.info = new NPCController.NPCInformation(temp, repeatableOrder[currentConversation]);
                
                currentConversation++;
            }

            info = controller.info;
            info.conversation.current = "";

            bubbles = Instantiate(speechBubble, transform);
            bubbles.GetComponent<SpeechBubblesManager>().Generate(
                info.conversation.nodes[0].statement,
                info.conversation.nodes[0].responses.ToArray(),
                bubblePosition,
                info.conversation.colour);
            
            isInConversation = true;
            player.pauseMovement = true;

            controller.hasInteracted = true;

            WaitEndConversation(info.conversation.FindNode("START"));
        }
    }

    public void MakeChoice(int choice)
    {
        Destroy(bubbles);

        Node temp = info.conversation.FindNode(info.conversation.current + choice);

        if (temp != null)
        {
            bubbles = Instantiate(speechBubble, transform);
            bubbles.GetComponent<SpeechBubblesManager>().Generate(
                temp.statement,
                temp.responses.ToArray(),
                bubblePosition,
                info.conversation.colour);

            info.conversation.current += choice;
            
             WaitEndConversation(temp);
        }
        else
        {
            EndConversation();
        }
    }

    void WaitEndConversation(Node check)
    {
        if (check.responses == null || check.responses.Count == 0)
            Invoke(nameof(EndConversation), check.statement.Length * 0.05f);
    }

    void EndConversation()
    {
        if (bubbles != null) Destroy(bubbles);

        isInConversation = false;
        player.pauseMovement = false;

        info.completed = true;

        if (info.switchSceneOnEnd) SceneManager.LoadScene(info.targetScene);
    }
}