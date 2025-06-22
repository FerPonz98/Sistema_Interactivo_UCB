﻿using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class NivelFacilManager : MonoBehaviour
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

    [Header("Prefabs")]
    public GameObject[] prefabsPregunta1;
    public GameObject[] prefabsPregunta2;
    public GameObject[] prefabsPregunta3;
    public Transform spawnPoint;

    private int preguntaAnterior = -1;
    private float tiempoRestante = 0f;
    private bool isContrarreloj = false;
    private float timeElapsed = 0f;
    private bool gameRunning = false;
    private int preguntaIndexSeleccionada;
    private GameObject[] objetosInstanciados;
   

    private int[] indicesCorrectosP1 = { 0, 1, 2, 4, 6 };
    private int[] indicesCorrectosP2 = { 2, 3, 4, 5, 6 };
    private int[] indicesCorrectosP3 = { 0, 1, 2, 3, 4, 5, 6, 7 , 8, 9};

    [Header("Tablero de conexión")]
    public GameObject tableroConSockets;


    string[] preguntas = new string[]
        {
        "<align=center><b>Pregunta 1:</b></align>\n<align=left>En un PLC se dispone de dos salidas digitales (KV+ y KV-) para controlar un cilindro neumático de simple efecto. La activación de KV+ debe extender el cilindro, y la activación de KV- debe retraerlo. Se debe incluir una válvula de accionamiento manual que sirva para recoger el vástago del cilindro siempre que se pulse, independientemente del estado de las salidas del PLC.\n\nSelecciona la válvula o válvulas necesarias de entre la disponibilidad, arma el esquema del circuito que permita gobernar el cilindro desde las salidas KV+ y KV- del PLC, e incluye la válvula con el pulsador de recogida. Explica brevemente cómo el PLC controla el cilindro y cómo funciona la válvula con el pulsador de recogida de accionamiento manual.</align>",

        "<align=center><b>Pregunta 2:</b></align>\n<align=left>En un PLC se dispone de dos salidas digitales (KV+ y KV-) para controlar un cilindro neumático de doble efecto. La activación de KV+ debe extender el cilindro, y la activación de KV- debe retraerlo. Se debe incluir una válvula de accionamiento manual que sirva para recoger el vástago del cilindro siempre que se pulse, independientemente del estado de las salidas del PLC.\n\nSelecciona la válvula o válvulas necesarias de entre la disponibilidad, arma el esquema del circuito que permita gobernar el cilindro de doble efecto desde las salidas KV+ y KV- del PLC, e incluye la válvula con el pulsador de recogida. Explica cómo el PLC controla el cilindro y cómo funciona la válvula con el pulsador de recogida de accionamiento manual.</align>",

        "<align=center><b>Pregunta 3:</b></align>\n<align=left>En un PLC se dispone de dos salidas digitales (KV+ y KV-) para controlar un cilindro neumático de simple efecto y un cilindro neumático de doble efecto. La activación de KV+ debe extender el cilindro de simple efecto, mientras que la activación de KV- debe retraerlo. Para el cilindro de doble efecto, la activación de KV+ debe extender el cilindro, y la activación de KV- debe retraerlo. Se debe incluir una válvula de accionamiento manual que sirva para recoger el vástago de ambos cilindros siempre que se pulse, independientemente del estado de las salidas del PLC.\n\nSelecciona la válvula o las válvulas necesarias de entre la disponibilidad, arma el esquema del circuito que permita gobernar ambos cilindros (simple y doble efecto) desde las salidas KV+ y KV- del PLC, e incluye la válvula con el pulsador de recogida.</align>"
        };

    void Start()
    {
        playButton.onClick.AddListener(ShowSeleccionModo);
        modoLibreButton.onClick.AddListener(() => SelectMode(false));
        contrarrelojButton.onClick.AddListener(() => SelectMode(true));
        readyButton.onClick.AddListener(StartGame);
        finalizarButton.onClick.AddListener(EndGame);
        reiniciarButton.onClick.AddListener(ReiniciarJuego);

        MostrarSolo(Panel_Inicio);
        timerText.gameObject.SetActive(true);
        textoBienvenida.text = "¡Bienvenido al Nivel Fácil!";
        readyButton.gameObject.SetActive(false);
        imagenPreguntaActual.gameObject.SetActive(false);
        errorText.gameObject.SetActive(false);
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

        preguntaAnterior = preguntaIndexSeleccionada;
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
            Vector3 offset = new Vector3((i - prefabs.Length / 2f) * 0.6f, 0, 0);
            instanciados[i] = Instantiate(prefabs[i], spawnPoint.TransformPoint(offset), spawnPoint.rotation);
        }

        return instanciados;
    }

    IEnumerator ContadorTiempo()
    {
        if (isContrarreloj && tiempoRestante <= 0f)
            tiempoRestante = 900f;

        while (gameRunning)
        {
            if (isContrarreloj)
                tiempoRestante -= Time.deltaTime;
            else
                timeElapsed += Time.deltaTime;

            int min = Mathf.FloorToInt((isContrarreloj ? tiempoRestante : timeElapsed) / 60f);
            int sec = Mathf.FloorToInt((isContrarreloj ? tiempoRestante : timeElapsed) % 60f);
            timerText.text = $"Tiempo: {min:00}:{sec:00}";

            if (isContrarreloj && tiempoRestante <= 0f)
            {
                EndGame();
                yield break;
            }

            yield return null;
        }
    }

    public void EndGame()
    {
        gameRunning = false;
        int cantidadErrores = 0;

        if (objetosInstanciados != null)
        {
            int[] indicesCorrectos = preguntaIndexSeleccionada switch
            {
                0 => indicesCorrectosP1,
                1 => indicesCorrectosP2,
                2 => indicesCorrectosP3,
                _ => new int[0]
            };

            var valvulasConectadas = ObtenerValvulasConectadas();

            for (int i = 0; i < objetosInstanciados.Length; i++)
            {
                GameObject valvula = objetosInstanciados[i];
                bool conectada = valvulasConectadas.Contains(valvula);
                bool esCorrecta = System.Array.Exists(indicesCorrectos, x => x == i);

                if (conectada && !esCorrecta)
                {
                    ResaltarErrorVisual(valvula, Color.red);
                    cantidadErrores++;
                }
                else if (!conectada && esCorrecta)
                {
                    ResaltarErrorVisual(valvula, Color.yellow);
                    cantidadErrores++;
                }
            }
        }

        MostrarSolo(panelFinal);

        if (cantidadErrores > 0)
        {
            errorText.text = $"Errores: {cantidadErrores}";
            errorText.gameObject.SetActive(true);
        }
        else
        {
            errorText.gameObject.SetActive(false);
        }

        textoGracias.text = "¡Gracias por participar!";
        int minutos = Mathf.FloorToInt((isContrarreloj ? tiempoRestante : timeElapsed) / 60f);
        int segundos = Mathf.FloorToInt((isContrarreloj ? tiempoRestante : timeElapsed) % 60f);
        textoTiempoFinal.text = isContrarreloj && tiempoRestante <= 0f
            ? "Tiempo agotado: 00:00"
            : (isContrarreloj ? $"Tiempo restante: {minutos:00}:{segundos:00}" : $"Tiempo total: {minutos:00}:{segundos:00}");

        if (solucionesImagenes.Length > preguntaIndexSeleccionada)
        {
            imagenSolucionFinal.sprite = solucionesImagenes[preguntaIndexSeleccionada];
        }
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
        errorText.text = "";
        errorText.gameObject.SetActive(false);

        if (objetosInstanciados != null)
        {
            foreach (GameObject obj in objetosInstanciados)
            {
                if (obj != null)
                {
                    foreach (Transform child in obj.transform)
                    {
                        if (child.CompareTag("Etiqueta"))
                        {
                            Destroy(child.gameObject);
                        }
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



}
