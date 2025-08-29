using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public void RetryGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1); // or a specific scene name
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
