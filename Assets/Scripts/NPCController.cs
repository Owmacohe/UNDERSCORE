using System;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public class NPCInformation
    {
        public Color colour;
        public TextAsset conversation;
        public bool switchSceneOnEnd;
        public string targetScene;

        public NPCInformation()
        {
            colour = Color.white;
        }

        public NPCInformation(Color col, TextAsset conv, bool switchScene, string target)
        {
            colour = col;
            conversation = conv;
            switchSceneOnEnd = switchScene;
            targetScene = target;
        }
    }

    [HideInInspector] public NPCInformation info;
    Light l;

    void Start()
    {
        Color temp = info == null ? Color.white : info.colour;
        
        Color.RGBToHSV(temp, out float h, out float s, out _);
     
        l = GetComponentInChildren<Light>();
        l.color = Color.HSVToRGB(h, s, 1);
        l.intensity = 0;

        GetComponentInChildren<SkinnedMeshRenderer>().material.color =
            temp.Equals(Color.white) ? temp : Color.HSVToRGB(h, 0.2f, 1);
    }

    void FixedUpdate()
    {
        if (l.intensity < 2)
        {
            l.intensity += 0.01f;
        }
    }
}