using UnityEngine;
using UnityEngine.InputSystem;

// Rota el cuerpo del jugador (izquierda/derecha) y la camara (arriba/abajo)
// segun el movimiento del mouse. Control estandar de "mirar" en primera persona.
public class MouseLook : MonoBehaviour
{
    [Header("Cámara")]
    [SerializeField] private Transform cameraTransform;

    [Header("Sensibilidad")]
    [SerializeField] private float sensitivity = 15f;
    [SerializeField] private float minPitch = -80f;
    [SerializeField] private float maxPitch = 80f;

    private float pitch = 0f; // rotacion acumulada en X (arriba/abajo)

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        float mouseX = mouseDelta.x * sensitivity * Time.deltaTime;
        float mouseY = mouseDelta.y * sensitivity * Time.deltaTime;

        // Yaw: gira todo el cuerpo del jugador sobre el eje Y (izquierda/derecha).
        transform.Rotate(Vector3.up * mouseX);

        // Pitch: gira solo la camara sobre el eje X (arriba/abajo).
        // Se acumula en una variable aparte y se limita para no poder dar una vuelta completa.
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        cameraTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }
}
