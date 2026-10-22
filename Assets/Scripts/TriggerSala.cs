using UnityEngine;

public class TriggerSala : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Entraste a la sala");
        }
    }
}