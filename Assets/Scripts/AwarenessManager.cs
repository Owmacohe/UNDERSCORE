using System;
using UnityEngine;

public class AwarenessManager : MonoBehaviour
{
    /*
     * player spotlight size and brightness
     * NPC spotlight size and brightness
     * terrain POI density
     * camera shakiness
     * media frequency
     * tip shakiness
     * text distortion in TitleSmash or ConversationManager?
    */
    
    float awareness;
    
    public void UpdateAwareness(float a, bool verbose = false)
    {
        awareness += a;
        if (verbose) Debug.Log("Awareness: " + awareness);

        //playerLight.spotAngle = 70 + (25 * awareness);
    }
}