using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ConexionVisual : MonoBehaviour
{
    [Header("Cable visual")]
    public GameObject cablePrefab;

    [Header("Opcional: Punto de reinicio manual (si no se asigna, usa posición inicial actual)")]
    public Transform puntoDeReinicioManual;

    private GameObject cableActual;
    private LineRenderer lineRenderer;

    private Transform puntoInicio = null;
    private Transform puntoFinal = null;

    private Vector3 posicionInicial;
    private Vector3 posicionInicioCable;

    private Renderer esferaRenderer;

    public Color colorInicial = Color.yellow;
    public Color colorConectado = Color.green;

    private bool siguiendoObjeto = false;
    private bool estaConectado = false;

    void Start()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnAgarrar);
        grabInteractable.selectExited.AddListener(OnSoltar);

        posicionInicial = transform.position;
        esferaRenderer = GetComponentInChildren<Renderer>();

        if (esferaRenderer != null)
        {
            esferaRenderer.material.color = colorInicial;
        }
    }

    void Update()
    {
        if (siguiendoObjeto && cableActual != null && puntoInicio != null)
        {
            lineRenderer.SetPosition(1, transform.position);
        }
    }

    void OnAgarrar(SelectEnterEventArgs args)
    {
        if (puntoInicio == null)
        {
            puntoInicio = transform;
            posicionInicioCable = puntoInicio.position;
            cableActual = Instantiate(cablePrefab);

            if (cableActual != null)
            {
                lineRenderer = cableActual.GetComponent<LineRenderer>();

                if (lineRenderer != null)
                {
                    lineRenderer.positionCount = 2;
                    lineRenderer.SetPosition(0, posicionInicioCable);
                    lineRenderer.SetPosition(1, posicionInicioCable);
                    siguiendoObjeto = true;
                }
            }
        }
    }

    void OnSoltar(SelectExitEventArgs args)
    {
        if (cableActual == null || lineRenderer == null)
            return;

        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.2f);
        bool seConecto = false;
        Transform puntoCercano = null;
        float distanciaMasCorta = float.MaxValue;

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Conexion") && col.transform != transform)
            {
                float distancia = Vector3.Distance(transform.position, col.transform.position);

                if (distancia < distanciaMasCorta)
                {
                    puntoCercano = col.transform;
                    distanciaMasCorta = distancia;
                }
            }
        }

        if (puntoCercano != null)
        {
            puntoFinal = puntoCercano;
            ReiniciarPosicion();

            lineRenderer.SetPosition(0, posicionInicioCable);
            lineRenderer.SetPosition(1, puntoFinal.position);

            if (esferaRenderer != null)
                esferaRenderer.material.color = colorConectado;

            estaConectado = true;
            seConecto = true;
        }

        if (!seConecto)
        {
            DestruirConexion();
        }
        else
        {
            siguiendoObjeto = false;
        }
    }


    public void DestruirConexion()
    {
        if (cableActual != null)
        {
            Destroy(cableActual);
        }

        ReiniciarPosicion();

        puntoInicio = null;
        puntoFinal = null;
        siguiendoObjeto = false;
        estaConectado = false;

        if (esferaRenderer != null)
        {
            esferaRenderer.material.color = colorInicial;
        }
    }

    private void ReiniciarPosicion()
    {
        if (puntoDeReinicioManual != null)
        {
            transform.position = puntoDeReinicioManual.position;
        }
        else
        {
            transform.position = posicionInicial;
        }
    }
}