using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable))]
[RequireComponent(typeof(Rigidbody))]
public class GrabAndRotateObject : MonoBehaviour
{
    public MonoBehaviour turnProvider;
    public InputActionReference turnInput;

    [Header("Partes visuales que quieres rotar")]
    public Transform[] partesVisuales;

    [Header("Cámara del jugador (Main Camera del XR Origin)")]
    public Transform camaraJugador;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab;
    private Rigidbody rb;

    private UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor interactor;
    private Vector3 grabOffset = Vector3.zero;

    void Awake()
    {
        grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        // 🚫 Evitamos que XR Toolkit mueva la posición/rotación automáticamente
        grab.trackPosition = false;
        grab.trackRotation = false;
        grab.throwOnDetach = false;
        grab.movementType = UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable.MovementType.Instantaneous;

        grab.selectEntered.AddListener(OnGrab);
        grab.selectExited.AddListener(OnRelease);
    }

    void OnDestroy()
    {
        grab.selectEntered.RemoveListener(OnGrab);
        grab.selectExited.RemoveListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        if (turnProvider != null)
        {
            turnProvider.enabled = false;
            Debug.Log("🟡 TurnProvider desactivado");
        }

        if (turnInput != null)
            turnInput.action.Enable();

        interactor = args.interactorObject;
        grabOffset = transform.position - interactor.transform.position;

        rb.useGravity = false;
        rb.isKinematic = true;
    }

    void OnRelease(SelectExitEventArgs args)
    {
        if (turnProvider != null)
        {
            turnProvider.enabled = true;
            Debug.Log("✅ TurnProvider activado");
        }

        interactor = null;

        rb.useGravity = true;
        rb.isKinematic = false;
    }

    void Update()
    {
        if (grab != null && grab.isSelected && interactor != null && turnInput != null && camaraJugador != null)
        {
            Vector2 input = turnInput.action.ReadValue<Vector2>();
            float giro = input.x;
            float zoom = input.y;

            // ✅ Rotación
            if (Mathf.Abs(giro) > 0.1f && partesVisuales.Length > 0)
            {
                foreach (Transform parte in partesVisuales)
                {
                    parte.Rotate(Vector3.up, giro * 100f * Time.deltaTime);
                }
                Debug.Log("🔄 Rotando partes visuales...");
            }

            // ✅ Zoom (acercar/alejar)
            if (Mathf.Abs(zoom) > 0.1f)
            {
                Vector3 dir = (transform.position - camaraJugador.position).normalized;

                // Calculamos nuevo offset temporal
                Vector3 nuevoOffset = grabOffset + dir * zoom * 1.5f * Time.deltaTime;
                float nuevaDistancia = nuevoOffset.magnitude;

                if (nuevaDistancia >= 0.3f && nuevaDistancia <= 3.5f)
                {
                    grabOffset = nuevoOffset;
                    Debug.Log("📸 Offset ajustado correctamente (zoom)...");
                }
                else
                {
                    Debug.Log("⛔ Límite de distancia alcanzado. No se ajusta más.");
                }
            }

            // ✅ Aplicar posición actualizada al objeto
            transform.position = interactor.transform.position + grabOffset;
            transform.rotation = interactor.transform.rotation;
        }
    }
}