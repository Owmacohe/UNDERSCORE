using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class NPCController : MonoBehaviour
{
    [SerializeField] Color[] colourRange = new Color[2];
    
    public class NPCInformation
    {
        public Color colour;
        public TextAsset conversation;
        public bool switchSceneOnEnd, hasCompleted;
        public string targetScene;

        public NPCInformation(Color col, TextAsset conv, bool switchScene = false, string target = "")
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
        if (info == null)
        {
            Color temp = Color.Lerp(colourRange[0], colourRange[1], Random.Range(0f, 1f));
            Color.RGBToHSV(temp, out var tempH, out var tempS, out _);
            temp = Color.HSVToRGB(tempH, tempS, Random.Range(0.3f, 0.8f));
            
            info = new NPCInformation(temp, null);
        }
        
        Color.RGBToHSV(info.colour, out float h, out float s, out _);
     
        l = GetComponentInChildren<Light>();
        l.color = Color.HSVToRGB(h, s, 1);
        l.intensity = 0;

        GetComponentInChildren<SkinnedMeshRenderer>().material.color =
            info.colour.Equals(Color.white) ? info.colour : Color.HSVToRGB(h, 0.2f, 1);
    }

    void FixedUpdate()
    {
        if (l.intensity < 2)
        {
            l.intensity += 0.01f;
        }
    }
}