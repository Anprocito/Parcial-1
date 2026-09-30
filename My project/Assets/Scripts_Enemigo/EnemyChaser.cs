using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyChaser : MonoBehaviour
{
    [Header("Objetivo")]
    [SerializeField] private Transform target;

    [Header("Balance")]
    [SerializeField] private float speed = 3f;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float stoppingDistance = 1.2f;

    private Rigidbody rb;
    private EnemyHealth health;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        health = GetComponent<EnemyHealth>();
    }

    private void FixedUpdate()
    {
        // Muerto: no se mueve, pero sigue presente en la escena (no se destruye ni desactiva).
        if (health.IsDead) return;

        if (target == null)
        {
            Debug.LogWarning("Falta asignar el Target en " + name);
            return;
        }

        Vector3 toTarget = target.position - transform.position;
        toTarget.y = 0f;

        float distance = toTarget.magnitude;

        if (distance > detectionRange || distance <= stoppingDistance) return;

        Vector3 direction = toTarget.normalized;
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }

    // Dibuja el rango de deteccion en la Scene cuando el enemigo esta seleccionado.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
