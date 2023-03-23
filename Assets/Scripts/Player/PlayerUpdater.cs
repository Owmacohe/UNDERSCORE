using System;
using UnityEngine;

public class PlayerUpdater : MonoBehaviour
{
    [SerializeField] bool awakenOnStart, ascendOnStart;
    
    void Start()
    {
        PlayerController temp = FindObjectOfType<PlayerController>();
        
        temp.UpdateAfterSceneSwitch();

        if (awakenOnStart) temp.SwitchToAwakened();
        else if (ascendOnStart) temp.SwitchToAscended();
    }
}