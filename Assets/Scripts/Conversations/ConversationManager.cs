using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConversationManager : MonoBehaviour
{
    [SerializeField] GameObject speechBubble;
    [SerializeField] TextAsset[] conversationOrder;
    [SerializeField] Color[] colourOrder;
    [SerializeField] string nextScene;
    [SerializeField] Color awakenedResponseColour;
    [SerializeField] float responseOpacity = 0.45f;
    [SerializeField] float blockedResponseOpacity = 0.15f;

    GameObject bubbles;
    [HideInInspector] public Vector3 bubblePosition;

    [HideInInspector] public int currentConversation;
    [HideInInspector] public bool isInConversation;
    NPCController.NPCInformation info;

    PlayerController player;
    AwarenessManager manager;
    
    public class Node
    {
        public string name;
        public string statement;
        public List<string> responses;
        public List<float> awarenessChange;
        public List<float> awarenessRequirements;

        public Node(string n, string s)
        {
            name = n;
            statement = s;
            responses = new List<string>();
            awarenessChange = new List<float>();
            awarenessRequirements = new List<float>();
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
            current = "START";
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
                        string[] namesAwarenessRestriction = line
                            .Substring(1, lines[i].Trim().Length - 2)
                            .Split(':');
                        string[] names = namesAwarenessRestriction[0].Split(',');
                        
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

                                if (tempNode == null)
                                    Debug.Log("No node with name: \"" + name + "\" found!");
                                else
                                {
                                    tempNode.responses.Add(temp);
                                    
                                    tempNode.awarenessChange.Add(namesAwarenessRestriction.Length >= 2
                                        ? float.Parse(namesAwarenessRestriction[1])
                                        : 0);
                                    
                                    tempNode.awarenessRequirements.Add(namesAwarenessRestriction.Length == 3
                                        ? float.Parse(namesAwarenessRestriction[2])
                                        : 0);
                                }
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
        manager = GameObject.FindWithTag("Player").GetComponent<AwarenessManager>();
    }

    public void NewConversation(NPCController controller)
    {
        if (!isInConversation &&
            ((controller.info == null && (currentConversation < conversationOrder.Length || controller.info.ignoreOrder)) ||
             (controller.info != null && controller.info.ignoreOrder)))
        {
            if (controller.info == null)
            {
                controller.info = new NPCController.NPCInformation(
                    new Conversation(
                        conversationOrder[currentConversation],
                        colourOrder[currentConversation]),
                    false,
                    currentConversation == conversationOrder.Length - 1,
                    nextScene
                );   
            }

            info = controller.info;

            Color[] responseColours = new Color[info.conversation.nodes[0].responses.Count];

            for (int i = 0; i < responseColours.Length; i++)
                responseColours[i] = info.conversation.nodes[0].awarenessChange[i] > 0
                    ? new Color(
                        awakenedResponseColour.r,
                        awakenedResponseColour.g,
                        awakenedResponseColour.b,
                        manager.awareness >= info.conversation.nodes[0].awarenessRequirements[i]
                            ? responseOpacity
                            : blockedResponseOpacity)
                    : new Color(1, 1, 1, 
                        manager.awareness >= info.conversation.nodes[0].awarenessRequirements[i]
                            ? responseOpacity
                            : blockedResponseOpacity);

            bubbles = Instantiate(speechBubble, transform);
            bubbles.GetComponent<SpeechBubblesManager>().Generate(
                info.conversation.nodes[0].statement,
                info.conversation.nodes[0].responses.ToArray(),
                bubblePosition,
                info.conversation.colour,
                responseColours,
                info.conversation.nodes[0].awarenessRequirements.ToArray());
            
            isInConversation = true;
            player.pauseMovement = true;

            controller.hasInteracted = true;

            WaitEndConversation(info.conversation.FindNode("START"));
        }
    }

    public void MakeChoice(int choice)
    {
        Node current = info.conversation.FindNode(info.conversation.current);
        
        if (manager.awareness >= current.awarenessRequirements[choice])
        {
            if (!info.completed && current.awarenessChange[choice] != 0)
                manager.UpdateAwareness(current.awarenessChange[choice]);

            if (info.conversation.current.Equals("START")) info.conversation.current = "";
        
            Destroy(bubbles);
        
            Node temp = info.conversation.FindNode(info.conversation.current + choice);

            Color[] responseColours = new Color[temp.responses.Count];

            for (int i = 0; i < responseColours.Length; i++)
                responseColours[i] = temp.awarenessChange[i] > 0
                    ? new Color(
                        awakenedResponseColour.r,
                        awakenedResponseColour.g,
                        awakenedResponseColour.b,
                        manager.awareness >= temp.awarenessRequirements[i]
                            ? responseOpacity
                            : blockedResponseOpacity)
                    : new Color(1, 1, 1, 
                        manager.awareness >= temp.awarenessRequirements[i]
                            ? responseOpacity
                            : blockedResponseOpacity);

            if (temp != null)
            {
                bubbles = Instantiate(speechBubble, transform);
                bubbles.GetComponent<SpeechBubblesManager>().Generate(
                    temp.statement,
                    temp.responses.ToArray(),
                    bubblePosition,
                    info.conversation.colour,
                    responseColours,
                    temp.awarenessRequirements.ToArray());

                info.conversation.current += choice;
            
                WaitEndConversation(temp);
            }
            else
            {
                EndConversation();
            }   
        }
    }

    void WaitEndConversation(Node check)
    {
        if (check.responses == null || check.responses.Count == 0)
        {
            Invoke(nameof(EndConversation), check.statement.Length * 0.08f + 3);
            
            player.pauseMovement = false;
            
            if (info.switchSceneOnEnd) player.FadeOut(-(1231f/9640f) * check.statement.Length + (11224f/241f));
        }
    }

    void EndConversation()
    {
        //if (bubbles != null) Destroy(bubbles);

        isInConversation = false;

        info.completed = true;
        
        if (!info.ignoreOrder) currentConversation++;

        if (info.switchSceneOnEnd) SceneManager.LoadScene(info.targetScene);
    }
}