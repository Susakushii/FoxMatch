using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using JetBrains.Annotations;
using TMPro;
public class MenuController : MonoBehaviour
{
    [System.Serializable]
    public class BotaoMenu
    {
        public Button botao;
        public GameObject tela;
        public Sprite spriteNormal;
        public Sprite spriteSelecionado;
        public TextMeshProUGUI textoBotao;
    }

    [Header("Tela Inicial")]
    public GameObject telaStart;
    public Button botaoStart;

    [Header("Tela Menu Principal")]
    public GameObject menu;

    [Header("Botões e Telas do Menu")]
    public List<BotaoMenu> botoesMenu;
    private Button botaoAtualSelecionado;

    void Start()
    {
        // Mostra a tela inicial e oculta tudo do menu
        telaStart.SetActive(true);
        menu.SetActive(false);

        // Listener do botão start
        botaoStart.onClick.AddListener(IniciarMenu);

        foreach (var item in botoesMenu)
        {
            Button btn = item.botao;
            btn.onClick.AddListener(() => SelecionarAba(btn));
        }       
    }

    void IniciarMenu()
    {
        telaStart.SetActive(false); // Oculta a tela inicial
        menu.SetActive(true); // Ativa o MainMenu

        // Ativa as telas do MainMenu
        SelecionarAba(botoesMenu[0].botao);
    }

    void SelecionarAba(Button botaoSelecionado)
    {
        foreach (var item in botoesMenu)
        {
            bool ativo = (item.botao == botaoSelecionado);
            item.tela.SetActive(ativo);

            Image imagemBotao = item.botao.GetComponent<Image>();
            imagemBotao.sprite = ativo ? item.spriteSelecionado : item.spriteNormal;

            // Troca a cor do texto
            item.textoBotao.color = ativo ? Color.white : Color.black;
        
        }

        // Armazena o botão selecionado
        botaoAtualSelecionado = botaoSelecionado;
    }

}
