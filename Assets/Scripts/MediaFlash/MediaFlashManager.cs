using System;
using UnityEngine;

public class MediaFlashManager : MonoBehaviour
{
    [SerializeField, Range(0, 1)] public float flashChance = 0.001f;
    
    VideoFlash vidFlash;
    ImageFlash imgFlash;

    void Start()
    {
        vidFlash = GetComponentInChildren<VideoFlash>();
        imgFlash = GetComponentInChildren<ImageFlash>();

        vidFlash.flashChance = flashChance;
        imgFlash.flashChance = flashChance;
    }
}