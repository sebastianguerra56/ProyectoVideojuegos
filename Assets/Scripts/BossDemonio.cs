using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
public class BossDemonio : MonoBehaviour
{
    [Header("Ataque")]
    [SerializeField] private int daño = 20;
    [SerializeField] private float distanciaAtaque = 2f;
    [SerializeField] private float tiempoEntreAtaques = 1.5f;
    [SerializeField] private float retrasoGolpe = 0.3f;
    private bool atacando = false;
    private float proximoAtaque = 0f;
    [Header("Estadísticas")]
    [SerializeField] private float velocidad = 2f;

    [Header("Jugador")]
    [SerializeField] private Transform player;
    [Header("Vida")]
    [SerializeField] private int vidaMaxima = 100;
    private int vidaActual;

    [Header("UI Boss")]
    [SerializeField] private Slider barraVida;
    [SerializeField] private TMP_Text textoNombreBoss;
    [SerializeField] private string nombreBoss = "Baby Demon";
    private Rigidbody2D rb;
    private Animator animator;

    private Vector2 direccion;
    private bool isMoving;
    private bool combateIniciado = false;
    private bool estaMuerto = false;
    [SerializeField] private PantallaVictoria pantallaVictoria;
    [Header("Fase 2 - Invocación")]
    [SerializeField] private GameObject enemigoInvocadoPrefab;
    [SerializeField] private Transform[] puntosInvocacion;
    [SerializeField] private int cantidadInvocados = 3;
    [SerializeField] private float tiempoInvocacion = 1.5f;
    private bool fase2Activada = false;
    private bool invocando = false;

    private void Start()
    {
        textoNombreBoss.text = nombreBoss;
        vidaActual = vidaMaxima;

        barraVida.maxValue = vidaMaxima;
        barraVida.value = vidaActual;

        // Oculta la barra hasta comenzar el combate
        barraVida.gameObject.SetActive(false);
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        GameObject jugador =
            GameObject.FindGameObjectWithTag("Player");

        if (jugador != null)
        {
            player = jugador.transform;
        }
    }

    private void Update()
    {
        if (!combateIniciado)
            return;

        if (player == null)
            return;

        direccion =
            (player.position - transform.position).normalized;

        animator.SetFloat("LookX", direccion.x);
        animator.SetFloat("LookY", direccion.y);

        animator.SetBool("isMoving", isMoving);
    }

    private void FixedUpdate()
    {
        if (!combateIniciado || estaMuerto)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        if (invocando)
        {
            rb.linearVelocity = Vector2.zero;
            isMoving = false;
            return;
        }

        if (player == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        float distancia = Vector2.Distance(
            rb.position,
            player.position
        );

        if (distancia <= distanciaAtaque)
        {
            rb.linearVelocity = Vector2.zero;
            isMoving = false;

            if (!atacando && Time.time >= proximoAtaque)
            {
                StartCoroutine(Atacar());
            }
        }
        else
        {
            if (!atacando)
            {
                isMoving = true;
                rb.linearVelocity = direccion * velocidad;
            }
            else
            {
                isMoving = false;
                rb.linearVelocity = Vector2.zero;
            }
        }
    }
    private IEnumerator Atacar()
    {
        atacando = true;
        isMoving = false;

        rb.linearVelocity = Vector2.zero;


        yield return new WaitForSeconds(retrasoGolpe);

        if (estaMuerto || !combateIniciado)
        {
            atacando = false;
            yield break;
        }
        if (invocando || estaMuerto)
        {
            atacando = false;
            yield break;
        }

        if (player == null)
        {
            Debug.LogError("2 - ERROR: Player es NULL");
            atacando = false;
            yield break;
        }

        float distanciaActual = Vector2.Distance(
            rb.position,
            player.position
        );

        if (distanciaActual <= distanciaAtaque)
        {

            VidaPlayer vidaPlayer =
                player.GetComponent<VidaPlayer>();

            if (vidaPlayer != null)
            {

                vidaPlayer.RecibirDaño(daño);

            }
            else
            {
                Debug.LogError(
                    "ERROR: No encontré VidaPlayer en el Player"
                );
            }
        }
        proximoAtaque =
            Time.time + tiempoEntreAtaques;

        atacando = false;
    }
    public void RecibirDaño(int cantidad)
    {
        if (estaMuerto)
            return;
        if (!combateIniciado)
            return;

        vidaActual -= cantidad;

        vidaActual = Mathf.Clamp(
            vidaActual,
            0,
            vidaMaxima
        );

        barraVida.value = vidaActual;

        if (!fase2Activada && vidaActual <= vidaMaxima / 2)
        {
            ActivarFase2();
        }

        if (vidaActual <= 0)
        {
            Morir();
        }
    }
    private void ActivarFase2()
    {
        fase2Activada = true;

        StartCoroutine(SecuenciaInvocacion());
    }
    private IEnumerator SecuenciaInvocacion()
    {
        invocando = true;

        // Detener al boss
        rb.linearVelocity = Vector2.zero;
        isMoving = false;

        Debug.Log("El Demonio comienza a invocar enemigos");

        // Más adelante:
        // animator.SetTrigger("Invocar");

        yield return new WaitForSeconds(tiempoInvocacion);

        // Si murió mientras invocaba, cancelamos
        if (estaMuerto)
        {
            yield break;
        }

        InvocarEnemigos();

        Debug.Log("El Demonio terminó la invocación");

        invocando = false;
    }
    private void InvocarEnemigos()
    {
        if (enemigoInvocadoPrefab == null)
        {
            Debug.LogWarning(
                "No hay prefab de enemigo asignado al Boss"
            );

            return;
        }

        if (puntosInvocacion == null ||
            puntosInvocacion.Length == 0)
        {
            Debug.LogWarning(
                "El Boss no tiene puntos de invocación"
            );

            return;
        }

        int cantidadReal = Mathf.Min(
            cantidadInvocados,
            puntosInvocacion.Length
        );

        for (int i = 0; i < cantidadReal; i++)
        {
            Instantiate(
                enemigoInvocadoPrefab,
                puntosInvocacion[i].position,
                Quaternion.identity
            );
        }
    }
    public void IniciarCombate()
    {
        if (combateIniciado)
            return;

        combateIniciado = true;

        barraVida.gameObject.SetActive(true);

        Debug.Log("¡Combate contra el Boss!");
    }

    private void Morir()
    {
        if (estaMuerto)
            return;

        estaMuerto = true;
        combateIniciado = false;
        atacando = false;
        isMoving = false;

        StopAllCoroutines();

        rb.linearVelocity = Vector2.zero;

        Collider2D colliderBoss = GetComponent<Collider2D>();

        if (colliderBoss != null)
        {
            colliderBoss.enabled = false;
        }

        if (barraVida != null)
        {
            barraVida.gameObject.SetActive(false);
        }

        animator.SetBool("isMoving", false);
        animator.SetTrigger("Muerto");

        Debug.Log("Boss derrotado");
    }
    public void FinMuerte()
    {
        if (pantallaVictoria != null)
        {
            pantallaVictoria.MostrarVictoria();
        }

        Destroy(gameObject);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            transform.position,
            distanciaAtaque
        );
    }
}