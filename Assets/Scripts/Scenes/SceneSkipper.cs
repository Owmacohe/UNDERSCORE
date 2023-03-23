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
        if (next.Equals("End")) Destroy(GameObject.FindWithTag("Player").transform.parent.gameObject);
        
        SceneManager.LoadScene(next);
    }
}