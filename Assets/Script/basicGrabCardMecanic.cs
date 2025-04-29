using UnityEngine;


[RequireComponent(typeof(Collider2D))]
public class NewEmptyCSharpScript: MonoBehaviour 
{[Header("Configurações de Arraste")]
    [Tooltip("Se marcado, o objeto volta à posição inicial quando solto")]
    public bool voltarParaPosicaoInicial = false;
    
    [Tooltip("Se marcado, o objeto só se move no eixo X")]
    public bool restringirX = false;
    
    [Tooltip("Se marcado, o objeto só se move no eixo Y")]
    public bool restringirY = false;
    
    [Tooltip("Offset do dedo em relação ao centro do objeto")]
    public Vector2 offsetToque = Vector2.zero;

    [Header("Limites de Movimento")]
    public bool usarLimites = false;
    public Rect areaPermitida = new Rect(0, 0, 5, 5);

    private bool estaSendoArrastado = false;
    private Collider2D colliderDoObjeto;
    private Vector3 posicaoInicial;
    private Transform objetoPai;
    private int toqueId = -1;

    void Start()
    {
        colliderDoObjeto = GetComponent<Collider2D>();
        posicaoInicial = transform.position;
        objetoPai = transform.parent;
    }

    void Update()
    {
        ProcessarToques();
    }

    void ProcessarToques()
    {
        foreach (Touch toque in Input.touches)
        {
            Vector2 posicaoToque = Camera.main.ScreenToWorldPoint(toque.position);

            switch (toque.phase)
            {
                case TouchPhase.Began:
                    if (toqueId == -1 && colliderDoObjeto.OverlapPoint(posicaoToque))
                    {
                        toqueId = toque.fingerId;
                        estaSendoArrastado = true;
                        offsetToque = (Vector2)transform.position - posicaoToque;
                    }
                    break;

                case TouchPhase.Moved:
                    if (estaSendoArrastado && toque.fingerId == toqueId)
                    {
                        Vector2 novaPosicao = posicaoToque + offsetToque;
                        
                        if (restringirX) novaPosicao.y = transform.position.y;
                        if (restringirY) novaPosicao.x = transform.position.x;
                        
                        if (usarLimites)
                        {
                            novaPosicao.x = Mathf.Clamp(novaPosicao.x, areaPermitida.xMin, areaPermitida.xMax);
                            novaPosicao.y = Mathf.Clamp(novaPosicao.y, areaPermitida.yMin, areaPermitida.yMax);
                        }
                        
                        transform.position = novaPosicao;
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (toque.fingerId == toqueId)
                    {
                        estaSendoArrastado = false;
                        toqueId = -1;
                        
                        if (voltarParaPosicaoInicial)
                        {
                            transform.position = posicaoInicial;
                        }
                    }
                    break;
            }
        }
    }

    // Método para resetar programaticamente
    public void ResetarPosicao()
    {
        transform.position = posicaoInicial;
    }

    // Desenha a área permitida no Editor
    void OnDrawGizmosSelected()
    {
        if (usarLimites)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(new Vector3(areaPermitida.x + areaPermitida.width/2, 
                                          areaPermitida.y + areaPermitida.height/2, 0),
                               new Vector3(areaPermitida.width, areaPermitida.height, 1));
        }
    }
}

