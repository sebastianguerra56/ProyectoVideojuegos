using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class VidaPlayer : MonoBehaviour
{
    [SerializeField] private int vida = 100;
    private int vidaActual;
    [SerializeField] private Slider barraVida;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Color colorDaño = Color.red;
    [SerializeField] private float duracionColorDaño = 0.2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vidaActual = vida;

        barraVida.maxValue = vida;
        barraVida.value = vidaActual;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void RecibirDaño(int cantidad)
    {
        vidaActual -= cantidad;

        barraVida.value = vidaActual;

        Debug.Log("Jugador recibió daño " + vidaActual);
        StartCoroutine(EfectoDaño());

        if (vidaActual <= 0)
        {
            Morir();
        }
    }
    public void Morir()
    {
        Debug.Log("Jugador Muerto");

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private IEnumerator EfectoDaño()
    {
        // Cambiar sprite a rojo
        spriteRenderer.color = colorDaño;

        yield return new WaitForSeconds(duracionColorDaño);

        // Volver al color normal
        spriteRenderer.color = Color.white;
    }
    public bool Curar(int cantidad)
    {
        // No recoger curación si la vida ya está completa
        if (vidaActual >= vida)
        {
            Debug.Log("La vida ya está completa");
            return false;
        }

        vidaActual += cantidad;

        // Nunca superar la vida máxima
        vidaActual = Mathf.Clamp(
            vidaActual,
            0,
            vida
        );

        // Actualizar barra de vida
        if (barraVida != null)
        {
            barraVida.value = vidaActual;
        }

        Debug.Log(
            "Jugador curado. Vida actual: "
            + vidaActual
            + " / "
            + vida
        );

        return true;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
