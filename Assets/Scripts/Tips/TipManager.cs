using System;
using TMPro;
using UnityEngine;

public class TipManager : MonoBehaviour
{
    [SerializeField] GameObject tip;

    public void ShowTip(string text, Vector3 pos, float time, float wait = 0, bool onPlayer = true)
    {
        Instantiate(tip, transform).GetComponent<TipController>().Show(text, pos, time, wait, onPlayer);
    }
}