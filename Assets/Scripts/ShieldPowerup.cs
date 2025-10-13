using UnityEngine;

public class ShieldPowerup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.ActivateShield();
            }

            // Hủy item sau khi nhặt
            Destroy(gameObject);
        }
    }
}