using Unity.VisualScripting;
using UnityEngine;

public class PowerUpVida : MonoBehaviour
{

    [SerializeField] private int cantidadCuracion = 25;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        VidaPlayer vidaPlayer =
            other.GetComponent<VidaPlayer>();

        if (vidaPlayer != null)
        {
            bool seCuro =
                vidaPlayer.Curar(cantidadCuracion);
            // Solo desaparece si realmente curó
            if (seCuro)
            {
                Destroy(gameObject);
            }
        }
    }
}