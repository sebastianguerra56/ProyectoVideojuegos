using UnityEngine;

public class TriggerBoss : MonoBehaviour
{
    [SerializeField] private BossDemonio boss;

    [Header("Bloqueo de la arena")]
    [SerializeField] private GameObject muroBoss;

    private bool activado = false;

    private void Start()
    {
        if (muroBoss != null)
        {
            muroBoss.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (activado)
            return;

        if (other.CompareTag("Player"))
        {
            activado = true;

            // Cerrar la salida
            if (muroBoss != null)
            {
                muroBoss.SetActive(true);
            }

            // Comenzar combate
            if (boss != null)
            {
                boss.IniciarCombate();
            }

            Debug.Log("Arena del Boss cerrada");
        }
    }
}