using System;
using UnityEngine;

public class PlayerUpdater : MonoBehaviour
{
    [SerializeField] bool awakenOnStart;
    
    void Start()
    {
        PlayerController temp = FindObjectOfType<PlayerController>();
        
        temp.UpdateAfterSceneSwitch();

        if (awakenOnStart)
        {
            temp.SwitchToAwakened();
        }
    }
}