using UnityEngine;

public class StarScoreUp : MonoBehaviour
{
    public int multiplier = 2;
    public float duration = 10f; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (ScoreManager.instance != null)
            {
                ScoreManager.instance.ActivateScoreMultiplier(multiplier, duration);
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
