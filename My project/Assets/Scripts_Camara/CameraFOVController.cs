using UnityEngine;
using UnityEngine.InputSystem;

// Controla el FOV (Field of View) de la cámara en primera persona.
// T baja el FOV, Y lo sube, dentro de un rango mínimo y máximo.
[RequireComponent(typeof(Camera))]
public class CameraFOVController : MonoBehaviour
{
    [Header("Balance de FOV")]
    [SerializeField] private float fovChangeSpeed = 30f; // grados por segundo
    [SerializeField] private float minFOV = 50f;
    [SerializeField] private float maxFOV = 120f;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        HandleFOVInput();
    }

    private void HandleFOVInput()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return;

        float fov = cam.fieldOfView;

        // T mantenida: baja el FOV.
        if (keyboard.tKey.isPressed)
        {
            fov -= fovChangeSpeed * Time.deltaTime;
        }

        // Y mantenida: sube el FOV.
        if (keyboard.yKey.isPressed)
        {
            fov += fovChangeSpeed * Time.deltaTime;
        }

        // Clamp: nunca deja que el FOV salga del rango permitido.
        cam.fieldOfView = Mathf.Clamp(fov, minFOV, maxFOV);
    }
}
