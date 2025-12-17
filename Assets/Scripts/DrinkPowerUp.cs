using UnityEngine;

public class DrinkPowerUp : MonoBehaviour
{
    public float duration = 8f;
    public Texture drinkIcon;

    void OnEnable()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        if (ps != null) ps.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
    if (PlayerFlyController.Instance != null && PlayerFlyController.Instance.IsFlying)
        {
            return;
        }
        AudioManager.Instance.PlayBuff();
        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player != null)
            player.ActivateBroomBoost(duration);

        BuffUIManager.Instance.AddBuff(drinkIcon, duration);

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
