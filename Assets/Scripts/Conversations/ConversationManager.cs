using System.Collections.Generic;
using UnityEngine;

public class ConversationManager : MonoBehaviour
{
    [SerializeField] GameObject speechBubble;
    [SerializeField] TextAsset conversationFile;

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
        public string current;
        public List<Node> nodes;

        public Conversation(TextAsset txt)
        {
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
                        string name = line.Substring(1, lines[i].Length - 2);

                        string temp = "";

                        i++;

                        while (i < lines.Length && !lines[i].Trim().Equals("") && !lines[i].Trim()[0].Equals('['))
                        {
                            temp += lines[i].Trim() + "\n";
                            i++;
                        }
                        
                        if (!isResponses) nodes.Add(new Node(name, temp));
                        else FindNode(name).responses.Add(temp);
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

    public void NewConversation()
    {
        if (!isInConversation)
        {
            conversation = new Conversation(conversationFile);

            bubbles = Instantiate(speechBubble, transform);
            bubbles.GetComponent<SpeechBubblesManager>().Generate(
                conversation.nodes[0].statement,
                conversation.nodes[0].responses.ToArray(),
                bubblePosition);
            
            isInConversation = true;
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
                bubblePosition);

            conversation.current += choice;
        }
        else
        {
            isInConversation = false;
        }
    }
}