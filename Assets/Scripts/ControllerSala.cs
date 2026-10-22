using UnityEngine;

public class ControllerSala : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject enemigoPrefab;
    [SerializeField] private int cantidadEnemigos = 5;
    [SerializeField] private GameObject guardianPrefab;
    [SerializeField] private bool tieneGuardian;
    private int enemigosRestantes;
    private bool salaIniciada;
    [Header("Bloqueo de sala")]
    [SerializeField] private GameObject muroEntrada;
    [Header("Progresión")]
    [SerializeField] private GameObject muroSiguienteSala;

    private void Start()
    {
        enemigosRestantes = cantidadEnemigos;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!salaIniciada && other.CompareTag("Player"))
        {
            salaIniciada = true;

            if (muroEntrada != null)
            {
                muroEntrada.SetActive(true);
            }

            SpawnEnemigo();

            Debug.Log("Sala iniciada");
        }
    }

    private void SpawnEnemigo()
    {
        Transform spawn =
            spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject prefabAInstanciar;

        // Si es el último enemigo y la sala tiene guardián
        if (tieneGuardian && enemigosRestantes == 1)
        {
            prefabAInstanciar = guardianPrefab;
        }
        else
        {
            prefabAInstanciar = enemigoPrefab;
        }

        GameObject enemigo =
            Instantiate(
                prefabAInstanciar,
                spawn.position,
                Quaternion.identity);

        Enemy enemy =
            enemigo.GetComponent<Enemy>();

        enemy.SetRoom(this);
    }

    public void EnemigoMuerto()
    {
        enemigosRestantes--;

        if (enemigosRestantes > 0)
        {
            SpawnEnemigo();
        }
        else
        {
            SalaCompletada();
        }
    }

    private void SalaCompletada()
    {
        Debug.Log("Sala completada");

        // Abrir la entrada de esta sala
        if (muroEntrada != null)
        {
            muroEntrada.SetActive(false);
        }

        // Desbloquear la siguiente sala
        if (muroSiguienteSala != null)
        {
            muroSiguienteSala.SetActive(false);
        }
    }
}