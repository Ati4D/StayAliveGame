using UnityEngine;
using UnityEngine.SceneManagement;

public class Router: MonoBehaviour
{
    public void PlayClick()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitToMenuClick()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitClick()
    {
        Application.Quit();
    }
}
