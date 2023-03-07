using System;
using Febucci.UI;
using UnityEngine;

public class TipController : MonoBehaviour
{
    string txt;
    float waitTime;
    
    Camera main;
    Transform trans, playerTransform;
    Vector3 position;
    bool startOnPlayer;

    void Start()
    {
        main = Camera.main;
        
        trans = GetComponent<Transform>();
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        trans.position = main.WorldToScreenPoint(position);
    }

    public void Show(string text, Vector3 pos, float time, float wait, bool onPlayer)
    {
        position = pos;
        txt = text;
        waitTime = time;
        startOnPlayer = onPlayer;
        
        Invoke(nameof(ShowText), wait);
    }

    void ShowText()
    {
        if (startOnPlayer) position += playerTransform.position;
        
        GetComponent<TextAnimatorPlayer>().ShowText(txt);
        Invoke(nameof(Hide), waitTime);
    }

    void Hide()
    {
        Destroy(gameObject);
    }
}