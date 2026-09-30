using UnityEngine;

// Maneja la vida del jugador. Otros scripts consultan IsDead para
// bloquear movimiento y ataque cuando la vida llega a 0.
public class PlayerHealth : MonoBehaviour
{
    [Header("Vida")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth = 100f;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public bool IsDead => currentHealth <= 0f;

    // Publico para que, en el futuro, un enemigo o una trampa pueda restar vida.
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
