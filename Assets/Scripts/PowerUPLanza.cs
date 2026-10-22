using UnityEngine;

public class PowerUPLanza : MonoBehaviour
{
    [SerializeField] private int cantidadUsos = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.ObtenerLanza(cantidadUsos);

                Destroy(gameObject);
            }
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
