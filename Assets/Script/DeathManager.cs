using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathManager: MonoBehaviour {
    public void QuitGame()
    {
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene("Level 1");
    }
}
