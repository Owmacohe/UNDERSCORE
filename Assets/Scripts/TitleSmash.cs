﻿using System;
using System.Collections.Generic;
using Febucci.UI;
using TMPro;
using UnityEngine;

public class TitleSmash : MonoBehaviour
{
    [SerializeField] float waitBeforeShowing = 3;
    [TextArea(1, 100)]
    [SerializeField] string text;
    enum AnimationType { None, Wiggle, Shake }
    [SerializeField] AnimationType animType = AnimationType.Wiggle;
    [SerializeField] GameObject chapter, subtitle;
    
    TMP_Text txt;
    TextAnimator anim;
    AudioSource src;
    PlayerController player;
    
    List<string> words = new List<string>();
    int currentWord, numWords;
    string currentText;
    
    float waitTime;
    float amount;

    void Start()
    {
        txt = GetComponent<TMP_Text>();
        anim = GetComponent<TextAnimator>();
        src = GetComponent<AudioSource>();

        GameObject tempPlayer = GameObject.FindWithTag("Player");
        if (tempPlayer != null) player = tempPlayer.GetComponent<PlayerController>();

        if (text is "" or " ") text = txt.text;

        txt.text = text;
        txt.fontSize = 1200f / text.Length + 55;
        waitTime = 20f / text.Length;

        float scaleOffset = -0.346f * Camera.main.aspect + 1.588f;
        transform.localScale = Vector3.one * scaleOffset;

        amount = 3f / text.Length;
        float max = 0.1f;
        float min = 0.04f;
        if (amount > max) amount = max;
        if (amount < min) amount = min;

        txt.ForceMeshUpdate();

        foreach (TMP_LineInfo line in txt.textInfo.lineInfo)
        {
            string[] temp = txt.text.Substring(line.firstCharacterIndex, line.characterCount).Split(" ");
            
            foreach (string word in temp)
            {
                if (word is not "" or " ")
                {
                    words.Add(word);
                    numWords++;
                }
            }
        }

        txt.text = "";
        
        Invoke(nameof(Show), waitBeforeShowing);
    }

    void Show()
    {
        Invoke(nameof(NextLine), 1);
        chapter.SetActive(true);
        if (player != null) player.pauseMovement = true;
    }

    void NextLine()
    {
        if (currentWord == numWords)
        {
            Invoke(nameof(Hide), subtitle.GetComponent<TMP_Text>().text.Length * 0.075f);
        }
        else
        {
            if (currentWord > 0) currentText += " ";
            currentText += words[currentWord];
        
            if (!animType.Equals(AnimationType.None))
            {
                string type = animType.ToString().ToLower();
                anim.SetText("<" + type + " a=" + amount + ">" + currentText + "</" + type + ">", false);   
            }
            else
            {
                txt.text = currentText;
            }
            
            src.Play();

            currentWord++;
        
            if (currentWord <= numWords)
            {
                if (currentWord == numWords) subtitle.SetActive(true);
                
                Invoke(nameof(NextLine), currentWord < numWords ? waitTime : 3.5f);
            }
        }
    }

    void Hide()
    {
        gameObject.transform.parent.gameObject.SetActive(false);
        if (player != null) player.pauseMovement = false;
    }
}