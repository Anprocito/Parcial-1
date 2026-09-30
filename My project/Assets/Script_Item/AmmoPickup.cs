using UnityEngine;

// Item agarrable: un cubo que, al tocarlo el jugador, le suma balas y desaparece.
// El Box Collider de este objeto debe estar marcado como "Is Trigger" en el Inspector.
public class AmmoPickup : MonoBehaviour
{
    [Header("Balance")]
    [SerializeField] private int ammoAmount = 5;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // PlayerShooting vive en la Camera, hija del objeto con el tag "Player",
        // por eso se busca con GetComponentInChildren en vez de GetComponent.
        PlayerShooting shooting = other.GetComponentInChildren<PlayerShooting>();
        if (shooting == null) return;

        shooting.AddBullets(ammoAmount);
        Destroy(gameObject);
    }
}
