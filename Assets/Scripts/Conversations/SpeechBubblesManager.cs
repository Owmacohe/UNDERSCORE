using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubblesManager : MonoBehaviour
{
    [SerializeField] GameObject speechBubble;
    
    List<SpeechBubble> bubbles = new List<SpeechBubble>();

    AwarenessManager awareness;
    
    Camera main;
    Transform offset, responseOffset, player;
    Vector3 hoverPosition;
    Color bubbleColour;
    string[] responseTexts;
    Color[] responseColours;
    float[] awarenessRequirements;

    [HideInInspector] public int lastHighlighted;
    float lastHighlightedTime;

    void Start()
    {
        main = Camera.main;
        awareness = FindObjectOfType<AwarenessManager>();
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
        Color[] respColours,
        float[] restRequirements)
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
        awarenessRequirements = restRequirements;
    }

    public void GenerateResponses()
    {
        float interval = 180f / (responseTexts.Length + 1);

        for (int i = 0; i < responseTexts.Length; i++)
        {
            if (awareness.awareness < awarenessRequirements[i])
            {
                string temp = "";

                for (int j = 0; j < responseTexts[i].Trim().Length; j++)
                {
                    char cha = responseTexts[i].Trim()[j];
                    temp += cha.Equals(' ') ? ' ' : (cha.Equals('.') ? '.' : '_');
                }

                responseTexts[i] = temp;
            }
            
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

        if (GamepadStatus.UsingGamepad)
            SetHighlight(0, true);
    }

    public void SetHighlight(int index, bool active)
    {
        lastHighlighted = index;
        lastHighlightedTime = Time.time;
        
        foreach (var i in bubbles[index + 1].GetComponentsInChildren<Outline>())
            i.enabled = active;
    }

    public void HighlightNext(bool right)
    {
        if (bubbles.Count > 1 && Time.time - lastHighlightedTime >= 0.3f)
        {
            if (right && lastHighlighted < bubbles.Count - 2)
            {
                SetHighlight(lastHighlighted, false);
                SetHighlight(lastHighlighted + 1, true);
                return;
            }

            if (!right && lastHighlighted > 0)
            {
                SetHighlight(lastHighlighted, false);
                SetHighlight(lastHighlighted - 1, true);
                return;
            }
        }
    }
}