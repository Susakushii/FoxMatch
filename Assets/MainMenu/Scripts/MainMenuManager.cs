using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private string levelName;
    [SerializeField] private GameObject panelPlay;
    [SerializeField] private GameObject panelAlbum;
    [SerializeField] private GameObject panelStore;
    [SerializeField] private GameObject panelOptions;
    
    public void Levels()
    {
        SceneManager.LoadScene(levelName);
    }

    public void Play()
    {
        panelPlay.SetActive(true);
        panelAlbum.SetActive(false);
        panelStore.SetActive(false);
        panelOptions.SetActive(false);
    }

    public void Album()
    {
        panelPlay.SetActive(false);
        panelStore.SetActive(false);
        panelOptions.SetActive(false);
        panelAlbum.SetActive(true);
    }

    public void Store()
    {
        panelPlay.SetActive(false);
        panelAlbum.SetActive(false);
        panelOptions.SetActive(false);
        panelStore.SetActive(true);
    }

    public void Options()
    {
        panelPlay.SetActive(false);
        panelAlbum.SetActive(false);
        panelStore.SetActive(false);
        panelOptions.SetActive(true);
    }

}
