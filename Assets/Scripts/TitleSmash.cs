using System;
using System.Collections.Generic;
using Febucci.UI;
using TMPro;
using UnityEngine;

public class TitleSmash : MonoBehaviour
{
    [TextArea(1, 100)]
    [SerializeField] string text;
    enum AnimationType { None, Wiggle, Shake }
    [SerializeField] AnimationType animType = AnimationType.Wiggle;
    
    TMP_Text txt;
    TextAnimator anim;
    AudioSource src;
    
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

        if (text is "" or " ") text = txt.text;

        txt.text = text;
        txt.fontSize = 1200f / text.Length + 55;
        waitTime = 20f / text.Length;

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
        
        Invoke(nameof(NextLine), 1);
    }

    void NextLine()
    {
        if (currentWord == numWords)
        {
            gameObject.SetActive(false);
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
                Invoke(nameof(NextLine), currentWord < numWords ? waitTime : 3.5f);
            }
        }
    }
}