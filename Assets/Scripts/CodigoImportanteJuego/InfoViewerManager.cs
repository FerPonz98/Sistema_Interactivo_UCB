using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class InfoViewerManager : MonoBehaviour
{
    [System.Serializable]
    public class InfoItem
    {
        public string nombre;
        public Sprite imagen;
        public GameObject prefab;
        [TextArea(2, 5)] public string descripcion;
    }

    [Header("Base de datos de válvulas")]
    public List<InfoItem> items = new List<InfoItem>();

    [Header("Elementos de UI")]
    public Image imagenDisplay;
    public TextMeshProUGUI textoNombre;
    public TextMeshProUGUI textoDescripcion;
    public TextMeshProUGUI contadorText;
    public Button botonSiguiente;
    public Button botonAnterior;
    public Button botonGenerar;
    public Button botonInfo;

    [Header("Respawn")]
    public Transform puntoDeRespawn;

    private int indiceActual = 0;
    private bool infoVisible = true;
    private GameObject objetoInstanciadoActual;

    private Image imagenBotonInfo;


    void Start()
    {
        botonSiguiente.onClick.AddListener(MostrarSiguiente);
        botonAnterior.onClick.AddListener(MostrarAnterior);
        botonGenerar.onClick.AddListener(GenerarObjeto);
        botonInfo.onClick.AddListener(ToggleInformacion);

        imagenBotonInfo = botonInfo.GetComponent<Image>();
        infoVisible = false;
        textoDescripcion.gameObject.SetActive(false);
        ActualizarVista();

    }

    void ActualizarVista()
    {
        if (items.Count == 0) return;

        InfoItem item = items[indiceActual];

        imagenDisplay.sprite = item.imagen;
        textoNombre.text = item.nombre;
        textoDescripcion.text = item.descripcion;
        textoDescripcion.gameObject.SetActive(infoVisible);

        // Mostrar el botón solo si hay prefab asignado
        botonGenerar.gameObject.SetActive(item.prefab != null);

        // Mostrar el contador (por ejemplo: "5 de 27")
        contadorText.text = $"Válvula {indiceActual + 1} de {items.Count}";


    }


    void MostrarSiguiente()
    {
        indiceActual = (indiceActual + 1) % items.Count;
        ActualizarVista();
    }

    void MostrarAnterior()
    {
        indiceActual = (indiceActual - 1 + items.Count) % items.Count;
        ActualizarVista();
    }

    void GenerarObjeto()
    {
        if (objetoInstanciadoActual != null)
            Destroy(objetoInstanciadoActual);

        InfoItem item = items[indiceActual];

        if (item.prefab != null && puntoDeRespawn != null)
        {
            objetoInstanciadoActual = Instantiate(
                item.prefab,
                puntoDeRespawn.position,
                puntoDeRespawn.rotation
            );
        }
    }

    void ToggleInformacion()
    {
        infoVisible = !infoVisible;
        textoDescripcion.gameObject.SetActive(infoVisible);
    }

}
