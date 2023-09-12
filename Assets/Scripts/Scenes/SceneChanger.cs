using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public static void Change(string sceneName)
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