using UnityEngine;

// Ataque del enemigo tipo pistola no automática (un disparo, cooldown, siguiente disparo),
// usando Physics.Raycast. Balas ilimitadas: no hay contador ni recarga.
public class EnemyShooting : MonoBehaviour
{
    [Header("Objetivo")]
    [SerializeField] private Transform target;

    [Header("Balance del arma")]
    [SerializeField] private float range = 5f;
    [SerializeField] private float damage = 20f;
    [SerializeField] private float fireRate = 2f; // disparos por segundo

    private float nextFireTime = 0f;
    private EnemyHealth health;

    private void Awake()
    {
        health = GetComponent<EnemyHealth>();
    }

    private void Update()
    {
        if (health.IsDead || target == null) return;

        if (Time.time >= nextFireTime)
        {
            TryShoot();
        }
    }

    private void TryShoot()
    {
        Vector3 direction = (target.position - transform.position).normalized;

        // El Raycast solo llega hasta "range": si el jugador está más lejos, ni siquiera lo detecta.
        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, range))
        {
            if (hit.collider.CompareTag("Player"))
            {
                nextFireTime = Time.time + (1f / fireRate);
                hit.collider.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
