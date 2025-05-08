using UnityEngine;
using System.Collections;

public class shakeCameraScript : MonoBehaviour
{
    public static shakeCameraScript instancia; // acesso global
    private Vector3 posicaoOriginal;

    void Awake()
    {
        instancia = this;
        posicaoOriginal = transform.localPosition;
    }

    public void Tremer(float intensidade, float duracao)
    {
        StartCoroutine(Shake(intensidade, duracao));
    }

    IEnumerator Shake(float intensidade, float duracao)
    {
        float tempo = 0f;

        while (tempo < duracao)
        {
            float offsetX = Random.Range(-1f, 1f) * intensidade;
            float offsetY = Random.Range(-1f, 1f) * intensidade;

            transform.localPosition = posicaoOriginal + new Vector3(offsetX, offsetY, 0f);

            tempo += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = posicaoOriginal;
    }
}
