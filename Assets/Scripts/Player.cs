using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("UI Cargador Sangre")]
    [SerializeField] private Image sangreCargador;
    [SerializeField] private GameObject cargadorNormal;
    [SerializeField] private GameObject imagenLanzaSangre;
    [SerializeField] private TMP_Text textoMunicion;
    public bool EstaEnDash => haciendoDash;
    private ControllerPlayer controles;
    private Vector2 inputMovimiento;
    [SerializeField] private float velocidad = 6f;
    private Rigidbody2D rb;
    private Animator animator;
    private bool isMoving;
    [Header("Disparo")]
    [SerializeField] private GameObject balaPrefab;
    [SerializeField] private GameObject lanzaPrefab;
    [SerializeField] private Transform puntoDisparo;
    [Header("Cadencia de disparo")]
    [SerializeField] private float tiempoEntreDisparos = 0.25f; 
    private float proximoDisparoDisponible;
    [SerializeField] private float velocidadBala = 12f;
    [Header("Munición normal")]
    [SerializeField] private int municionMaxima = 6; 
    [SerializeField] private float tiempoRecargaPorBala = 1f;
    private int municionActual; 
    private float temporizadorRecarga;
    [Header("Power-Up de lanza")]
    [SerializeField] private int usosLanza = 0;
    public bool TieneLanza => usosLanza > 0;
    [Header ("Sonidos")]
    public AudioSource audioSource;
    public AudioClip sonidoDisparo;
    public AudioClip sonidoPowerUp;


    [Header("Dash")]
    [SerializeField] private float velocidadDash = 16f;
    [SerializeField] private float duracionDash = 0.23f;
    [SerializeField] private float cooldownDash = 1f;

    private bool haciendoDash = false;
    private bool puedeHacerDash = true;
    private Vector2 direccionDash;

    private void Awake()
    {
        controles = new ControllerPlayer();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable() 
    { 
        controles.Player.Enable();
        controles.Player.Disparar.performed += ctx => Disparar();
    }
    private void OnDisable() 
    { 
        controles.Player.Disable();
        controles.Player.Disparar.performed -= ctx => Disparar();
    }
    private void ActualizarUIMunicion()
    {
        if (textoMunicion == null)
            return;

        if (usosLanza > 0)
        {
            textoMunicion.text = "" + usosLanza;
        }
        else
        {
            textoMunicion.text =
                "" + municionActual + "/" + municionMaxima;
        }
    }
    private void ActualizarCargadorSangre()
    {
        if (sangreCargador == null)
            return;

        if (municionMaxima <= 0)
        {
            sangreCargador.fillAmount = 0f;
            return;
        }

        sangreCargador.fillAmount =
            (float)municionActual / municionMaxima;
    }
    private void ActualizarVisualCargador()
    {
        bool lanzaActiva = usosLanza > 0;

        // Munición normal
        if (sangreCargador != null)
        {
            sangreCargador.gameObject.SetActive(!lanzaActiva);
        }

        if (cargadorNormal != null)
        {
            cargadorNormal.SetActive(!lanzaActiva);
        }

        // Power-up de lanza
        if (imagenLanzaSangre != null)
        {
            imagenLanzaSangre.SetActive(lanzaActiva);
        }
    }

    private void Disparar()
    {
        // 1. Comprobar cooldown entre disparos
        if (Time.time < proximoDisparoDisponible)
        {
            return;
        }

        // 2. Comprobar qué tipo de proyectil vamos a utilizar
        bool disparandoLanza = usosLanza > 0;

        // Si no tenemos lanza ni munición normal, no podemos disparar
        if (!disparandoLanza && municionActual <= 0)
        {
            Debug.Log("Sin munición, esperando recarga...");
            return;
        }

        // 3. Dirección hacia el mouse
        Vector2 mousePos = Mouse.current.position.ReadValue();

        Vector3 mouseWorld =
            Camera.main.ScreenToWorldPoint(mousePos);

        mouseWorld.z = 0f;

        Vector2 direccion =
            ((Vector2)mouseWorld - (Vector2)puntoDisparo.position).normalized;

        // 4. Rotación del proyectil
        float angle =
            Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;

        Quaternion rotacion =
            Quaternion.Euler(0f, 0f, angle);

        // 5. Elegir proyectil
        GameObject proyectilPrefab;

        if (disparandoLanza)
        {
            proyectilPrefab = lanzaPrefab;

            usosLanza--;

            ActualizarUIMunicion();
            ActualizarVisualCargador();
            Debug.Log(
                "Usos de lanza restantes: " + usosLanza
            );

            if (usosLanza == 0)
            {
                Debug.Log(
                    "Lanza agotada. Volviendo a bala normal."
                );
            }
        }
        else
        {
            proyectilPrefab = balaPrefab;

            // Consumir una bala normal
            municionActual--;

            // Reiniciar el temporizador de recarga
            temporizadorRecarga = 0f;
            ActualizarUIMunicion();
            ActualizarCargadorSangre();
        }

        // 6. Crear proyectil
        GameObject proyectil = Instantiate(
            proyectilPrefab,
            puntoDisparo.position,
            rotacion
        );

        Rigidbody2D rbProyectil =
            proyectil.GetComponent<Rigidbody2D>();

        if (rbProyectil != null)
        {
            rbProyectil.linearVelocity =
                direccion * velocidadBala;
        }

        // 7. Sonido
        audioSource.PlayOneShot(sonidoDisparo);

        // 8. Destruir proyectil después de 3 segundos
        Destroy(proyectil, 3f);

        // 9. Activar cooldown del disparo
        proximoDisparoDisponible =
            Time.time + tiempoEntreDisparos;
    }

    public void ObtenerLanza(int cantidadUsos)
    {
        usosLanza += cantidadUsos;

        audioSource.PlayOneShot(sonidoPowerUp);
        ActualizarUIMunicion();
        ActualizarVisualCargador();

        Debug.Log(
            "Lanza obtenida. Usos totales: " + usosLanza
        );
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        municionActual = municionMaxima; 
        temporizadorRecarga = 0f;
        ActualizarCargadorSangre();
        ActualizarUIMunicion();
        ActualizarVisualCargador();

    }

    // Update is called once per frame
    void Update()
    {
        RecargarMunicion();
        // Movimiento
        inputMovimiento = controles.Player.Mover.ReadValue<Vector2>();

        isMoving = inputMovimiento != Vector2.zero;

        animator.SetBool("isMove", isMoving);
        animator.SetFloat("Speed", inputMovimiento.sqrMagnitude);

        // Dirección hacia el mouse
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mousePos);
        mouseWorld.z = 0f;

        Vector2 direccion = (mouseWorld - transform.position).normalized;

        animator.SetFloat("LookX", direccion.x);
        animator.SetFloat("LookY", direccion.y);

        // DASH
        if (controles.Player.Esquivar.WasPressedThisFrame())
        {
            IniciarDash();
        }
    }
    private void RecargarMunicion()
    {
        if (municionActual >= municionMaxima)
        {
            temporizadorRecarga = 0f;
            return;
        }

        temporizadorRecarga += Time.deltaTime;

        if (temporizadorRecarga >= tiempoRecargaPorBala)
        {
            municionActual++;

            temporizadorRecarga -= tiempoRecargaPorBala;

            ActualizarUIMunicion();
            ActualizarCargadorSangre();
        }
    }
    private void IniciarDash()
    {
        if (!puedeHacerDash || haciendoDash)
            return;

        // El dash será en la dirección en la que te estás moviendo
        if (inputMovimiento != Vector2.zero)
        {
            direccionDash = inputMovimiento.normalized;

            StartCoroutine(Dash());
        }
    }
    private IEnumerator Dash()
    {
        puedeHacerDash = false;
        haciendoDash = true;

        animator.SetTrigger("Dash");

        yield return new WaitForSeconds(duracionDash);

        haciendoDash = false;

        yield return new WaitForSeconds(cooldownDash);

        puedeHacerDash = true;
    }
    private void FixedUpdate()
    {
        if (haciendoDash)
        {
            rb.linearVelocity = direccionDash * velocidadDash;
        }
        else
        {
            rb.linearVelocity = inputMovimiento.normalized * velocidad;
        }
    }

}

