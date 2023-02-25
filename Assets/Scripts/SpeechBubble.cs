using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour
{
    [TextArea(1, 100)]
    [SerializeField] string text;
    [SerializeField] float speed = 0.075f;
    [SerializeField] Sprite corner, triangle;
    [SerializeField] bool generateOnStart;

    Transform offset;
    TMP_Text txt;
    int currentCharacter;
    Vector2 size;

    Image background, source;
    Image topRightCorner, topLeftCorner, bottomRightCorner, bottomLeftCorner;
    float cornerSize;
    Image top, bottom, right, left;

    Camera main;
    Vector3 hoverPosition;

    void Start()
    {
        if (generateOnStart)
        {
            Generate(text, Vector3.zero);
        }
    }

    void FixedUpdate()
    {
        if (!hoverPosition.Equals(Vector3.zero))
        {
            offset.position = main.WorldToScreenPoint(hoverPosition);
        }
    }

    public void Generate(string bubbleText, Vector3 hover)
    {
        offset = transform.GetChild(0);

        main = Camera.main;
        
        text = bubbleText;
        hoverPosition = hover + Vector3.up * 7.5f;
        
        txt = GetComponentInChildren<TMP_Text>();
        cornerSize = txt.fontSize;
        
        background = CreateImage("Background");
        source = CreateImage("Source", triangle);
        
        topRightCorner = CreateImage("TopRightCorner", corner);
        topLeftCorner = CreateImage("TopLeftCorner", corner);
        bottomRightCorner = CreateImage("BottomRightCorner", corner);
        bottomLeftCorner = CreateImage("BottomLeftCorner", corner);
        
        top = CreateImage("Top");
        bottom = CreateImage("Bottom");
        right = CreateImage("Right");
        left = CreateImage("Left");
        
        Type();
    }

    void Type()
    {
        if (currentCharacter <= text.Length)
        {
            SetAll(text.Substring(0, currentCharacter++));
            Invoke(nameof(Type), speed);
        }
    }

    void SetAll(string bubbleText)
    {
        txt.text = bubbleText;
        SetBubble();
        
        RectTransform temp = source.GetComponent<RectTransform>();
        temp.sizeDelta = Vector2.one * (cornerSize * 1.5f);
        temp.anchoredPosition = Vector2.down * ((size.y / 2f) + (1.5f * cornerSize));
        
        SetCorner(topRightCorner, true, true);
        SetCorner(topLeftCorner, true, false);
        SetCorner(bottomRightCorner, false, true);
        SetCorner(bottomLeftCorner, false, false);
        
        SetSide(top, true, true);
        SetSide(bottom, true, false);
        SetSide(right, false, true);
        SetSide(left, false, false);
    }

    Image CreateImage(string name, Sprite spr = null)
    {
        GameObject temp = new GameObject(name);
        temp.transform.SetParent(offset);
        temp.transform.SetSiblingIndex(0);

        Image img = temp.AddComponent<Image>();
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