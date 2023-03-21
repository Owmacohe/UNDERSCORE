using System;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubblesManager : MonoBehaviour
{
    [SerializeField] GameObject speechBubble;
    
    List<SpeechBubble> bubbles = new List<SpeechBubble>();
    
    Camera main;
    Transform offset, responseOffset, player;
    Vector3 hoverPosition;
    Color bubbleColour;
    string[] responseTexts;
    Color[] responseColours;

    void Start()
    {
        main = Camera.main;
    }
    
    void FixedUpdate()
    {
        if (!hoverPosition.Equals(Vector3.zero))
        {
            offset.position = main.WorldToScreenPoint(hoverPosition);
            responseOffset.position = main.WorldToScreenPoint(player.position + Vector3.up * 7);
        }
    }

    public void Generate(
        string bubbleText,
        string[] responseText,
        Vector3 bubblePosition,
        Color colour,
        Color[] respColours)
    {
        player = GameObject.FindWithTag("Player").transform;
        bubbleColour = colour;
        
        offset = transform.GetChild(0);
        responseOffset = transform.GetChild(1);

        hoverPosition = bubblePosition + Vector3.up * (7.5f + (0.0015f * bubbleText.Length));

        bubbles.Add(Instantiate(speechBubble, offset).GetComponent<SpeechBubble>());
        bubbles[0].Generate(bubbleText, bubbleColour, false, Vector2.zero);

        responseTexts = responseText;
        responseColours = respColours;
    }

    public void GenerateResponses()
    {
        float interval = 180f / (responseTexts.Length + 1);

        for (int i = 0; i < responseTexts.Length; i++)
        {
            bubbles.Add(Instantiate(speechBubble, responseOffset).GetComponent<SpeechBubble>());
            bubbles[i+1].Generate(
                responseTexts[i],
                responseColours[i],
                true,
                Vector2.zero,
                i,
                interval * (i + 1),
                170);
        }
    }
}