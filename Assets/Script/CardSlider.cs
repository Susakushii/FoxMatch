using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class CardSlider : MonoBehaviour, IPointerUpHandler
{
    public Slider slider;
    public List<Transform> cards; // agora é List
    public Transform contentHolder;
    public float lerpSpeed = 5f;
    public float snapThreshold = 0.001f;

    public Vector3 targetWorldPosition = new Vector3(0.03f, 0.857f, 0);

    private List<float> cardSliderPositions;  // lista de posições
    private float targetSliderValue;
    private bool snapping = false;

    private int currentCenteredIndex = -1;
    public float centerScaleMultiplier = 1.2f; // escala aumentada no centro

    private List<Vector3> originalScales;  // escalas originais

    // Espaçamento entre cartas no eixo X (ajuste conforme seu layout)
    public float cardSpacingX = 2f;

    void Start()
    {
        RecalcularCards();
        ReorganizarCartas();
    }

    void Update()
    {
        // Remove cartas destruídas (referência nula) da lista
        for (int i = cards.Count - 1; i >= 0; i--)
        {
            if (cards[i] == null)
            {
                cards.RemoveAt(i);
                RecalcularCards();
                ReorganizarCartas();
                currentCenteredIndex = -1; // reset índice centralizado
            }
        }

        if (cards.Count == 0) return; // evita erros se não tiver cartas

        if (snapping)
        {
            slider.value = Mathf.Lerp(slider.value, targetSliderValue, Time.deltaTime * lerpSpeed);

            if (Mathf.Abs(slider.value - targetSliderValue) < snapThreshold)
            {
                slider.value = targetSliderValue;
                snapping = false;
            }
        }

        float t = Mathf.InverseLerp(0f, 1f, slider.value);
        Vector3 desiredCardPos = Vector3.Lerp(cards[0].position, cards[cards.Count - 1].position, t);

        float offsetX = desiredCardPos.x - targetWorldPosition.x;

        Vector3 newPosition = contentHolder.position;
        newPosition.x = Mathf.Lerp(contentHolder.position.x, contentHolder.position.x - offsetX, Time.deltaTime * lerpSpeed);
        contentHolder.position = newPosition;

        // Detecta qual carta está centralizada
        int nearestIndex = -1;
        float nearestDist = Mathf.Infinity;
        for (int i = 0; i < cards.Count; i++)
        {
            float dist = Mathf.Abs(cards[i].position.x - targetWorldPosition.x);
            if (dist < nearestDist)
            {
                nearestDist = dist;
                nearestIndex = i;
            }
        }

        // Se não achou carta válida (improvável)
        if (nearestIndex < 0 || nearestIndex >= cards.Count)
        {
            if (currentCenteredIndex >= 0 && currentCenteredIndex < cards.Count)
                cards[currentCenteredIndex].localScale = originalScales[currentCenteredIndex];

            currentCenteredIndex = -1;
            return;
        }

        // Ajusta escala das cartas
        if (nearestIndex != currentCenteredIndex)
        {
            if (currentCenteredIndex >= 0 && currentCenteredIndex < cards.Count)
                cards[currentCenteredIndex].localScale = originalScales[currentCenteredIndex];

            cards[nearestIndex].localScale = originalScales[nearestIndex] * centerScaleMultiplier;
            currentCenteredIndex = nearestIndex;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (cards.Count == 0) return;

        float closestDist = Mathf.Infinity;
        float closestValue = slider.value;

        foreach (float pos in cardSliderPositions)
        {
            float dist = Mathf.Abs(slider.value - pos);
            if (dist < closestDist)
            {
                closestDist = dist;
                closestValue = pos;
            }
        }

        targetSliderValue = closestValue;
        snapping = true;
    }

    void RecalcularCards()
    {
        cardSliderPositions = new List<float>();
        originalScales = new List<Vector3>();

        int total = cards.Count;

        for (int i = 0; i < total; i++)
        {
            cardSliderPositions.Add(total == 1 ? 0f : (float)i / (total - 1));
            originalScales.Add(cards[i].localScale);
        }
    }

    void ReorganizarCartas()
    {
        if (cards.Count == 0) return;

        float totalWidth = cardSpacingX * (cards.Count - 1);
        float startX = cards[0].position.x - totalWidth / 2f;

        for (int i = 0; i < cards.Count; i++)
        {
            Transform card = cards[i];

            // Reposiciona horizontalmente
            Vector3 novaPos = card.position;
            novaPos.x = startX + cardSpacingX * i;
            card.position = novaPos;

            // Atualiza a posição inicial no script de drag (evita voltar pro lugar antigo)
            basicGrabCardMecanicForPC drag = card.GetComponent<basicGrabCardMecanicForPC>();
            if (drag != null)
            {
                drag.AtualizarPosicaoInicial(card.position);
            }
        }
    }

}
