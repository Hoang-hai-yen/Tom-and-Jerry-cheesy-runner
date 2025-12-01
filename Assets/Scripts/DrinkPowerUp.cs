using UnityEngine;

public class DrinkPowerUp : MonoBehaviour
{
    public float duration = 8f;

    void OnEnable()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        if (ps != null) ps.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player != null)
            player.ActivateBroomBoost(duration);

        ItemTagHolder tagHolder = GetComponent<ItemTagHolder>();
        if (tagHolder != null && !string.IsNullOrEmpty(tagHolder.itemTag))
        {
            ItemPoolManager.instance.ReturnItem(tagHolder.itemTag, gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
