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
    Color bubbleColour;
    string[] responseTexts;

    void Start()
    {
        main = Camera.main;

        if (generateOnStart)
        {
            Generate(text, responses, Vector3.zero, bubbleColour, true);
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

    public void Generate(
        string bubbleText,
        string[] responseText,
        Vector3 bubblePosition,
        Color colour,
        bool fromStart = false)
    {
        player = GameObject.FindWithTag("Player").transform;
        bubbleColour = colour;
        
        offset = transform.GetChild(0);
        responseOffset = transform.GetChild(1);

        if (!fromStart) hoverPosition = bubblePosition + Vector3.up * (7.5f + (0.0015f * bubbleText.Length));

        bubbles.Add(Instantiate(speechBubble, offset).GetComponent<SpeechBubble>());
        bubbles[0].Generate(bubbleText, bubbleColour, false, Vector2.zero);

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
                Color.white,
                true,
                Vector2.zero,
                i,
                interval * (i + 1),
                170);
        }
    }
}