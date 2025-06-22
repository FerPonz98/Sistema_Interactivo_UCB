using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PuntoConexionTablero : MonoBehaviour
{
    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor socket;

    void Awake()
    {
        socket = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>();
    }

    void OnEnable()
    {
        if (socket != null)
        {
            socket.selectEntered.AddListener(OnObjetoConectado);
            socket.selectExited.AddListener(OnObjetoDesconectado);
        }
    }

    void OnDisable()
    {
        if (socket != null)
        {
            socket.selectEntered.RemoveListener(OnObjetoConectado);
            socket.selectExited.RemoveListener(OnObjetoDesconectado);
        }
    }

    void OnObjetoConectado(SelectEnterEventArgs args)
    {
        var obj = args.interactableObject?.transform?.GetComponent<GeneradorPuntosConexion>();
        if (obj != null)
        {
            
            obj.ActivarPuntos();
        }
        else
        {
            Debug.LogWarning($"⚠️ El objeto conectado en {gameObject.name} no tiene GeneradorPuntosConexion.");
        }
    }

    void OnObjetoDesconectado(SelectExitEventArgs args)
    {
        var obj = args.interactableObject?.transform?.GetComponent<GeneradorPuntosConexion>();
        if (obj != null)
        {
            obj.OcultarTodos();
        }
    }
}
