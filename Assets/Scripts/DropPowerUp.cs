using UnityEngine;
[System.Serializable]
public class DropPowerUp
{
    public string nombre;
    public GameObject prefab;

    [Range(0f, 100f)]
    public float peso = 1f;
}
