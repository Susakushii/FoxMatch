using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class FaseInfo
{
    public int numero;
    public Sprite imagemFase;
}
public class LevelsMap : MonoBehaviour
{

    public GameObject ButtonLevelPrefab;
    public Transform content;
    public List<FaseInfo> fases;
    //public int totalLevels = 30;

    void Start()
    {
        int lastLevelUnlock = PlayerPrefs.GetInt("LastLevelUnlock", 1);

        foreach (FaseInfo fase in fases)
        {
            GameObject button = Instantiate(ButtonLevelPrefab, content);

            // Numero
            button.transform.Find("NumberLevel").GetComponent<TextMeshProUGUI>().text = fase.numero.ToString();

            button.transform.Find("IconFase").GetComponent<Image>().sprite = fase.imagemFase;

            bool unlock = fase.numero <= lastLevelUnlock;
            Button btn = button.GetComponent<Button>();

            // Desbloqueada

            if (unlock)
            {
                button.transform.Find("Padlock").gameObject.SetActive(false);
                btn.interactable = true;
                int level = fase.numero; //captura Local para evitar problema com closures
                btn.onClick.AddListener(() => LoadLevel(level));
            }
            else
            {
                button.transform.Find("Padlock").gameObject.SetActive(true);
                btn.interactable = false;
            }
        }
    }

    void LoadLevel(int NumberLevel)
    {
        Debug.Log("Carregar fase" + NumberLevel);
        // Aqui pode chamar SceneManager.LoadScene() ou abrir a fase real
    }

    void Vitoria()
    {
        int faseAtual = 1;
        int faseSalva = PlayerPrefs.GetInt("LastLevelUnlock", 1);

        if (faseAtual >= faseSalva)
        {
            PlayerPrefs.SetInt("LastLevelUnlock", faseAtual + 1);
            PlayerPrefs.Save();
        }

        // Volta ao mapa ou tela de vítoria
    }
}
