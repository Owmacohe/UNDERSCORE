using System;
using Febucci.UI;
using UnityEngine;

public class Title : MonoBehaviour
{
    [SerializeField] float wait = 2;
    [SerializeField] string text;
    [SerializeField] GameObject button;
    [SerializeField] string changeTo;

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
    
    void OnFire()
    {
        GamepadStatus.UsingGamepad = false;
    }

    void OnSelect()
    {
        GamepadStatus.UsingGamepad = true;
        ChangeScene(changeTo);
    }

    void OnWalk()
    {
        GamepadStatus.UsingGamepad = true;
    }

    public void ChangeScene(string sceneName)
    {
        SceneChanger.Change(sceneName);
    }
}