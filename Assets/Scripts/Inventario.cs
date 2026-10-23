using UnityEngine;

public class Inventario : MonoBehaviour
{
    [Header("Sonidos")]
    public AudioSource audioSource;
    public AudioClip sonidoLLave;
    public bool tieneLlave = false;

    [SerializeField] private PuertaJefe puertaJefe;

    public void ObtenerLlave()
    {
        tieneLlave = true;
        audioSource.PlayOneShot(sonidoLLave);

        Debug.Log("Llave obtenida");

        if (puertaJefe != null)
        {
            puertaJefe.AbrirPuerta();
        }
    }
    private void Start()
    {
        
    }
    private void Update()
    {
        
    }
}