using System;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public Color colour;
    public TextAsset conversation;

    void Start()
    {
        Color.RGBToHSV(colour, out float h, out float s, out _);
        
        GetComponentInChildren<Light>().color = Color.HSVToRGB(h, s, 1);
        GetComponentInChildren<SkinnedMeshRenderer>().material.color =
            colour.Equals(Color.white) ? colour : Color.HSVToRGB(h, 0.2f, 1);
    }
}