using UnityEngine;

public class MagnetPowerup : MonoBehaviour
{
    public float duration = 10f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement playerController = other.GetComponent<PlayerMovement>();
            if (playerController != null)
            {
                playerController.ActivateMagnet(duration);
                Destroy(gameObject);
            }
        }   
    }
}
