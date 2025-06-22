using UnityEngine;
using System.Collections.Generic;

public class GeneradorPuntosConexion : MonoBehaviour
{
    [Header("Lista de referencias que contienen puntos como hijos")]
    public List<Transform> referenciasDePuntos;

    private List<Transform> puntosDeConexion = new();

    void Awake()
    {
        puntosDeConexion.Clear();

        foreach (Transform referencia in referenciasDePuntos)
        {
            if (referencia == null) continue;

            foreach (Transform hijo in referencia)
            {
                puntosDeConexion.Add(hijo);
                hijo.gameObject.SetActive(false); 
            }
        }
    }

    public void ActivarPuntos()
    {
        foreach (Transform punto in puntosDeConexion)
        {
            if (punto != null)
                punto.gameObject.SetActive(true);
        }

    }

    public void OcultarTodos()
    {
        foreach (Transform punto in puntosDeConexion)
        {
            if (punto != null)
                punto.gameObject.SetActive(false);
        }

    }

    public void DesconectarDelTablero()
    {
        foreach (Transform punto in puntosDeConexion)
        {
            if (punto == null) continue;

            ConexionVisual conexion = punto.GetComponent<ConexionVisual>();
            if (conexion != null)
            {
                conexion.DestruirConexion();
            }

            punto.gameObject.SetActive(false);
        }

    }

    public List<Transform> ObtenerPuntos()
    {
        return puntosDeConexion;
    }
}