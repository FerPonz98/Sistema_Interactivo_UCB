using UnityEngine;
using Unity.XR.CoreUtils;

[AddComponentMenu("XR/Posición Inicial XROrigin")]
public class GestorPosicionInicialXROrigin : MonoBehaviour
{
    [Header("Referencia al XR Origin")]
    public XROrigin xrOrigin;

    [Header("Punto de inicio (arrastra aquí un Transform)")]
    public Transform puntoInicial;

    [Header("Opciones")]
    public bool loadOnStart = true;
    public bool saveOnQuit = false;

    private const string KEY_X = "XR_HomeX";
    private const string KEY_Y = "XR_HomeY";
    private const string KEY_Z = "XR_HomeZ";

    void Awake()
    {
        if (xrOrigin == null)
            xrOrigin = FindObjectOfType<XROrigin>();
        if (loadOnStart)
            MoveToHome();
    }

    void OnApplicationQuit()
    {
        if (saveOnQuit)
            SaveCurrentAsHome();
    }

    public void MoveToHome()
    {
        Vector3 home;
        if (puntoInicial != null)
        {
            home = puntoInicial.position;
        }
        else
        {
            float x = PlayerPrefs.GetFloat(KEY_X, 0f);
            float y = PlayerPrefs.GetFloat(KEY_Y, 0f);
            float z = PlayerPrefs.GetFloat(KEY_Z, 0f);
            home = new Vector3(x, y, z);
        }
        xrOrigin.MoveCameraToWorldLocation(home);
    }

    public void SaveCurrentAsHome()
    {
        // Obtener posición mundial de la cámara principal dentro del XR Origin
        Vector3 camWorldPos = xrOrigin.Camera.transform.position;
        PlayerPrefs.SetFloat(KEY_X, camWorldPos.x);
        PlayerPrefs.SetFloat(KEY_Y, camWorldPos.y);
        PlayerPrefs.SetFloat(KEY_Z, camWorldPos.z);
        PlayerPrefs.Save();
    }

    public void ResetToHome()
    {
        MoveToHome();
    }
}