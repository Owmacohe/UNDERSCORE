using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSkipper : MonoBehaviour
{
    [SerializeField] string next;
    
    void Start()
    {
        Invoke(nameof(Skip), 1);
    }

    void Skip()
    {
        SceneManager.LoadScene(next);
    }
}