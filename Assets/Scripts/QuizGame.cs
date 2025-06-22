using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class QuizGame : MonoBehaviour
{
    [Header("Configuración de Preguntas")]
    public List<Pregunta> todasLasPreguntas; // Lista de preguntas

    [Header("Referencias de UI")]
    public Image preguntaImagen;
    public TextMeshProUGUI preguntaTexto;
    public Button[] botonesRespuestas;
    public TextMeshProUGUI tiempoTexto;
    public TextMeshProUGUI puntuacionTexto;

    [Header("Paneles de Juego")]
    public GameObject panelInicio;
    public GameObject panelDificultad;
    public GameObject panelPreguntas;
    public GameObject panelFin;

    [Header("Botones de Dificultad")]
    public Button botonFacil;
    public Button botonMedio;
    public Button botonDificil;
    public Button botonReiniciar;

    private Pregunta preguntaActual;
    private int puntuacion = 0;
    private int indicePregunta = 0;
    private float tiempoRestante = 60f;
    private bool juegoEnCurso = false;
    private string dificultadActual = "Medio"; // Por defecto

    void Start()
    {
        // Ocultar todos los paneles excepto el inicio
        panelInicio.SetActive(true);
        panelDificultad.SetActive(false);
        panelPreguntas.SetActive(false);
        panelFin.SetActive(false);

        // Asignar eventos a los botones de dificultad
        botonFacil.onClick.AddListener(() => SeleccionarDificultad("Facil"));
        botonMedio.onClick.AddListener(() => SeleccionarDificultad("Medio"));
        botonDificil.onClick.AddListener(() => SeleccionarDificultad("Dificil"));
        botonReiniciar.onClick.AddListener(ReiniciarJuego);

        // Asegurar que los botones de respuesta están correctamente asignados
        foreach (Button boton in botonesRespuestas)
        {
            boton.onClick.RemoveAllListeners();
        }
    }

    public void IniciarJuego()
    {
        panelInicio.SetActive(false);
        panelDificultad.SetActive(true); // Mostrar panel de selección de dificultad
    }

    public void SeleccionarDificultad(string dificultad)
    {
        dificultadActual = dificultad;
        panelDificultad.SetActive(false);
        panelPreguntas.SetActive(true);
        juegoEnCurso = true;
        puntuacion = 0;
        tiempoRestante = 60f;
        indicePregunta = 0;

        SiguientePregunta();
        StartCoroutine(Temporizador());
    }

    IEnumerator Temporizador()
    {
        while (juegoEnCurso && tiempoRestante > 0)
        {
            tiempoTexto.text = "Tiempo: " + Mathf.CeilToInt(tiempoRestante);
            tiempoRestante -= Time.deltaTime;
            yield return null;
        }

        TerminarJuego();
    }

    void SiguientePregunta()
    {
        if (indicePregunta >= todasLasPreguntas.Count)
        {
            TerminarJuego();
            return;
        }

        preguntaActual = todasLasPreguntas[indicePregunta];
        preguntaImagen.sprite = preguntaActual.imagen;
        preguntaTexto.text = preguntaActual.texto;

        for (int i = 0; i < botonesRespuestas.Length; i++)
        {
            botonesRespuestas[i].GetComponentInChildren<TextMeshProUGUI>().text = preguntaActual.opciones[i];
            int index = i;
            botonesRespuestas[i].onClick.RemoveAllListeners();
            botonesRespuestas[i].onClick.AddListener(() => VerificarRespuesta(index));
        }
    }

    public void VerificarRespuesta(int index)
    {
        if (!juegoEnCurso) return;

        if (index == preguntaActual.respuestaCorrecta)
        {
            puntuacion += 10;
        }
        else
        {
            if (dificultadActual == "Facil") tiempoRestante -= 5f;
            else if (dificultadActual == "Medio") puntuacion -= 5;
            else if (dificultadActual == "Dificil")
            {
                puntuacion -= 5;
                tiempoRestante -= 5f;
            }
        }

        puntuacionTexto.text = "Puntuación: " + puntuacion;
        indicePregunta++;
        SiguientePregunta();
    }

    void TerminarJuego()
    {
        juegoEnCurso = false;
        panelPreguntas.SetActive(false);
        panelFin.SetActive(true);
    }

    public void ReiniciarJuego()
    {
        panelFin.SetActive(false);
        panelInicio.SetActive(true);
        indicePregunta = 0;
    }
}

[System.Serializable]
public class Pregunta
{
    public string texto;
    public Sprite imagen;
    public string[] opciones;
    public int respuestaCorrecta;
}
