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

        public Node(string n, string s)
        {
            name = n;
            statement = s;
            responses = new List<string>();
            awarenessChange = new List<float>();
        }
    }

    public class Conversation
    {
        public Color colour;
        public string current;
        public List<Node> nodes;

        public Conversation(TextAsset txt, Color col, float awarenessAtCreation)
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
                                    if ((namesAwarenessRestriction.Length == 3 &&
                                         awarenessAtCreation >= float.Parse(namesAwarenessRestriction[2])) ||
                                        namesAwarenessRestriction.Length < 3)
                                    {
                                        tempNode.responses.Add(temp);
                                    
                                        tempNode.awarenessChange.Add(
                                            namesAwarenessRestriction.Length >= 2 ? float.Parse(namesAwarenessRestriction[1]) : 0);   
                                    }
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
                        colourOrder[currentConversation],
                        manager.awareness),
                    false,
                    currentConversation == conversationOrder.Length - 1,
                    nextScene
                );   
            }

            info = controller.info;

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
        if (!info.completed)
            manager.UpdateAwareness(info.conversation.FindNode(info.conversation.current).awarenessChange[choice]);

        if (info.conversation.current.Equals("START")) info.conversation.current = "";
        
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
        {
            Invoke(nameof(EndConversation), check.statement.Length * 0.08f);
            
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