using UnityEngine;
using UnityEngine.InputSystem;

// Ataque tipo pistola no automatica (un disparo por click) usando Physics.Raycast.
// Sin sistema de cargadores: las balas son un pozo fijo que baja y no se recarga.
public class PlayerShooting : MonoBehaviour
{
    [Header("Origen del disparo")]
    [SerializeField] private Transform cameraTransform;

    [Header("Balance del arma")]
    [SerializeField] private float range = 20f;
    [SerializeField] private float damage = 25f;
    [SerializeField] private float fireRate = 1.5f; // disparos por segundo
    [SerializeField] private int bullets = 10;

    private float nextFireTime = 0f;
    private PlayerHealth health;

    public int Bullets => bullets;

    private void Awake()
    {
        // PlayerShooting vive en la Camera, pero PlayerHealth vive en el Player (su padre).
        // GetComponentInParent busca el componente en este objeto y, si no lo encuentra, sube por los padres.
        health = GetComponentInParent<PlayerHealth>();
    }

    // Llamado desde afuera (por ejemplo, AmmoPickup) para sumar balas recolectadas.
    public void AddBullets(int amount)
    {
        bullets += amount;
    }

    private void Update()
    {
        if (health.IsDead) return;

        // wasPressedThisFrame: un disparo por click, no se puede mantener apretado (no automática).
        bool canFire = Time.time >= nextFireTime && bullets > 0;

        if (Mouse.current.leftButton.wasPressedThisFrame && canFire)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        nextFireTime = Time.time + (1f / fireRate);
        bullets--;

        // Dispara desde el centro de la pantalla: la posición y la dirección "adelante" de la cámara.
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, range))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                // SendMessage evita que este script dependa de una clase de enemigo especifica.
                // Si el enemigo tiene un metodo TakeDamage(float), lo recibe; si no, no pasa nada.
                hit.collider.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}