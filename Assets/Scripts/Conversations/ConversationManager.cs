using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConversationManager : MonoBehaviour
{
    [SerializeField] GameObject speechBubble;

    GameObject bubbles;
    [HideInInspector] public Vector3 bubblePosition;

    bool isInConversation;
    Conversation conversation;
    
    public class Node
    {
        public string name;
        public string statement;
        public List<string> responses;
        public List<string> targets;

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
        public bool switchSceneOnEnd;
        public string targetScene;

        public Conversation(TextAsset txt, Color col, bool switchScene, string target)
        {
            colour = col;
            current = "";
            nodes = new List<Node>();
            switchSceneOnEnd = switchScene;
            targetScene = target;
            
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
                                print(name);
                                FindNode(name).responses.Add(temp);
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

    public void NewConversation(NPCController controller)
    {
        if (!isInConversation && controller.info != null && controller.info.conversation != null)
        {
            NPCController.NPCInformation info = controller.info;
            
            conversation = new Conversation(
                info.conversation,
                info.colour,
                info.switchSceneOnEnd,
                info.targetScene
            );

            bubbles = Instantiate(speechBubble, transform);
            bubbles.GetComponent<SpeechBubblesManager>().Generate(
                conversation.nodes[0].statement,
                conversation.nodes[0].responses.ToArray(),
                bubblePosition,
                conversation.colour);
            
            isInConversation = true;
            
            print(conversation.nodes[0].name);
            
            WaitEndConversation(conversation.FindNode("START"));
        }
    }

    public void MakeChoice(int choice)
    {
        Destroy(bubbles);

        Node temp = conversation.FindNode(conversation.current + choice);

        if (temp != null)
        {
            bubbles = Instantiate(speechBubble, transform);
            bubbles.GetComponent<SpeechBubblesManager>().Generate(
                temp.statement,
                temp.responses.ToArray(),
                bubblePosition,
                conversation.colour);

            conversation.current += choice;
            
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
            Invoke(nameof(EndConversation), check.statement.Length * 0.13f);
    }

    void EndConversation()
    {
        if (bubbles != null) Destroy(bubbles);

        isInConversation = false;

        if (conversation.switchSceneOnEnd)
        {
            SceneManager.LoadScene(conversation.targetScene);
        }

        conversation = null;
    }
}