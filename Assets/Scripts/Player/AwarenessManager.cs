using System;
using UnityEngine;

public class AwarenessManager : MonoBehaviour
{
    [SerializeField] float changeSpeed = 0.02f;
    [SerializeField] Light playerLight;
    [SerializeField] Camera playerCamera;
    
    /*
     * player spotlight size and brightness
     * NPC spotlight size and brightness
     * terrain POI density
     * camera shakiness
     * media frequency
     * tip shakiness
     * text distortion in TitleSmash or ConversationManager?
    */
    
    [HideInInspector] public float awareness;

    bool upPlayerLightAngle;
    float playerLightAngle;

    bool upPlayerCameraFOV;
    float playerCameraFOV;

    void Start()
    {
        playerLightAngle = playerLight.spotAngle;
        playerCameraFOV = playerCamera.fieldOfView;
    }

    public void UpdateAwareness(float amount)
    {
        awareness += amount;

        if (awareness < -2) awareness = -2;
        if (awareness > 3) awareness = 3;

        Debug.Log("Awareness: " + awareness);

        playerLightAngle = 70 + (25 * awareness);
        playerCameraFOV = 60 + (10 * awareness);

        upPlayerLightAngle = playerLight.spotAngle < playerLightAngle;
        upPlayerCameraFOV = playerCamera.fieldOfView < playerCameraFOV;

        ChangePlayerLightAngle();
        ChangePlayerCameraFOV();
    }

    void ChangePlayerLightAngle()
    {
        if ((upPlayerLightAngle && playerLight.spotAngle < playerLightAngle) ||
            (!upPlayerLightAngle && playerLight.spotAngle > playerLightAngle))
        {
            playerLight.spotAngle += upPlayerLightAngle ? changeSpeed : -changeSpeed;
            
            Invoke(nameof(ChangePlayerLightAngle), 0.01f);
        }
    }
    
    void ChangePlayerCameraFOV()
    {
        if ((upPlayerCameraFOV && playerCamera.fieldOfView < playerCameraFOV) ||
            (!upPlayerCameraFOV && playerCamera.fieldOfView > playerCameraFOV))
        {
            playerCamera.fieldOfView += upPlayerCameraFOV ? changeSpeed : -changeSpeed;
            
            Invoke(nameof(ChangePlayerCameraFOV), 0.01f);
        }
    }
}