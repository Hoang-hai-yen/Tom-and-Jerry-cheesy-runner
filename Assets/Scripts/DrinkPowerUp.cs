using UnityEngine;

public class DrinkPowerUp : MonoBehaviour
{
    public float duration = 8f; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.ActivateBroomBoost(duration);
            }
            Destroy(gameObject);
        }
    }
}
