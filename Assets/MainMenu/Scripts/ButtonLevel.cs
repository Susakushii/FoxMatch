using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class BotaoFase : MonoBehaviour
{
    public string nomeCenaFase;
    private Button botao;

    void Start()
    {
        botao = GetComponent<Button>();
        botao.onClick.AddListener(CarregarFase);
    }

    void CarregarFase()
    {
        SceneManager.LoadScene(nomeCenaFase);
    }
}
