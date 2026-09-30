using UnityEngine;

// Maneja la estamina del jugador. Se regenera sola con el tiempo
// y otros scripts (como el salto) le piden consumir mediante TryConsume.
public class PlayerStamina : MonoBehaviour
{
    [Header("Estamina")]
    [SerializeField] private float maxStamina = 10f;
    [SerializeField] private float currentStamina = 10f;
    [SerializeField] private float regenPerSecond = 2f;

    public float CurrentStamina => currentStamina;
    public float MaxStamina => maxStamina;

    private void Update()
    {
        // Se regenera sola, sin pasarse del maximo.
        currentStamina = Mathf.Clamp(currentStamina + regenPerSecond * Time.deltaTime, 0f, maxStamina);
    }

    // Devuelve true y descuenta si había suficiente estamina, si no, devuelve false y no descuenta nada.
    public bool TryConsume(float amount)
    {
        if (currentStamina < amount) return false;

        currentStamina -= amount;
        return true;
    }
}
