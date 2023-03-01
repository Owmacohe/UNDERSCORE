using System;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubblesManager : MonoBehaviour
{
    [SerializeField] bool generateOnStart;
    
    [TextArea(1, 100)]
    [SerializeField] string text;
    [TextArea(1, 100)]
    [SerializeField] string[] responses;
    [SerializeField] GameObject speechBubble;
    
    List<SpeechBubble> bubbles = new List<SpeechBubble>();
    
    Camera main;
    Transform offset, responseOffset, player;
    Vector3 hoverPosition;
    string[] responseTexts;

    void Start()
    {
        main = Camera.main;

        if (generateOnStart)
        {
            Generate(text, responses, Vector3.zero, true);
        }
    }
    
    void FixedUpdate()
    {
        if (!hoverPosition.Equals(Vector3.zero))
        {
            offset.position = main.WorldToScreenPoint(hoverPosition);
            responseOffset.position = main.WorldToScreenPoint(player.position + Vector3.up * 7.5f);
        }
    }

    public void Generate(string bubbleText, string[] responseText, Vector3 bubblePosition, bool fromStart = false)
    {
        player = GameObject.FindWithTag("Player").transform;
        
        offset = transform.GetChild(0);
        responseOffset = transform.GetChild(1);

        if (!fromStart) hoverPosition = bubblePosition + Vector3.up * (7.5f + (0.0075f * bubbleText.Length));

        bubbles.Add(Instantiate(speechBubble, offset).GetComponent<SpeechBubble>());
        bubbles[0].Generate(bubbleText, false, Vector2.zero);

        responseTexts = responseText;
    }

    public void GenerateResponses()
    {
        float interval = 180f / (responseTexts.Length + 1);

        for (int i = 0; i < responseTexts.Length; i++)
        {
            bubbles.Add(Instantiate(speechBubble, responseOffset).GetComponent<SpeechBubble>());
            bubbles[i+1].Generate(
                responseTexts[i],
                true,
                Vector2.zero,
                i,
                interval * (i + 1),
                220);
        }
    }
}