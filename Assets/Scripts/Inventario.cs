using UnityEngine;

public class Inventario : MonoBehaviour
{
    public bool tieneLlave = false;

    [SerializeField] private PuertaJefe puertaJefe;

    public void ObtenerLlave()
    {
        tieneLlave = true;

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