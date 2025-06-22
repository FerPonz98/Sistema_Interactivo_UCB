using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Obi;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class CableExtremoConectable : MonoBehaviour
{
    public ObiParticleAttachment attachment;
    public Renderer miRenderer;
    public Color colorConectado = Color.green;
    public Color colorNormal = Color.white;
    public float distanciaDesconexion = 0.2f;

    private XRGrabInteractable grab;
    private Transform puntoConectado;
    private GameObject puntoReferencia;
    private bool conectado = false;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grab = GetComponent<XRGrabInteractable>();
        grab.selectEntered.AddListener(OnGrab);
        grab.selectExited.AddListener(OnRelease);
    }

    private void Start()
    {
        if (miRenderer != null)
            miRenderer.material.color = colorNormal;
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints.None;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints.None;
    }

    public void SetConexionVisual(Transform punto)
    {
        puntoConectado = punto;
        puntoReferencia = punto.gameObject;
        conectado = true;

        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints.None;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.position = punto.position;
        rb.rotation = punto.rotation;

        attachment.attachmentType = ObiParticleAttachment.AttachmentType.Static;
        attachment.target = punto;
        attachment.enabled = false;
        attachment.enabled = true;

        rb.constraints = RigidbodyConstraints.FreezeAll;
        rb.isKinematic = true;

        if (miRenderer != null)
            miRenderer.material.color = colorConectado;

        puntoReferencia.SetActive(false); // Oculta el punto de referencia al conectarse
    }

    private void Update()
    {
        if (conectado && puntoConectado != null)
        {
            float distancia = Vector3.Distance(transform.position, puntoConectado.position);
            if (distancia > distanciaDesconexion)
            {
                Desconectar();
            }
        }
    }

    public void Desconectar()
    {
        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints.None;

        if (miRenderer != null)
            miRenderer.material.color = colorNormal;

        attachment.attachmentType = ObiParticleAttachment.AttachmentType.Dynamic;
        attachment.target = null;
        attachment.enabled = false;
        attachment.enabled = true;

        if (puntoReferencia != null)
            puntoReferencia.SetActive(true); // Vuelve a mostrar el punto al desconectar

        puntoConectado = null;
        puntoReferencia = null;
        conectado = false;
    }
}