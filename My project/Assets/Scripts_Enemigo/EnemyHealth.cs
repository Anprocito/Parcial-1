using UnityEngine;

// Maneja la vida del enemigo. Al llegar a 0, el objeto sigue existiendo en la escena,
// pero EnemyChaser y EnemyShooting consultan IsDead para dejar de moverse y atacar.
public class EnemyHealth : MonoBehaviour
{
    [Header("Vida")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth = 100f;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public bool IsDead => currentHealth <= 0f;

    // Llamado desde afuera (por ejemplo, PlayerShooting via SendMessage) cuando recibe daño.
    public void TakeDamage(float amount)
    {
        if (IsDead) return;

        currentHealth = Mathf.Clamp(currentHealth - amount, 0f, maxHealth);

        if (IsDead)
        {
            Debug.Log(name + " ha muerto.");
        }
    }
}
