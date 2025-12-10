using UnityEngine;

public class ShieldPowerup : MonoBehaviour
{
    public float duration = 8f;
    public Texture shieldIcon;
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
            player.ActivateShield();

        BuffUIManager.Instance.AddBuff(shieldIcon, duration);

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
