using UnityEngine;

public class TriggerBoss : MonoBehaviour
{
    [SerializeField] private BossDemonio boss;

    [Header("Bloqueo de la arena")]
    [SerializeField] private GameObject muroBoss;
    [Header("Música")]
    [SerializeField] private AudioSource musicaNivel;
    [SerializeField] private AudioSource musicaBoss;
    private bool activado = false;

    private void Start()
    {
        if (muroBoss != null)
        {
            muroBoss.SetActive(false);
        }
        if (musicaBoss != null)
        {
            musicaBoss.Stop();
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
            // Pausar música normal
            if (musicaNivel != null)
            {
                musicaNivel.Pause();
            }
            // Reproducir música del Boss
            if (musicaBoss != null)
            {
                musicaBoss.Play();
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