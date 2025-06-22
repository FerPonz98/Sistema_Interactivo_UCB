using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class NivelDificilManager : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject Panel_Inicio;
    public GameObject panelSeleccionModo;
    public GameObject panelJuego;
    public GameObject panelFinal;

    [Header("Botones")]
    public Button playButton;
    public Button modoLibreButton;
    public Button contrarrelojButton;
    public Button readyButton;
    public Button finalizarButton;
    public Button reiniciarButton;
    public Button botonSiguienteImagen;

    [Header("Textos")]
    public TextMeshProUGUI textoBienvenida;
    public TextMeshProUGUI textoSeleccionModo;
    public TextMeshProUGUI preguntaText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI textoModo;
    public TextMeshProUGUI textoTiempoFinal;
    public TextMeshProUGUI textoGracias;
    public TextMeshProUGUI errorText;

    [Header("Imágenes")]
    public Image imagenPreguntaActual;
    public Sprite[] imagenesPreguntas;
    public Image imagenSolucionFinal;
    public Sprite[] solucionesImagenes;
    private Sprite[] imagenesDeSolucionActuales;

    [Header("Prefabs")]
    public GameObject[] prefabsPregunta1;
    public GameObject[] prefabsPregunta2;
    public GameObject[] prefabsPregunta3;
    public Transform spawnPoint;


    [Header("Tablero")]
    public GameObject tableroConSockets;

    private int imagenActual = 0;
    private int preguntaAnterior = -1;
    private float tiempoRestante = 0f;
    private bool isContrarreloj = false;
    private float timeElapsed = 0f;
    private bool gameRunning = false;
    private int preguntaIndexSeleccionada;
    private GameObject[] objetosInstanciados;
  

    string[] preguntas = new string[]
    {
        "<align=center><b>Pregunta 1:</b></align>\n<align=left>En un PLC se dispone de dos salidas digitales (KV+ y KV-) para controlar un cilindro neumático de simple efecto. La activación de KV+ debe extender el cilindro, y la activación de KV- debe retraerlo. Se debe incluir una válvula de accionamiento manual que sirva para recoger el vástago del cilindro siempre que se pulse, independientemente del estado de las salidas del PLC.\n\nSelecciona la válvula o válvulas necesarias de entre la disponibilidad, arma el esquema del circuito que permita gobernar el cilindro desde las salidas KV+ y KV- del PLC, e incluye la válvula con el pulsador de recogida. Explica brevemente cómo el PLC controla el cilindro y cómo funciona la válvula con el pulsador de recogida de accionamiento manual.</align>",

        "<align=center><b>Pregunta 2:</b></align>\n<align=left>En un PLC se dispone de dos salidas digitales (KV+ y KV-) para controlar un cilindro neumático de doble efecto. La activación de KV+ debe extender el cilindro, y la activación de KV- debe retraerlo. Se debe incluir una válvula de accionamiento manual que sirva para recoger el vástago del cilindro siempre que se pulse, independientemente del estado de las salidas del PLC.\n\nSelecciona la válvula o válvulas necesarias de entre la disponibilidad, arma el esquema del circuito que permita gobernar el cilindro de doble efecto desde las salidas KV+ y KV- del PLC, e incluye la válvula con el pulsador de recogida. Explica cómo el PLC controla el cilindro y cómo funciona la válvula con el pulsador de recogida de accionamiento manual.</align>",

        "<align=center><b>Pregunta 3:</b></align>\n<align=left>En un PLC se dispone de dos salidas digitales (KV+ y KV-) para controlar un cilindro neumático de simple efecto y un cilindro neumático de doble efecto. La activación de KV+ debe extender el cilindro de simple efecto, mientras que la activación de KV- debe retraerlo. Para el cilindro de doble efecto, la activación de KV+ debe extender el cilindro, y la activación de KV- debe retraerlo. Se debe incluir una válvula de accionamiento manual que sirva para recoger el vástago de ambos cilindros siempre que se pulse, independientemente del estado de las salidas del PLC.\n\nSelecciona la válvula o las válvulas necesarias de entre la disponibilidad, arma el esquema del circuito que permita gobernar ambos cilindros (simple y doble efecto) desde las salidas KV+ y KV- del PLC, e incluye la válvula con el pulsador de recogida.</align>"
    };
    private int[][][] indicesCorrectosPorPreguntaYImagen = new int[][][]
    {
    new int[][] { new int[] { 0,2,5,6, 9 }, new int[] { 0,2,3,6,9,11 }, new int[] { 0,2,3,6,8,11 } },
    new int[][] { new int[] { 0,3,6,7,11 }, new int[] { 0,2,4,8,9,10 }, new int[] { 0,2,5,8,9,10 } },
    new int[][] { new int[] { 0, 1,3,4,7,8,9,11,12,14 }, new int[] { 0, 1,3,5,7,8,9,13 }, new int[] { 0, 1,3,6,7,8,9,11,12 } }
    };


    void Start()
    {
        playButton.onClick.AddListener(ShowSeleccionModo);
        modoLibreButton.onClick.AddListener(() => SelectMode(false));
        contrarrelojButton.onClick.AddListener(() => SelectMode(true));
        readyButton.onClick.AddListener(StartGame);
        finalizarButton.onClick.AddListener(EndGame);
        reiniciarButton.onClick.AddListener(ReiniciarJuego);
        botonSiguienteImagen.onClick.AddListener(MostrarSiguienteImagen);

        MostrarSolo(Panel_Inicio);
        timerText.gameObject.SetActive(true);
        textoBienvenida.text = "¡Bienvenido al nivel dificil";
        textoSeleccionModo.text = "";
        textoModo.text = "";
        readyButton.gameObject.SetActive(false);
        imagenPreguntaActual.sprite = null;
        imagenPreguntaActual.gameObject.SetActive(false);
    }

    void MostrarSolo(GameObject panel)
    {
        Panel_Inicio.SetActive(false);
        panelSeleccionModo.SetActive(false);
        panelJuego.SetActive(false);
        panelFinal.SetActive(false);
        panel.SetActive(true);
    }

    void ShowSeleccionModo()
    {
        MostrarSolo(panelSeleccionModo);
        textoSeleccionModo.text = "Seleccione un modo para comenzar";
        textoModo.text = "Selecciona el modo";
        textoModo.gameObject.SetActive(true);
        readyButton.gameObject.SetActive(false);
    }

    void SelectMode(bool contrarreloj)
    {
        isContrarreloj = contrarreloj;
        readyButton.gameObject.SetActive(true);
    }

    void StartGame()
    {
        MostrarSolo(panelJuego);
        readyButton.gameObject.SetActive(false);
        finalizarButton.gameObject.SetActive(true);

        do
        {
            preguntaIndexSeleccionada = Random.Range(0, preguntas.Length);
        } while (preguntaIndexSeleccionada == preguntaAnterior);
        preguntaText.text = preguntas[preguntaIndexSeleccionada];
        textoModo.text = isContrarreloj ? "Modo: Contrarreloj" : "Modo: Libre";
        textoModo.gameObject.SetActive(true);

        if (imagenesPreguntas.Length > preguntaIndexSeleccionada)
        {
            imagenPreguntaActual.sprite = imagenesPreguntas[preguntaIndexSeleccionada];
            imagenPreguntaActual.gameObject.SetActive(true);
        }
        timeElapsed = 0f;
        timerText.gameObject.SetActive(true);
        gameRunning = true;
        StartCoroutine(ContadorTiempo());


        objetosInstanciados = InstanciarPrefabsPorPregunta(preguntaIndexSeleccionada);

 

        
    }




    GameObject[] InstanciarPrefabsPorPregunta(int index)
    {
        GameObject[] prefabs = index switch
        {
            0 => prefabsPregunta1,
            1 => prefabsPregunta2,
            2 => prefabsPregunta3,
            _ => null
        };

        if (prefabs == null) return null;

        GameObject[] instanciados = new GameObject[prefabs.Length];

        for (int i = 0; i < prefabs.Length; i++)
        {
            Vector3 offsetLocal = new Vector3((i - prefabs.Length / 2f) * 0.6f, 0, 0);
            Vector3 posicionFinal = spawnPoint.TransformPoint(offsetLocal);

            instanciados[i] = Instantiate(prefabs[i], posicionFinal, spawnPoint.rotation);
        }

        return instanciados;
    }

    IEnumerator ContadorTiempo()
    {
        if (isContrarreloj)
        {
            if (tiempoRestante <= 0f)
                tiempoRestante = 2700f;
        }

        while (gameRunning)
        {
            if (isContrarreloj)
            {
                tiempoRestante -= Time.deltaTime;
                tiempoRestante = Mathf.Max(tiempoRestante, 0f);

                int minutos = Mathf.FloorToInt(tiempoRestante / 60f);
                int segundos = Mathf.FloorToInt(tiempoRestante % 60f);

                timerText.text = $"Tiempo: {minutos:00}:{segundos:00}";

                if (tiempoRestante <= 0f)
                {
                    EndGame(); // Finaliza automáticamente
                    yield break;
                }
            }
            else
            {
                timeElapsed += Time.deltaTime;

                int minutos = Mathf.FloorToInt(timeElapsed / 60f);
                int segundos = Mathf.FloorToInt(timeElapsed % 60f);

                timerText.text = $"Tiempo: {minutos:00}:{segundos:00}";
            }

            yield return null;
        }
    }


    public void EndGame()
    {
        gameRunning = false;
        MostrarSolo(panelFinal);

        textoGracias.text = "¡Gracias por participar!";
        if (isContrarreloj)
        {
            int minutos = Mathf.FloorToInt(tiempoRestante / 60f);
            int segundos = Mathf.FloorToInt(tiempoRestante % 60f);

            if (tiempoRestante > 0f)
            {
                textoTiempoFinal.text = $"Tiempo restante: {minutos:00}:{segundos:00}";
            }
            else
            {
                textoTiempoFinal.text = "Tiempo agotado: 00:00";
            }
        }
        else
        {
            int minutos = Mathf.FloorToInt(timeElapsed / 60f);
            int segundos = Mathf.FloorToInt(timeElapsed % 60f);
            textoTiempoFinal.text = $"Tiempo total: {minutos:00}:{segundos:00}";
        }


        int indiceInicio = preguntaIndexSeleccionada * 3;

        if (solucionesImagenes.Length >= indiceInicio + 3)
        {
            imagenesDeSolucionActuales = new Sprite[]
            {
        solucionesImagenes[indiceInicio],
        solucionesImagenes[indiceInicio + 1],
        solucionesImagenes[indiceInicio + 2]
            };

            imagenActual = 0;
            imagenSolucionFinal.sprite = imagenesDeSolucionActuales[imagenActual];
            botonSiguienteImagen.gameObject.SetActive(true);
            MostrarValidacionImagenActual();

        }

    }

    public void MostrarSiguienteImagen()
    {
        if (imagenesDeSolucionActuales == null || imagenesDeSolucionActuales.Length == 0)
            return;

        imagenActual = (imagenActual + 1) % imagenesDeSolucionActuales.Length;
        MostrarValidacionImagenActual();
    }
    public void MostrarValidacionImagenActual()
    {
        RestaurarColoresOriginales();

        imagenSolucionFinal.sprite = imagenesDeSolucionActuales[imagenActual];
        var conectadas = ObtenerValvulasConectadas();
        int[] indicesCorrectos = indicesCorrectosPorPreguntaYImagen[preguntaIndexSeleccionada][imagenActual];
        int errores = 0;

        bool casoEspecial = (preguntaIndexSeleccionada == 2 && imagenActual == 1);
        int[] grupoOpcional = new int[] { 3, 12,14 };
        int usados = 0;

        for (int i = 0; i < objetosInstanciados.Length; i++)
        {
            var valvula = objetosInstanciados[i];
            bool conectada = conectadas.Contains(valvula);
            bool esCorrecta = System.Array.Exists(indicesCorrectos, x => x == i);

            if (casoEspecial && System.Array.Exists(grupoOpcional, x => x == i))
            {
                if (conectada) usados++;
                continue;
            }

            if (conectada && !esCorrecta)
            {
                ResaltarErrorVisual(valvula, Color.red);
                errores++;
            }
            else if (!conectada && esCorrecta)
            {
                ResaltarErrorVisual(valvula, Color.yellow);
                errores++;
            }
        }

        if (casoEspecial)
        {
            if (usados == 0)
            {
                foreach (int i in grupoOpcional)
                {
                    ResaltarErrorVisual(objetosInstanciados[i], Color.yellow);
                }
                errores++;
            }
            else if (usados > 1)
            {
                foreach (int i in grupoOpcional)
                {
                    if (conectadas.Contains(objetosInstanciados[i]))
                    {
                        ResaltarErrorVisual(objetosInstanciados[i], Color.red);
                    }
                }
                errores++;
            }
        }

        errorText.text = $"Errores: {errores}";
        errorText.gameObject.SetActive(true);
    }
    void ReiniciarJuego()
    {
        MostrarSolo(Panel_Inicio);
        readyButton.gameObject.SetActive(false);
        finalizarButton.gameObject.SetActive(false);
        textoModo.gameObject.SetActive(true);
        timerText.gameObject.SetActive(false);
        preguntaText.text = "";
        textoSeleccionModo.text = "";
        textoModo.text = "";
        imagenPreguntaActual.sprite = null;
        imagenPreguntaActual.gameObject.SetActive(false);

        if (objetosInstanciados != null)
        {
            foreach (GameObject obj in objetosInstanciados)
            {
                if (obj != null)
                {
                    foreach (Transform child in obj.transform)
                    {
                        Destroy(child.gameObject);
                    }

                    Destroy(obj);
                }
            }
        }

        var cables = GameObject.FindGameObjectsWithTag("Cable");
        foreach (var cable in cables)
        {
            Destroy(cable);
        }

        var etiquetas = GameObject.FindGameObjectsWithTag("Etiqueta");
        foreach (var etiqueta in etiquetas)
        {
            Destroy(etiqueta);
        }

        ConexionVisual[] conexiones = FindObjectsOfType<ConexionVisual>();
        foreach (var conexion in conexiones)
        {
            conexion.DestruirConexion();
        }
    }



    List<GameObject> ObtenerValvulasConectadas()
    {
        XRSocketInteractor[] sockets = tableroConSockets.GetComponentsInChildren<XRSocketInteractor>();
        List<GameObject> valvulasConectadas = new();

        foreach (var socket in sockets)
        {
            if (socket.hasSelection)
            {
                var interactable = socket.GetOldestInteractableSelected();
                if (interactable != null)
                {
                    valvulasConectadas.Add(interactable.transform.gameObject);
                }
            }
        }

        return valvulasConectadas;
    }
    void ResaltarErrorVisual(GameObject valvula, Color color)
    {
        Renderer[] renderers = valvula.GetComponentsInChildren<Renderer>();
        foreach (var rend in renderers)
        {
            foreach (var mat in rend.materials)
            {
                mat.EnableKeyword("_EMISSION");
                mat.SetColor("_EmissionColor", color * 0.5f);
                mat.color = Color.Lerp(mat.color, color, 0.6f);
            }
        }
        StartCoroutine(ParpadeoError(valvula, color));
    }

    IEnumerator ParpadeoError(GameObject valvula, Color color)
    {
        if (valvula == null) yield break;

        Renderer[] renderers = valvula.GetComponentsInChildren<Renderer>();
        float duracion = 2f;
        float tiempo = 0f;

        while (tiempo < duracion)
        {
            if (valvula == null) yield break;

            float intensidad = Mathf.PingPong(Time.time * 2, 1f);

            foreach (var rend in renderers)
            {
                if (rend == null) continue;

                try
                {
                    foreach (var mat in rend.materials)
                    {
                        if (mat != null)
                            mat.SetColor("_EmissionColor", color * intensidad);
                    }
                }
                catch (MissingReferenceException)
                {
                    yield break; 
                }
            }

            tiempo += Time.deltaTime;
            yield return null;
        }
    }

    void RestaurarColoresOriginales()
    {
        foreach (var valvula in objetosInstanciados)
        {
            Renderer[] renderers = valvula.GetComponentsInChildren<Renderer>();
            foreach (var rend in renderers)
            {
                foreach (var mat in rend.materials)
                {
                    mat.DisableKeyword("_EMISSION");
                    mat.color = Color.white; 
                }
            }
        }
    }
}