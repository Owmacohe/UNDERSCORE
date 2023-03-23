using System;
using Febucci.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    [SerializeField] float wait = 2;
    [SerializeField] string text;
    [SerializeField] GameObject button;

    TextAnimatorPlayer player;

    void Start()
    {
        player = GetComponent<TextAnimatorPlayer>();

        Invoke(nameof(Show), wait);
    }

    void Show()
    {
        player.ShowText("<shake a=0.05>" + text + "</shake>");
        if (button != null) button.SetActive(true);
    }

    public void ChangeScene(string sceneName)
    {
        if (sceneName.Equals("Quit"))
        {
            Application.Quit(0);
        }
        else
        {
            SceneManager.LoadScene(sceneName);   
        }
    }
}