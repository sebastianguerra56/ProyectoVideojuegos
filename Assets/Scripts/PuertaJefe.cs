using UnityEngine;

public class PuertaJefe : MonoBehaviour
{
    [SerializeField] private GameObject puertaIzquierdaCerrada;
    [SerializeField] private GameObject puertaDerechaCerrada;

    [SerializeField] private GameObject puertaIzquierdaAbierta;
    [SerializeField] private GameObject puertaDerechaAbierta;

    private void Start()
    {
        puertaIzquierdaAbierta.SetActive(false);
        puertaDerechaAbierta.SetActive(false);
    }

    public void AbrirPuerta()
    {
        puertaIzquierdaCerrada.SetActive(false);
        puertaDerechaCerrada.SetActive(false);

        puertaIzquierdaAbierta.SetActive(true);
        puertaDerechaAbierta.SetActive(true);

        GetComponent<BoxCollider2D>().enabled = false;

        Debug.Log("Puerta abierta");
    }
}