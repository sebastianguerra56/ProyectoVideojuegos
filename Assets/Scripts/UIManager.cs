using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : MonoBehaviour
{
    public Button btnCanvas;
    public Image imgCanvas;
    public TextMeshProUGUI txtCanvas, txtContador;
    public int contadorClick;

    public string MensajeAMostrar;
    public Sprite[] sprites;
    public void contarClick()
    {
        contadorClick++;
        txtContador.text = contadorClick.ToString();
    }
    public void CambiarTexto()
    {
        txtCanvas.text = MensajeAMostrar;
    }
    public void CambiarImagenAleatoria()
    {
        imgCanvas.sprite = sprites[Random.Range(0, sprites.Length)]; 
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CambiarTexto();
        CambiarImagenAleatoria();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
