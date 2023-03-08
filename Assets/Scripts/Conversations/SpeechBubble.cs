using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour
{
    [SerializeField] float speed = 0.1f;
    [SerializeField] Sprite corner, triangle;
    
    string text;
    RectTransform offset;

    TMP_Text txt;
    int currentCharacter;
    Vector2 size;
    
    bool isResponse;
    int responseIndex;
    float rotationAmount, rotationOffset;

    Color bubbleColour;
    Image background, source;
    Image topRightCorner, topLeftCorner, bottomRightCorner, bottomLeftCorner;
    float cornerSize;
    Image top, bottom, right, left;

    public void Generate(
        string bubbleText,
        Color colour,
        bool response,
        Vector2 positionOffset,
        int index = -1,
        float rotAmount = 0,
        float rotOffset = 0)
    {
        offset = transform.GetChild(0).GetComponent<RectTransform>();
        
        isResponse = response;

        txt = GetComponentInChildren<TMP_Text>();
        bubbleColour = colour;
        cornerSize = txt.fontSize;
        
        text = bubbleText;
        
        background = CreateImage("Background");
        if (!isResponse) source = CreateImage("Source", triangle);
        
        topRightCorner = CreateImage("TopRightCorner", corner);
        topLeftCorner = CreateImage("TopLeftCorner", corner);
        bottomRightCorner = CreateImage("BottomRightCorner", corner);
        bottomLeftCorner = CreateImage("BottomLeftCorner", corner);
        
        top = CreateImage("Top");
        bottom = CreateImage("Bottom");
        right = CreateImage("Right");
        left = CreateImage("Left");

        GetComponent<RectTransform>().anchoredPosition = positionOffset;
        
        if (isResponse)
        {
            responseIndex = index;
            
            rotationAmount = rotAmount;
            rotationOffset = rotOffset;

            txt.GetComponent<RectTransform>().sizeDelta = new Vector2(250 / 3f, 150);

            GetComponentInChildren<Button>().enabled = true;
        }

        Type();
    }

    public void MakeChoice()
    {
        FindObjectOfType<ConversationManager>().MakeChoice(responseIndex);
    }

    void Type()
    {
        if (currentCharacter <= text.Length)
        {
            SetAll(text.Substring(0, currentCharacter++));
            Invoke(nameof(Type), speed);
        }
        else
        {
            if (!isResponse) FindObjectOfType<SpeechBubblesManager>().GenerateResponses();
        }
    }

    void SetAll(string bubbleText)
    {
        txt.text = bubbleText;
        Color.RGBToHSV(bubbleColour, out float h, out float s, out float v);
        txt.color = v >= 0.5f ? Color.HSVToRGB(h, s, 0.2f) : Color.HSVToRGB(h, s * 0.2f, 1);
        
        SetBubble();

        if (!isResponse)
        {
            RectTransform temp = source.GetComponent<RectTransform>();
            temp.sizeDelta = Vector2.one * (cornerSize * 1.5f);
            temp.anchoredPosition = Vector2.down * ((size.y / 2f) + (1.5f * cornerSize));
        }

        SetCorner(topRightCorner, true, true);
        SetCorner(topLeftCorner, true, false);
        SetCorner(bottomRightCorner, false, true);
        SetCorner(bottomLeftCorner, false, false);
        
        SetSide(top, true, true);
        SetSide(bottom, true, false);
        SetSide(right, false, true);
        SetSide(left, false, false);
        
        if (isResponse)
        {
            offset.anchoredPosition = Vector2.left * rotationOffset;
            
            RectTransform temp = GetComponent<RectTransform>();
            temp.localRotation = Quaternion.identity;
            temp.Rotate(Vector3.forward, rotationAmount);
            
            offset.rotation = Quaternion.identity;
        }
    }

    Image CreateImage(string name, Sprite spr = null)
    {
        GameObject temp = new GameObject(name);
        temp.transform.SetParent(offset);
        temp.transform.SetSiblingIndex(0);

        Image img = temp.AddComponent<Image>();
        img.color = bubbleColour;
        img.sprite = spr;
        
        temp.GetComponent<RectTransform>().localScale = Vector3.one;

        return img;
    }
    
    void SetBubble()
    {
        RectTransform temp = background.GetComponent<RectTransform>();

        float max = txt.GetComponent<RectTransform>().sizeDelta.x;
        float min = 50;
        float width = txt.preferredWidth > max ? max : (txt.preferredWidth < min ? min : txt.preferredWidth);

        size = new Vector2(
            width, 
            txt.preferredHeight);
        
        temp.sizeDelta = size;
        temp.anchoredPosition = Vector2.zero;
    }

    void SetCorner(Image img, bool top, bool right)
    {
        RectTransform temp = img.GetComponent<RectTransform>();
        temp.sizeDelta = Vector2.one * cornerSize;
        
        temp.localScale = new Vector3(
            right ? -1 : 1,
            top ? 1 : -1,
            1);
        
        img.GetComponent<RectTransform>().anchoredPosition = new Vector2(
            right ? (size.x / 2f) + (temp.sizeDelta.x / 2f) : -(size.x / 2f) - (temp.sizeDelta.x / 2f),
            top ? (size.y / 2f) + (temp.sizeDelta.y / 2f) : -(size.y / 2f) - (temp.sizeDelta.y / 2f));
    }

    void SetSide(Image img, bool vertical, bool first)
    {
        RectTransform temp = img.GetComponent<RectTransform>();
        
        temp.sizeDelta = new Vector2(
            vertical ? size.x : cornerSize,
            vertical ? cornerSize : size.y);

        temp.anchoredPosition = new Vector2(
            vertical
                ? 0
                : (first ? (size.x / 2f) + (cornerSize / 2f) : -(size.x / 2f) - (cornerSize / 2f)),
            vertical
                ? (first ? (size.y / 2f) + (cornerSize / 2f) : -(size.y / 2f) - (cornerSize / 2f))
                : 0);
    }
}