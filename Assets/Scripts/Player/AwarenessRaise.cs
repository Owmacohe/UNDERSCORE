using System;
using Febucci.UI;
using TMPro;
using UnityEngine;

public class AwarenessRaise : MonoBehaviour
{
    [SerializeField] AudioSource source;
    
    [Header("Text")]
    [SerializeField] TMP_Text text;
    [SerializeField] TextAnimator anim;
    [SerializeField] Color textColour;
    [SerializeField] float speed = 0.002f;

    bool isRaising, raisingUp;
    float start, last, current, target;

    void Start()
    {
        Reset();
    }

    void FixedUpdate()
    {
        if (isRaising)
        {
            if ((raisingUp && current >= target) || (!raisingUp && current <= target))
            {
                isRaising = false;
                current = target;
                Invoke(nameof(Reset), 3);
            }
            else
            {
                current += raisingUp ? speed : -speed;

                float round = Mathf.Round(current * 10);
                if ((raisingUp && round > last) || (!raisingUp && round < last)) source.Play();
                last = round;
            }

            float percentage = raisingUp ? current / target : target / current;
            
            text.color = new Color(textColour.r, textColour.g, textColour.b, percentage);
            anim.SetText("<shake a=" + percentage + ">" + Mathf.Round(current * 10) + "</shake>", false);
        }
    }

    public void Raise(float from, float to)
    {
        current = from;
        target = to;

        if (from < to) raisingUp = true;
        else raisingUp = false;

        isRaising = true;
    }

    void Reset()
    {
        text.color = new Color(textColour.r, textColour.g, textColour.b, 0);
        text.text = "";

        current = 0;
        target = 0;
    }
}