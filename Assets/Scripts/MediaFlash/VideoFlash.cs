using System;
using UnityEngine;
using UnityEngine.Video;
using Random = UnityEngine.Random;

public class VideoFlash : MonoBehaviour
{
    [SerializeField] VideoClip[] clips;
    [Range(0, 1)] public float flashChance = 0.001f;

    VideoPlayer vid;
    bool isPlaying;

    void Start()
    {
        vid = GetComponent<VideoPlayer>();
    }

    void FixedUpdate()
    {
        if (!isPlaying && Random.Range(0f, 1f) <= flashChance)
        {
            PlayRandomClip();
        }
        else if (isPlaying && !vid.isPlaying)
        {
            print("test");
            isPlaying = false;
            vid.clip = null;
        }
    }

    void PlayRandomClip()
    {
        VideoClip temp = clips[Random.Range(0, clips.Length)];
        vid.clip = temp;
        
        vid.Play();
        isPlaying = true;
    }
}