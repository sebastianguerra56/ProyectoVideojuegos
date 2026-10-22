using UnityEngine;

public class Llave : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Inventario inventario =
                other.GetComponent<Inventario>();

            if (inventario != null)
            {
                inventario.ObtenerLlave();
            }

            Destroy(gameObject);
        }
    }
}
