using UnityEngine;
using UnityEngine.InputSystem;

// Movimiento estandar de FPS (WASD relativo a hacia dónde mira el jugador) + salto.
// Usa Rigidbody para que la gravedad y las colisiones funcionen de verdad.
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float speed = 5f;

    [Header("Salto")]
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float staminaCostPerJump = 5f;
    [SerializeField] private float groundCheckDistance = 1.1f;

    private Rigidbody rb;
    private PlayerHealth health;
    private PlayerStamina stamina;
    private InputAction moveAction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        health = GetComponent<PlayerHealth>();
        stamina = GetComponent<PlayerStamina>();

        moveAction = new InputAction("Move", InputActionType.Value);
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");
    }

    private void OnEnable() => moveAction.Enable();
    private void OnDisable() => moveAction.Disable();

    private void Update()
    {
        // El salto se detecta en Update (una vez por frame apretada la tecla),
        // pero se aplica fisicamente en FixedUpdate a traves del Rigidbody.
        if (health.IsDead) return;

        if (Keyboard.current.spaceKey.wasPressedThisFrame && IsGrounded())
        {
            TryJump();
        }
    }

    private void FixedUpdate()
    {
        // Muerto: se frena en X/Z pero se sigue cayendo por gravedad en Y.
        if (health.IsDead)
        {
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
            return;
        }

        Move();
    }

    private void Move()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();

        // transform.right y transform.forward: la direccion se calcula respecto
        // a hacia donde mira el jugador (lo rota MouseLook), no respecto al mundo.
        Vector3 direction = transform.right * input.x + transform.forward * input.y;

        // Se conserva la velocidad vertical actual (rb.linearVelocity.y) para no
        // pisar la gravedad ni el impulso del salto.
        rb.linearVelocity = new Vector3(direction.x * speed, rb.linearVelocity.y, direction.z * speed);
    }

    private void TryJump()
    {
        if (!stamina.TryConsume(staminaCostPerJump)) return;

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
    }

    private bool IsGrounded()
    {
        // Un rayo corto hacia abajo desde el centro del personaje.
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);
    }
}