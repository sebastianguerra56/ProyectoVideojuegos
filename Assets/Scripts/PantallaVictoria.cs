using UnityEngine;
using UnityEngine.SceneManagement;

public class PantallaVictoria : MonoBehaviour
{
    [SerializeField] private GameObject panelVictoria;

    private void Start()
    {
        panelVictoria.SetActive(false);
    }

    public void MostrarVictoria()
    {
        panelVictoria.SetActive(true);

        // Pausamos el juego
        Time.timeScale = 0f;
    }

    public void ReiniciarNivel()
    {
        // Muy importante antes de recargar
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}