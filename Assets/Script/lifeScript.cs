using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEditor.SearchService;

public class lifeScript : MonoBehaviour
{
    public Image[] vidas; // arraste os 3 coracoes aqui no inspetor
    public Sprite vidaNormal;
    public Sprite vidaPerdida;

    private int vidaAtual;

    void Start()
    {
        vidaAtual = vidas.Length;
    }

    public void PerderVida()
    {
        if (vidaAtual <= 1)
        {
            Debug.Log("O silencio venceu"); //substituir por tela de perdeu
        } else {
            vidaAtual--;
            shakeCameraScript.instancia.Tremer(0.1f, 0.3f); // intensidade e duração
            StartCoroutine(AnimarVidaPerdida(vidas[vidaAtual]));
            }
    }

    private IEnumerator AnimarVidaPerdida(Image imagemVida)
    {
        imagemVida.sprite = vidaPerdida;
        yield return new WaitForSeconds(0.5f); // tempo em vermelho
    }
}
