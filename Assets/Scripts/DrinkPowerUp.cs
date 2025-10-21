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

        ItemIdentifier identifier = GetComponent<ItemIdentifier>();
        if (identifier != null && identifier.itemTag != null)
        {
            string itemTag = identifier.itemTag;
            ItemPoolManager.instance.ReturnItem(itemTag, gameObject);
        }
        else
        {
            Debug.LogWarning("Item này không có Tag Pool, đang Destroy thay vì trả về Pool.");
            Destroy(gameObject);
        }
    }
}
}
