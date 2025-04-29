using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class basicGrabCardMecanicForPC : MonoBehaviour
{
    public static bool algumaCartaSendoArrastada = false;

    [Header("Configurações de Arraste")]
    public bool voltarParaPosicaoInicial = false;
    public Vector2 offsetMouse = Vector2.zero;

    [Header("Restrições de Movimento")]
    public bool restringirX = false;
    public bool restringirY = false;

    [Header("Limites de Movimento")]
    public bool usarLimites = false;
    public Rect areaPermitida = new Rect(0, 0, 5, 5);

    [Header("Efeito Visual")]
    public Vector2 escalaArrastando = new Vector2(2f, 2f);
    public float tempoAnimacao = 0.2f;
    public float tempoDeEspera = 0.3f;

    [Header("Verificação de Slot")]
    public bool usarVerificacaoDeSlot = false;
    public string tagDoSlotCorreto = "SlotCorreto";

    private bool estaSendoArrastado = false;
    private bool travadoNoSlot = false;

    private Collider2D colliderDoObjeto;
    private Vector3 posicaoInicial;
    private Vector3 offset;
    private Vector3 escalaOriginal;

    private float tempoSegurando = 0f;

    void Start()
    {
        colliderDoObjeto = GetComponent<Collider2D>();
        posicaoInicial = transform.position;
        escalaOriginal = transform.localScale;
    }

    void Update()
    {
        if (travadoNoSlot) return;

        if (estaSendoArrastado && Input.GetMouseButton(0))
        {
            tempoSegurando += Time.deltaTime;
        }

        ProcessarMouse();
        AtualizarEscala();
    }

    void ProcessarMouse()
    {
        Vector2 posicaoMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (colliderDoObjeto.OverlapPoint(posicaoMouse))
            {
                estaSendoArrastado = true;
                algumaCartaSendoArrastada = true;
                offset = transform.position - (Vector3)posicaoMouse;
                tempoSegurando = 0f; // zera contador ao iniciar arraste
            }
        }

        if (estaSendoArrastado && Input.GetMouseButton(0))
        {
            Vector3 novaPosicao = posicaoMouse + (Vector2)offset;

            if (restringirX) novaPosicao.y = transform.position.y;
            if (restringirY) novaPosicao.x = transform.position.x;

            if (usarLimites)
            {
                novaPosicao.x = Mathf.Clamp(novaPosicao.x, areaPermitida.xMin, areaPermitida.xMax);
                novaPosicao.y = Mathf.Clamp(novaPosicao.y, areaPermitida.yMin, areaPermitida.yMax);
            }

            transform.position = novaPosicao;
        }

        if (estaSendoArrastado && Input.GetMouseButtonUp(0))
        {
            estaSendoArrastado = false;
            algumaCartaSendoArrastada = false;

            if (usarVerificacaoDeSlot)
            {
                Collider2D slot = Physics2D.OverlapPoint(transform.position);

                if (slot != null && slot.CompareTag(tagDoSlotCorreto))
                {
                    transform.position = slot.transform.position;
                    travadoNoSlot = true;
                    return;
                }
            }

            if (voltarParaPosicaoInicial)
            {
                transform.position = posicaoInicial;
            }
        }
    }

    void AtualizarEscala()
    {
        if (travadoNoSlot)
        {
            // Volta ao tamanho original instantaneamente e não faz mais nada
            transform.localScale = escalaOriginal;
            return;
        }

        Vector3 escalaAlvo = escalaOriginal;

        if (estaSendoArrastado && tempoSegurando >= tempoDeEspera)
        {
            escalaAlvo = new Vector3(escalaArrastando.x, escalaArrastando.y, escalaOriginal.z);
        }

        transform.localScale = Vector3.Lerp(transform.localScale, escalaAlvo, tempoAnimacao * Time.deltaTime * 10f);
    }

    public void ResetarPosicao()
    {
        transform.position = posicaoInicial;
        transform.localScale = escalaOriginal;
        travadoNoSlot = false;
    }

    void OnDrawGizmosSelected()
    {
        if (usarLimites)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(new Vector3(areaPermitida.x + areaPermitida.width / 2,
                                          areaPermitida.y + areaPermitida.height / 2, 0),
                               new Vector3(areaPermitida.width, areaPermitida.height, 1));
        }
    }
}
