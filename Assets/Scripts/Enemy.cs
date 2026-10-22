using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int vida = 3;
    [SerializeField] private float moveSpeed = 2f; 
    [SerializeField] private Transform player;
    private Rigidbody2D rb;
    private Animator anim;
    public AudioSource audioSource;
    public AudioClip sonidoDaño;
    [Header("Ataque")]
    [SerializeField] private int daño = 10;
    [SerializeField] private float radioAtaque = 1.5f;
    [SerializeField] private float tiempoEntreAtaques = 1f;
    [SerializeField] private LayerMask capaPlayer;
    [SerializeField] private Transform puntoAtaque;

    private bool isAttacking = false;
    private bool estaMuerto = false;
    private ControllerSala roomController;
    [SerializeField] private bool sueltaLlave;
    [SerializeField] private GameObject llavePrefab;
    [Header("Sistema de Drops")]
    [SerializeField] private bool puedeSoltarPowerUp = true;

    [Range(0f, 100f)]
    [SerializeField] private float probabilidadDeDrop = 20f;

    [SerializeField] private DropPowerUp[] powerUps;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null || isAttacking)
            return;
        AreaDeAtaque();

    }
    void FixedUpdate()
    {
        if (player != null)
        {
            
            if (isAttacking == true)
            {
                rb.linearVelocity = Vector2.zero;
            }
            else
            {
                Vector2 direccion = (player.position - transform.position).normalized;

                // Asignar velocidad directamente
                rb.linearVelocity = direccion * moveSpeed;
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
    public void RecibirDaño(int cantidad)
    {
        if (estaMuerto)
            return;

        vida -= cantidad;

        Debug.Log(
            "Enemigo recibió daño, vida restante: " + vida
        );

        if (anim != null)
        {
            anim.SetTrigger("Hurt");
        }

        if (audioSource != null && sonidoDaño != null)
        {
            audioSource.PlayOneShot(sonidoDaño);
        }

        if (vida <= 0)
        {
            Morir();
        }
    }
    private void AreaDeAtaque()
    {
        if (player == null || isAttacking || puntoAtaque == null)
            return;

        if (ObtenerJugadorEnRango() != null)
        {
            StopAllCoroutines();
            StartCoroutine(Atacar());
        }
    }
    public void SetRoom(ControllerSala room)
    {
        roomController = room;
    }


    private void Morir()
    {
        if (estaMuerto)
            return;

        estaMuerto = true;
        isAttacking = false;

        StopAllCoroutines();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        if (roomController != null)
        {
            roomController.EnemigoMuerto();
        }

        if (sueltaLlave)
        {
            if (llavePrefab != null)
            {
                Instantiate(
                    llavePrefab,
                    transform.position,
                    Quaternion.identity
                );
            }
        }
        else
        {
            IntentarSoltarPowerUp();
        }

        Destroy(gameObject);
    }
    private void IntentarSoltarPowerUp()
    {
        if (!puedeSoltarPowerUp || powerUps == null || powerUps.Length == 0)
            return;

        // ¿El enemigo suelta algo?
        float tiradaDrop = Random.Range(0f, 100f);

        if (tiradaDrop > probabilidadDeDrop)
            return;

        // Calcular peso total
        float pesoTotal = 0f;

        foreach (DropPowerUp powerUp in powerUps)
        {
            if (powerUp.prefab != null && powerUp.peso > 0f)
            {
                pesoTotal += powerUp.peso;
            }
        }

        if (pesoTotal <= 0f)
            return;

        // Elegir PowerUp
        float tiradaPowerUp = Random.Range(0f, pesoTotal);
        float pesoAcumulado = 0f;

        foreach (DropPowerUp powerUp in powerUps)
        {
            if (powerUp.prefab == null || powerUp.peso <= 0f)
                continue;

            pesoAcumulado += powerUp.peso;

            if (tiradaPowerUp <= pesoAcumulado)
            {
                Instantiate(
                    powerUp.prefab,
                    transform.position,
                    Quaternion.identity
                );

                Debug.Log("PowerUp obtenido: " + powerUp.nombre);

                return;
            }
        }
    }

    /* private void OnTriggerEnter2D(Collider2D other)
     {
         if (other.CompareTag("Player"))
         {
             VidaPlayer vidaplayer= other.GetComponent<VidaPlayer>();
             if (vidaplayer != null)
             {
                 vidaplayer.RecibirDaño(daño);
             }
         }
     }*/
    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            StartCoroutine(coldownAtaque());
            anim.SetTrigger("Attack");
            VidaPlayer vidaplayer = collision.transform.GetComponent<VidaPlayer>();
            if (vidaplayer != null)
            {
                vidaplayer.RecibirDaño(daño);
            }
        }

    }*/
    private IEnumerator Atacar()
    {
        isAttacking = true;
        rb.linearVelocity = Vector2.zero;

        if (anim != null)
        {
            anim.ResetTrigger("Attack");
            anim.SetTrigger("Attack");
        }

        // Espera antes de poder comenzar otro ataque.
        yield return new WaitForSeconds(tiempoEntreAtaques);

        if (!estaMuerto)
        {
            isAttacking = false;
        }
    }
    public void Golpear()
    {
        if (estaMuerto || puntoAtaque == null)
            return;

        VidaPlayer vidaPlayer = ObtenerJugadorEnRango();

        if (vidaPlayer == null)
        {
            Debug.Log("El jugador esquivó el ataque.");
            return;
        }

        vidaPlayer.RecibirDaño(daño);
        Debug.Log(
            "Enemigo golpeó al jugador. Daño: " + daño
        );
    }

    private VidaPlayer ObtenerJugadorEnRango()
    {
        if (puntoAtaque == null)
            return null;

        Collider2D[] objetivos = Physics2D.OverlapCircleAll(
            puntoAtaque.position,
            radioAtaque,
            capaPlayer
        );

        foreach (Collider2D objetivo in objetivos)
        {
            VidaPlayer vidaObjetivo = objetivo.GetComponentInParent<VidaPlayer>();

            if (vidaObjetivo == null)
                vidaObjetivo = objetivo.GetComponentInChildren<VidaPlayer>();

            if (vidaObjetivo != null)
                return vidaObjetivo;
        }

        return null;
    }

    private void OnDrawGizmosSelected()
    {
        if (puntoAtaque == null)
            return;

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            puntoAtaque.position,
            radioAtaque
        );
    }
}
