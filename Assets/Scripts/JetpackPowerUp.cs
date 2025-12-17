using UnityEngine;

public class JetpackPowerup : MonoBehaviour
{
    public float duration = 5f;
    public string powerupTag = "Jetpack";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerFlyController.Instance.StartFly(duration);
            
            ItemTagHolder tagHolder = GetComponent<ItemTagHolder>();
            if (tagHolder != null)
                ItemPoolManager.instance.ReturnItem(tagHolder.itemTag, gameObject);
            else
                gameObject.SetActive(false);
        }
    }
}