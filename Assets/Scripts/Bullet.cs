using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float lifeTime = 2f; // segundos antes de destruirse
    [SerializeField] private int daño = 1;      // daño que aplica

    private void Start()
    {
        // Destruye la bala automáticamente después de cierto tiempo
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Aquí puedes filtrar qué objetos reciben daño
        if (collision.CompareTag("Enemy"))
        {
            // Ejemplo: llamar a un método de daño en el enemigo
            collision.GetComponent<Enemy>().RecibirDaño(daño);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Wall"))
        {
            // Si choca contra un muro, también se destruye
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Boss"))
        {
            BossDemonio boss = collision.GetComponent<BossDemonio>();

            if (boss != null)
            {
                boss.RecibirDaño(daño);
            }

            Destroy(gameObject);
        }
    }
}
